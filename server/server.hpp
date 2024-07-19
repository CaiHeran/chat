
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

class GlobalRoom                                                   // 服务器全局信息
{
private:
    std::set<User_ptr> users;                                      // 玩家集合
    std::map<int, Room_ptr> rooms;                                 // 房间集合（映射）

public:
    void join(User_ptr user){users.insert(user);}                  // 加入房间
    void leave(User_ptr user){users.erase(user);}                  // 退出房间
    Room_ptr create_room(User_ptr host)                            // 创建房间
    {
        int room_id = _new_id();                                   // 获取房间id
        Room_ptr room = std::make_shared<Room>(room_id, host);     // 房间初始化并获取房间
        rooms.emplace(room_id, room);                              // 存储房间信息和地址
        return room;
    }

    Room_ptr get_room(int room_id)                                 // 由id获取房间指针
    {
        try {
            return rooms.at(room_id);                              // 查询映射
        }
        catch (std::out_of_range) {return nullptr;}                // 房间位置无效
    }
private:
    static int _new_id() noexcept                                  // 生成新id
    {
        static std::atomic_int id{10};                             // ？
        return ++id;                                               // id自增为新id，随后返回
    }
} global_room;

class User : public std::enable_shared_from_this<User>             // 用户信息
{
public:
    struct Info {                                                  // 用户信息结构
        int id;
        char name[30];
        explicit operator json() const                             // 信息消息化
        {
            return json {
                {"id", id},
                {"name", std::string(name)}
            };
        }
    };

    User(asio::ssl::stream<tcp::socket> socket)                    // 新用户初始化
        : socket_(std::move(socket)),
        timer_(socket_.get_executor())
    {
        info_.id = _new_id();                                      // 为新用户获取新id
        println("{} connected, id: {}",                            // 在服务器发送通知
            socket_.lowest_layer().remote_endpoint().address().to_string(), info_.id);
        timer_.expires_at(std::chrono::steady_clock::time_point::max());
    }

    void parse_info(json info)                                     // 处理用户姓名信息
    {
        std::strcpy(info_.name, info["name"].get<std::string>().c_str());// 将用户姓名由结构转入消息中
    }

    void start()                                                   // ？？？
    {
        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->do_shake_hands();},
            detached);
    }

    template<class T>
    void deliver(T&& msg)                                              // 输出被递送的消息（下层）
    {
        write_msgs.emplace_back(std::forward<T>(msg));
        timer_.cancel_one();                                           // ？？？
        println("Deliver to {}: {}", info_.id, msg);
    }
    template<class T>
    void deliver(int type, T&& data)                    // 输出被递送的消息（上层）——（吴桐：将msg改为data）
    {
        std::string message = json{                                    // 消息由数据转为信息
            {"type", type},
            {"data", std::forward<T>(data)}
        }.dump();
        deliver(message);
    }

    const Info& info() const {return info_;}                           // 获取用户信息
    int id() const noexcept {return info_.id;}                         // 获取用户id
    int get_num() const noexcept{return num;}                          // 获取？？？
    Room_ptr room() { return room_ptr; }                               // 获取所在房间
    Room_ptr create_room()                                             // 创建房间并返回
    {
        if (room_ptr) std::terminate();
        room_ptr = global_room.create_room(shared_from_this());
        return room_ptr;
    }
    bool join_room(int room_id);                                       // 加入房间并返回是否加入成功

private:
    awaitable<void> do_shake_hands()                                   // ？？？
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

    void stop()                                                       // 用户断开连接并输出信息
    {
        println("[{}] disconnected", id());
        global_room.leave(shared_from_this());
        socket_.shutdown();
        timer_.cancel();
    }

    static int _new_id()                                              // 生成新用户id
    {
        static std::atomic_int id{10};
        return ++id;
    }

private:
    friend class Room;  // 设置order——？？？（吴桐的疑惑）
    int num = -1;       // -1代表未进入房间——？？？（吴桐的疑惑）
    Room_ptr room_ptr;  // 房间地址
    Info info_;         // 用户信息结构

    asio::ssl::stream<tcp::socket> socket_;                          // ？？？
    asio::steady_timer timer_;                                       // ？？？
    std::deque<std::string> write_msgs;                              // 输出信息的队列
};

awaitable<void> listen(tcp::acceptor acceptor_, std::string CN = "The Server")    // ？？？
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

class Room                                                           // 房间信息
{
private:
    static constexpr int host_num = 1;                               // 房主序号
    const int room_id;                                               // 本房间房间号
    int part_cnt = 0;                                                // 加入者数目
    std::map<int, User_ptr> parts;                                   // 加入者信息集合（映射）

public:
    Room(int room_id, User_ptr host)                                 // 新房间初始化
      : room_id(room_id),
        part_cnt(1),
        parts{{host_num, host}}
        {}

    int id() const { return room_id; }                               // 获取房间id
    // 给所有人发送信息
    void deliver(const std::string& msg)                             // 发送消息（底层）
    {
        for (const auto& [_, p] : parts)
            p->deliver(msg);
    }
    template<class T>
    void deliver(int type, T&& data)                   // 将type和data合并后发送消息
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

    int join(User_ptr p)                                            // 加入房间，参数为加入者地址
    {
        part_cnt++;
        parts.emplace(part_cnt, p);                   // 将用户加入
        json roominfo = get_roominfo();
        roominfo.emplace("ec", 0);
        roominfo.emplace("num", part_cnt);              // "your num"
        p->deliver(21, roominfo);

        json info {
            {"num", part_cnt},
            {"info", json(p->info())}
        };
        deliver(21, info, p->id());                          // 向服务器发送用户信息
        return part_cnt;
    }
    json get_roominfo()                                             // 房间信息转化为消息并返回
    {
        json::array_t list;
        for (auto [num, part]: parts)                  // 遍历参与者
        {
            json t {
                {"num", num},
                {"info", json(part->info())}
            };
            list.emplace_back(std::move(t));           // 将参与者信息加入list
        }
        json j {
            {"id", room_id},
            {"list", std::move(list)},
        };
        return j;
    }
};

bool User::join_room(int room_id)//见上面
{
    if (room_ptr) std::terminate();
    auto room = global_room.get_room(room_id);
    if (!room)
        return false;
    room->join(shared_from_this());
    return true;
}

void process(User_ptr p, json message)//见上面
{
    switch (message["type"].get<int>()) // info type
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
            {"type", 20},//bug fixed
            {"data", {{"id", p->room()->id()}}}
        };
        p->deliver(info.dump());
        break;
    }
    // 加入房间
    case 21: {
        json info = message["data"];
        bool ec = ! p->join_room(info["id"].get<int>());
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

    default:
        ;//TODO
    }
}

#endif