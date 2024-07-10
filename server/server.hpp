
#ifndef __SERVER_CPP__
#define __SERVER_CPP__

#include "headers.hpp"
#include "basics.hpp"
#include "cert.hpp"

using fmt::print, fmt::println;

using asio::ip::tcp;
using asio::awaitable;
using asio::co_spawn;
using asio::detached;
using asio::use_awaitable;

class GlobalRoom;
class User;
struct Player;
class Room;

using User_ptr = std::shared_ptr<User>;
using Room_ptr = std::shared_ptr<Room>;

void process(User_ptr p, json info);

class GlobalRoom
{
private:
    std::set<User_ptr> users;
    std::map<int, Room_ptr> rooms;

public:
    void join(User_ptr user)
    {
        users.insert(user);
    }

    void leave(User_ptr user)
    {
        users.erase(user);
    }

    Room_ptr create_room(User_ptr host)
    {
        int room_id = _new_id();
        auto room = std::make_shared<Room>(room_id, host);
        rooms.emplace(room_id, room);
        return room;
    }

    Room_ptr get_room(int room_id)
    {
        try {
            return rooms.at(room_id);
        }
        catch (std::out_of_range) {
            return nullptr;
        }
    }
private:
    static int _new_id() noexcept
    {
        static std::atomic_int id{10};
        return ++id;
    }
} global_room;

class User : public std::enable_shared_from_this<User>
{
public:
    struct Info {
        int id;
        char name[30];

        explicit operator json() const
        {
            return json {
                {"id", id},
                {"name", std::string(name)}
            };
        }
    };

    User(asio::ssl::stream<tcp::socket> socket)
      : socket_(std::move(socket)),
        timer_(socket_.get_executor())
    {
        info_.id = _new_id();
        fmt::println("{} connected, id: {}",
            socket_.lowest_layer().remote_endpoint().address().to_string(), info_.id);
        timer_.expires_at(std::chrono::steady_clock::time_point::max());
    }

    void parse_info(json info)
    {
        std::strcpy(info_.name, info["name"].get<std::string>().c_str());
    }

    void start()
    {
        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->do_shake_hands();},
            detached);
    }

    template<class T>
    void deliver(T&& msg)
    {
        write_msgs.emplace_back(std::forward<T>(msg));
        timer_.cancel_one();
        fmt::println("Deliver to {}: {}", info_.id, msg);
    }
    void deliver(int type, const std::string& msg)
    {
        std::string message = json{
            {"type", type},
            {"data", msg}
        }.dump();
        deliver(message);
    }

    const Info& info() const
    {
        return info_;
    }

    int id() const noexcept
    {
        return info_.id;
    }
    
    int get_num() const noexcept
    {
        return num;
    }

    auto room()
    {
        return room_ptr;
    }

    Room_ptr create_room()
    {
        if (room_ptr) std::terminate();
        room_ptr = global_room.create_room(shared_from_this());
        return room_ptr;
    }

    bool join_room(int room_id);

private:
    awaitable<void> do_shake_hands()
    {
        co_await socket_.async_handshake(asio::ssl::stream_base::server, use_awaitable);
        fmt::println("[{}] connected safely.", info_.id);

        global_room.join(shared_from_this());

        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->reader();},
            detached);

        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->writer();},
            detached);
    }

    awaitable<void> reader()
    try {
        fmt::println("Start to read from [{}]", info_.id);
        for (std::string s;;)
        {
            std::size_t n = co_await asio::async_read_until(socket_,
                asio::dynamic_buffer(s, 256), '\n', use_awaitable);
            std::string_view sv(s.data(), s.data()+n);
            fmt::println("Receive from {}: {}", info_.id, sv);
            process(shared_from_this(), json::parse(sv));
            s.erase(0, n);
        }
    }
    catch (std::exception& e) {
        fmt::println("[{}] Exception: {}", info_.id, e.what());
        stop();
    }

    awaitable<void> writer()
    try {
        while (socket_.lowest_layer().is_open())
        {
            if (write_msgs.empty())
            {
                asio::error_code ec;
                co_await timer_.async_wait(asio::redirect_error(use_awaitable, ec));
            }
            else
            {
                fmt::println("Sending to [{}]:{}", id(), write_msgs.front());
                co_await asio::async_write(socket_,
                    asio::buffer(write_msgs.front()), use_awaitable);
                write_msgs.pop_front();
                co_await asio::async_write(socket_, asio::buffer("\n"), use_awaitable);
            }
        }
    }
    catch (std::exception& e) {
        fmt::println("[{}] Exception: {}", info_.id, e.what());
        stop();
    }

    void stop()
    {
        println("[{}] disconnected", id());
        global_room.leave(shared_from_this());
        socket_.shutdown();
        timer_.cancel();
    }

    static int _new_id()
    {
        static std::atomic_int id{10};
        return ++id;
    }

private:
    friend class Room;  // 设置order
    int num=-1;     // -1代表未进入房间
    Room_ptr room_ptr;
    Info info_;

    asio::ssl::stream<tcp::socket> socket_;
    asio::steady_timer timer_;
    std::deque<std::string> write_msgs;
};

