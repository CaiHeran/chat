
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
class PlayRoom;

using User_ptr = std::shared_ptr<User>;
using PlayRoom_ptr = std::shared_ptr<PlayRoom>;

void process(User_ptr p, json info);

class GlobalRoom
{
private:
    std::set<User_ptr> users;
    std::map<int, PlayRoom_ptr> playrooms;

public:
    void join(User_ptr user)
    {
        users.insert(user);
    }

    void leave(User_ptr user)
    {
        users.erase(user);
    }

    PlayRoom_ptr create_room(User_ptr host)
    {
        int room_id = _new_id();
        auto room = std::make_shared<PlayRoom>(room_id, host);
        playrooms.emplace(room_id, room);
        return room;
    }

    PlayRoom_ptr get_room(int room_id)
    {
        try {
            return playrooms.at(room_id);
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

    void deliver(const std::string& msg)
    {
        write_msgs.push_back(msg);
        timer_.cancel_one();
        fmt::println("Deliver to {}: {}", info_.id, msg);
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
    int get_order() const noexcept
    {
        return order;
    }

    auto room()
    {
        return room_ptr;
    }

    PlayRoom_ptr create_room()
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

        global_room.join(shared_from_this());

        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->reader();},
            detached);

        co_spawn(socket_.get_executor(),
            [self = shared_from_this()] {return self->writer();},
            detached);

        deliver(fmt::format(R"({{"type":10,"id":{}}})", id()));
    }

    awaitable<void> reader()
    try {
        fmt::println("Start to read from {}", info_.id);
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
    catch (std::exception&) {
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
                fmt::println("Sending:{}", write_msgs.front());
                write_msgs.front()[write_msgs.front().size()] = '\n';
                co_await asio::async_write(socket_,
                    asio::buffer(write_msgs.front().data(), write_msgs.front().size()+1), use_awaitable);
                write_msgs.pop_front();
            }
        }
    }
    catch (std::exception&) {
        stop();
    }

    void stop()
    {
        println("{} disconnected", id());
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
    friend class PlayRoom;  // 设置order
    int num=-1;     // -1代表未进入房间
    int order=-1;   // -1代表未参加游戏
    PlayRoom_ptr room_ptr;
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

    auto [cert, prikey] = make_cert(CN);
    context_.use_certificate(asio::const_buffer(cert.c_str(), cert.size()), asio::ssl::context::pem);
    context_.use_private_key(asio::const_buffer(prikey.c_str(), prikey.size()), asio::ssl::context::pem);
    context_.use_tmp_dh(asio::const_buffer(DHparam.data(), DHparam.size()));

    while (true)
    {
        std::make_shared<User>(
            asio::ssl::stream<tcp::socket>(co_await acceptor_.async_accept(use_awaitable), context_)
        )->start();
    }
}

struct Player
{
    enum Type : bool {Human, Robot};
    Type type;
    int num;
    User_ptr part_ptr;
    Cards cards;
};

class PlayRoom
{
private:
    struct Game;
    static constexpr int host_num = 1;
    const int room_id;
    int part_cnt=0;
    std::map<int, User_ptr> parts;
    std::vector<Game> games;
    struct Game
    {
        Cards origin;
        std::vector<Player> players;
        std::vector<Operation> ops;

        Game(PlayRoom *upper, const Cards& cards, const std::vector<int>& nums)
            : origin(cards)
        {
            int n = nums.size();
            players.reserve(nums.size());
            for (int i=0, num; i<n; i++)
            {
                num = nums[i];
                players.emplace_back(Player{
                    .type = num==-1? Player::Robot : Player::Human,
                    .num = num,
                    .part_ptr = upper->parts[num]
                });
                if (num != -1)
                    upper->parts[num]->order = i;
            }
            _deal_cards(); // 然后洗牌
        }

        void _deal_cards();
        json get_gameinfo();
        void deliver();		// 给每位玩家发送房间信息和自己的牌

        void push_op(Operation op)
        {
            ops.push_back(op);
            /* TODO Robot
            int cur = ops.size() % players.size();
            */
        }

    };

public:
    PlayRoom(int room_id, User_ptr host)
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
    void deliver(const std::string& msg, const int except_id)
    {
        for (const auto& [_, p] : parts)
            if (p->id() != except_id)
                p->deliver(msg);
    }

