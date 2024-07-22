
#ifndef __SERVER_CPP__
#define __SERVER_CPP__

#include "headers.hpp"
#include "basics.hpp"
#include "cert.hpp"

using std::print, std::println;
using std::format;

using asio::ip::tcp;
using asio::awaitable;
using asio::co_spawn;
using asio::detached;
using asio::use_awaitable;

class GlobalRoom;
class User;
class Room;
using User_ptr = std::shared_ptr<User>;
using Room_ptr = std::shared_ptr<Room>;

void process(User_ptr p, json info);                               // 处理由客户端发来的消息

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

    // 客户端离线
    void leave(User_ptr user)
    {
        //println("[{}] departed.", user->id());
        users.erase(user);
    }

    Room_ptr create_room(User_ptr host)
    {
        int room_id = _new_id();
        Room_ptr room = std::make_shared<Room>(room_id, host);
        rooms.emplace(room_id, room);
        return room;
    }

    Room_ptr get_room(int room_id)
    {
        return rooms.contains(room_id)? rooms[room_id] : nullptr;
    }

    void delete_room(int id)
    {
        rooms.erase(id);
    }
private:
    static int _new_id() noexcept                                  // 生成新id
    {
        static std::atomic_int id{10};
        return ++id;                                               // id自增为新id，随后返回
    }
} global_room;

class User : public std::enable_shared_from_this<User>
{
public:
    struct Info {                                                  // 用户信息结构
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
        println("{} connected, id: [{}]",
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
        println("Deliver to [{}]: {}", info_.id, msg);
    }
    template<class T>
    void deliver(int type, T&& data)
    {
        std::string message = json{
            {"type", type},
            {"data", std::forward<T>(data)}
        }.dump();
        deliver(message);
    }

    const Info& info() const { return info_; }
    int id() const noexcept { return info_.id; }
    Room_ptr room() { return room_ptr; }
    Room_ptr create_room()
    {
        if (room_ptr) std::terminate();
        room_ptr = global_room.create_room(shared_from_this());
        return room_ptr;
    }
    bool join_room(int room_id);

    void leave_room(int room_id);

private:
    awaitable<void> do_shake_hands()
    {
        co_await socket_.async_handshake(asio::ssl::stream_base::server, use_awaitable);
        println("[{}] connected safely.", info_.id);

        global_room.join(shared_from_this());

        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->reader();},
            detached);

        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->writer();},
            detached);
    }

    awaitable<void> reader()                                           // 输出从客户端获取的信息并处理
    try {
        println("Start to read from [{}]", info_.id);
        for (std::string s;;)
        {
            std::size_t n = co_await asio::async_read_until(socket_,
                asio::dynamic_buffer(s, 256), '\n', use_awaitable);
            std::string_view sv(s.data(), s.data()+n-1); // 去除\r(CR)
            println("Receive from [{}]: {}", info_.id, sv);
            process(shared_from_this(), json::parse(sv));
            s.erase(0, n);
        }
    }
    catch (std::exception& e) {
        println("[{}] Exception: {}", info_.id, e.what());
        stop();
    }

    awaitable<void> writer()                                           // 输出？？？
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
                println("Sending to [{}]: {}", id(), write_msgs.front());
                co_await asio::async_write(socket_,
                    asio::buffer(write_msgs.front()), use_awaitable);
                write_msgs.pop_front();
                co_await asio::async_write(socket_, asio::buffer("\r\n"), use_awaitable);
            }
        }
    }
    catch (std::exception& e) {
        println("[{}] Exception: {}", info_.id, e.what());
        stop();
    }

    void stop();                                                      // 用户异常断开连接

    static int _new_id()
    {
        static std::atomic_int id{10};
        return ++id;
    }


