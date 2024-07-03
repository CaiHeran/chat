
#include "headers.h"

class PlayRoom;
class User;
using User_ptr = std::shared_ptr<User>;
using PlayRoom_ptr = std::shared_ptr<PlayRoom>;

class GlobalRoom
{
private:
	int room_cnt=0;
	std::set<User_ptr> parts;
	std::map<int, PlayRoom_ptr> playrooms;

public:
	void join(User_ptr part);
	void leave(User_ptr part);
	PlayRoom_ptr create_room();
	std::optional<PlayRoom_ptr> get_room(int room_id);
};

class User : public std::enable_shared_from_this<User>
{
public:
	struct Info {
		int id;
		char name[30];
	};

	User(asio::ip::tcp::socket socket);
	void start();
	void deliver(const std::string& msg);
	const Info& info() const;
	auto room();
	PlayRoom_ptr create_room();
	bool join_room(int room_id);

private:
	asio::awaitable<void> reader();
	asio::awaitable<void> writer();
	void stop();

	int order;
	Info info_;
	PlayRoom_ptr room_ptr;

	asio::ip::tcp::socket socket_;
	boost::asio::steady_timer timer_;
	std::deque<std::string> write_msgs;
};