    int join(User_ptr p)
    {
        part_cnt++;
        parts.emplace(part_cnt, p);
        json roominfo = get_roominfo();
        roominfo.emplace("type", 11);
        roominfo.emplace("num", part_cnt);
        p->deliver(roominfo.dump());

        json info {
            {"type", 11},
            {"num", part_cnt},
            {"info", json(p->info())}
        };
        deliver(info.dump(), p->id());
        return part_cnt;
    }

    void new_game(const Cards& cards, const std::vector<int>& nums)
    {
        games.emplace_back(this, cards, nums);
    }
    void new_game(json info)
    {
        Cards cards(info["cards"].get<json::array_t>());
        auto nums = info["list"].get<std::vector<int>>();
        new_game(cards, nums);
    }
    // deliver initial info with cards info
    void deliver_gameinfo()
    {
        games.back().deliver();
    }

    bool push_op(int order, json info)
    {
        Operation op(info);
        if (!push_op(order, op))
            return false;
        deliver(info);
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

private:
    bool push_op(int order, Operation op)
    {
        if (games.back().ops.size() % games.back().players.size() != order)
            return false;
        games.back().push_op(op);
        return true;
    }
    /**
private:
     * player和ops都是0起
     * games.end().ops.size() % games.end().players.size() 就是当前出牌人
     * 从games.end().ops中获取历史出牌
     */
    // Operation get_robot_op(/*水平*/);

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

void PlayRoom::Game::_deal_cards()
{
    int n = players.size();
    std::default_random_engine e(std::random_device{}());
    std::vector<std::uint8_t> cardlist;
    int total_cards = origin.count();
    int player_cards = total_cards / n;
    cardlist.reserve(total_cards);

    for (int i=0; i<9; i++)
    {
        int m = *(reinterpret_cast<int*>(&origin) + i);
        for (int j=0; j<m; j++)
            cardlist.push_back(i);
    }

    std::ranges::shuffle(cardlist, e);

    int i=0;
    for (Player &player: players)
    {
        for (int j=0; j<player_cards; i++, j++)
            ++*(reinterpret_cast<int*>(&player.cards) + cardlist[i]);
    }
}

json PlayRoom::Game::get_gameinfo()
{
    json::array_t nums;
    for (const auto& p: players)
        nums.push_back(p.num);
    json info {
        {"type", 20},
        {"list", std::move(nums)},
        {"origin", json(origin)}
    };
    return info;
}

void PlayRoom::Game::deliver()
{
    json info = get_gameinfo();
    info.emplace("self", json::array_t{});
    for (const auto& p: players)
    {
        info["self"] = json(p.cards);
        p.part_ptr->deliver(info.dump());
    }
}

void process(User_ptr p, json info)
{
    switch (info["type"].get<int>()) // info type
    {
    // 注册
    case 1: {
        p->parse_info(info);
        info.emplace("id", p->info().id);
        p->deliver(info.dump());
        break;
    }
    // 修改个人信息
    case 10: {
        p->parse_info(info);
        p->deliver(info.dump());
        break;
    }
    // 创建房间
    case 20: {
        Expects(!p->room());
        p->create_room();
        json info {
            {"type", 10},
            {"id", p->room()->id()}
        };
        p->deliver(info.dump());
        break;
    }
    // 加入房间
    case 21: {
        bool ec = ! p->join_room(info["id"].get<int>());
        if (ec) {
            json info {
                {"type", 11},
                {"ec", (int)ec}
            };
            p->deliver(info.dump());
        }
        break;
    }
    // 在房间中发送信息
    case 22: {
        //TODO check info["id"]==p->room().id();
        p->room()->deliver(info["message"]);
        break;
    }

    case 30: {
        //TODO: check whether p is the host
        p->room()->new_game(info);
        p->room()->deliver_gameinfo();
        break;
    }
    case 31: {
        int order = p->get_order();
        bool ec = ! p->room()->push_op(order, info["op"]);
        json info {
            {"type", 21},
            {"ec", (int)ec}
        };
        p->deliver(info.dump());
        break;
    }
    default:
        ;//TODO
    }
}

#endif