private:
    Room_ptr room_ptr;  // 房间地址
    Info info_;         // 用户信息结构

    asio::ssl::stream<tcp::socket> socket_;
    asio::steady_timer timer_;
    std::deque<std::string> write_msgs;                              // 输出信息的队列
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
    print("Certificate fingerprint: ");
    for (int i = 0; i < 20; i++)
        print("{:x}{:x}", sha1[i] >> 4, sha1[i] & 0b1111);
    println("");
    context_.use_certificate(asio::const_buffer(cert.c_str(), cert.size()), asio::ssl::context::pem);
    context_.use_private_key(asio::const_buffer(prikey.c_str(), prikey.size()), asio::ssl::context::pem);
    context_.use_tmp_dh(asio::const_buffer(DHparam.data(), DHparam.size()));
    }
    println("Listening...");
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
    const int room_id;                                               // 本房间房间号
    std::map<int, User_ptr> parts;   // id -> user_ptr

public:
    Room(int room_id, User_ptr host)
      : room_id(room_id),
        parts{{host->id(), host}}
        {}

    int id() const { return room_id; }
    // 给所有人发送信息
    void deliver(const std::string& msg)
    {
        for (const auto& [_, p] : parts)
            p->deliver(msg);
    }
    template<class T>
    void deliver(int type, T&& data)
    {
        std::string message = json{
            {"type", type},
            {"data", std::forward<T>(data)}
        }.dump();
        deliver(message);
    }
    void deliver(const std::string& msg, const int except_id)
    {
        for (const auto& [_, p] : parts)
            if (p->id() != except_id)
                p->deliver(msg);
    }
    template<class T>
    void deliver(int type, T&& data, const int except_id)
    {
        std::string message = json{
            {"type", type},
            {"data", std::forward<T>(data)}
        }.dump();
        deliver(message, except_id);
    }

    void join(User_ptr p)
    {
        parts.emplace(p->id(), p);
        json roominfo = get_roominfo();
        roominfo.emplace("ec", 0);
        p->deliver(21, roominfo);

        json info {
            {"room", room_id},
            {"info", json(p->info())}
        };
        deliver(21, info, p->id());
    }

    void leave(int id)
    {
        println("[{}] left room {}", id, room_id);
        parts.erase(id);
        if (parts.empty()) {
            global_room.delete_room(room_id);
            return;
        }

        deliver(23, json{
            {"room", room_id},
            {"id", id}
        });
    }

    json get_roominfo() const
    {
        json::array_t list;
        for (auto [_, part]: parts)
        {
            json t {
                json(part->info())
            };
            list.emplace_back(std::move(t));
        }
        json j {
            {"room", room_id},
            {"list", std::move(list)},
        };
        return j;
    }
};

bool User::join_room(int room_id)
{
    Expects(!room_ptr);
    auto room = global_room.get_room(room_id);
    if (!room)
        return false;
    room_ptr = room;
    room->join(shared_from_this());
    return true;
}

void User::leave_room(int room_id)
{
    Expects(room_id == room_ptr->id());
    room_ptr->leave(id());
    room_ptr.reset();
}

void User::stop()
{
    println("[{}] disconnected", id());
    if (room_ptr)
        room_ptr->leave(id());
    global_room.leave(shared_from_this());
    socket_.shutdown();
    timer_.cancel();
}

void process(User_ptr p, json message)
{
    switch (message["type"].get<int>())
    {
    // 注册
    case 1: {
        json info = message["data"];
        p->parse_info(info);
        info.emplace("id", p->info().id);
        p->deliver(1, info);
        break;
    }
    // 修改个人信息
    case 10: {
        json info = message["data"];
        p->parse_info(info);
        p->deliver(message.dump());
        break;
    }
    // 创建房间
    case 20: {
        Expects(!p->room());
        p->create_room();
        json info {
            {"type", 20},
            {"data", {{"room", p->room()->id()}}}
        };
        p->deliver(info.dump());
        break;
    }
    // 加入房间
    case 21: {
        json info = message["data"];
        bool ec = ! p->join_room(info["room"].get<int>());
        if (ec) {
            p->deliver(21, json{{"ec", (int)ec}});
        }
        break;
    }
    // 在房间中发送信息
    case 22: {
        json info = message["data"];
        //TODO check info["id"]==p->room().id();
        p->room()->deliver(message.dump());
        break;
    }
    // 主动退出房间
    case 23: {
        json info = message["data"];
        p->leave_room(info["room"].get<int>());
        break;
    }

    default:
        ;//TODO
    }
}

#endif