awaitable<void> listen(tcp::acceptor acceptor_, std::string CN = "The Server")
{
    constexpr std::string_view DHparam =
R"(-----BEGIN DH PARAMETERS-----
MIICDAKCAgEAwqeHU/77gDqJ3XkFYjLNEDpVomUkbffJnsQxQ6saSAhmu18IEW6A
eCg9FWGJZgmLxg2fj6S8nGvkv/8wXWRl0+GYiKOqOTBsqTI+i5YSvQalzvkoMt6g
GInOred27t7pw/W1NY8Kt0vijRc+2s+tdep3wPlsCmfTWaztygXE65iWhhMYmFda
TacRh1M7ruEqRNVuiT62HpGMremEEPLOXHNv59wVatU1MimYjMnpa4+8N5NRz6ON
llV+ICnZvh5oFdSZ3KMQ/rNHkjwAbhxJt5SCiOduwzhohrqeSz2Y5XlKWU/G8PeD
AONYX3sahhO3mWnji91EtvaBDS/V24sFuLJgH/WlA7H/BxFAdH1VM7ELrik1PAJ/
Fwx3vQkX8z3wiW+oPzECbIlOPGHUHdlo7eT/dK5iRpCbQrVNfjA3sUb6VSURW3Pk
an+2IU7h8dssboaa4lHOw1Psf6/37RSoQSgZRLHb9/gijnUXywVqfJSL5AFIxL1B
UkANOCgI6LfgOkAnDCm/C3DeLKd40ScX4gxWKfUvegsUaPH+WGXlEVSht89OLWKD
lUSIRWJsJ6RFtCiY58x7Cj7kR7OihdehNDRMfNa1M2xrsiJiPtvoEqDaWgxDjyza
NuOJ1xk1CWjjrYhJNtWK6OMUpgNrTIJpOKwvlHy8b6MPgJSOubxnEE8CAQICAgFF
-----END DH PARAMETERS-----
)";
    asio::ssl::context context_(asio::ssl::context::tls);

    context_.set_options(
        asio::ssl::context::default_workarounds
        | asio::ssl::context::no_tlsv1
        | asio::ssl::context::no_tlsv1_1
        | asio::ssl::context::single_dh_use);
    {
    auto [cert, prikey, sha1] = make_cert(CN);
    fmt::print("Certificate fingerprint: ");
    for (int i = 0; i < 20; i++)
        fmt::print("{:x}{:x}", sha1[i] >> 4, sha1[i] & 0b1111);
    fmt::println("");
    context_.use_certificate(asio::const_buffer(cert.c_str(), cert.size()), asio::ssl::context::pem);
    context_.use_private_key(asio::const_buffer(prikey.c_str(), prikey.size()), asio::ssl::context::pem);
    context_.use_tmp_dh(asio::const_buffer(DHparam.data(), DHparam.size()));
    }
    fmt::println("Listening...");
    while (true)
    {
        std::make_shared<User>(
            asio::ssl::stream<tcp::socket>(co_await acceptor_.async_accept(use_awaitable), context_)
        )->start();
    }
}

class Room
{
private:
    static constexpr int host_num = 1;
    const int room_id;
    int part_cnt=0;
    std::map<int, User_ptr> parts;

public:
    Room(int room_id, User_ptr host)
      : room_id(room_id),
        part_cnt(1),
        parts{{host_num, host}}
        {}

    int id() const { return room_id; }

    // 给所有人发送信息
    void deliver(const std::string& msg)
    {
        for (const auto& [_, p] : parts)
            p->deliver(msg);
    }
    void deliver(int type, const std::string& msg)
    {
        std::string message = json{
            {"type", type},
            {"data", msg}
        }.dump();
        deliver(message);
    }
    void deliver(const std::string& msg, const int except_id)
    {
        for (const auto& [_, p] : parts)
            if (p->id() != except_id)
                p->deliver(msg);
    }
    void deliver(int type, const std::string& msg, const int except_id)
    {
        std::string message = json{
            {"type", type},
            {"data", msg}
        }.dump();
        deliver(message, except_id);
    }

    int join(User_ptr p)
    {
        part_cnt++;
        parts.emplace(part_cnt, p);
        json roominfo = get_roominfo();
        roominfo.emplace("num", part_cnt);
        p->deliver(21, roominfo.dump());

        json info {
            {"num", part_cnt},
            {"info", json(p->info())}
        };
        deliver(21, info.dump(), p->id());
        return part_cnt;
    }

    json get_roominfo()
    {
        json::array_t list;
        for (auto [num, part]: parts)
        {
            auto &info = part->info();
            json t {
                {"num", num},
                {"id", info.id},
                {"name", info.name}
            };
            list.emplace_back(std::move(t));
        }
        json j {
            {"id", room_id},
            {"host", host_num},
        };
        j.emplace("list", std::move(list));

        return j;
    }
};

bool User::join_room(int room_id)
{
    if (room_ptr) std::terminate();
    auto room = global_room.get_room(room_id);
    if (!room)
        return false;
    room->join(shared_from_this());
    return true;
}


void process(User_ptr p, json message)
{
    switch (message["type"].get<int>()) // info type
    {
    // 注册
    case 1: {
        json info = json::parse(message["data"].get<std::string>());
        p->parse_info(info);
        info.emplace("id", p->info().id);
        p->deliver(1, info.dump());
        break;
    }
    // 修改个人信息
    case 10: {
        json info = json::parse(message["data"].get<std::string>());
        p->parse_info(info);
        p->deliver(message.dump());
        break;
    }
    // 创建房间
    case 20: {
        Expects(!p->room());
        p->create_room();
        json info {
            {"type", 10},
            {"data", fmt::format(R"({{"id":"{}"}})", p->room()->id())}
        };
        p->deliver(info.dump());
        break;
    }
    // 加入房间
    case 21: {
        json info = json::parse(message["data"].get<std::string>());
        bool ec = ! p->join_room(info["id"].get<int>());
        if (ec) {
            p->deliver(21, fmt::format(R"({{"ec":{}}})", (int)ec));
        }
        //TODO
        break;
    }
    // 在房间中发送信息
    case 22: {
        std::string data = message["data"].get<std::string>();
        json info = json::parse(data);
        //TODO check info["id"]==p->room().id();
        p->room()->deliver(message.dump());
        break;
    }

    default:
        ;//TODO
    }
}

#endif