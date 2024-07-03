
#include "server.hpp"

void print_ipv6_address()
{
	using asio::ip::tcp;
	asio::io_service io_service;

    tcp::resolver resolver(io_service);
    tcp::resolver::query query(asio::ip::host_name(),"");
    tcp::resolver::iterator it=resolver.resolve(query);

    while (it!=tcp::resolver::iterator())
    {
        asio::ip::address addr = (it++)->endpoint().address();
        if (!addr.is_v6()) continue;
		auto addr6 = addr.to_v6();
        std::cout << "Local IPv6 address: " << addr6 << std::endl;
    }
}

int main(int argc, char* argv[])
{
try
{
	if (argc < 2)
	{
		std::cerr << "Usage: server <port>\n";
		return 1;
	}

	print_ipv6_address();

	asio::io_context io_context(1);

	asio::ip::port_type port = std::atoi(argv[1]);

	co_spawn(io_context,
		listener(tcp::acceptor(io_context, {tcp::v6(), port})),
		detached);
	co_spawn(io_context,
		listener(tcp::acceptor(io_context, {tcp::v4(), port})),
		detached);

	asio::signal_set signals(io_context, SIGINT, SIGTERM);
	signals.async_wait([&](auto, auto){ io_context.stop(); });

	io_context.run();
}
catch (std::exception& e)
{
	std::cerr << "Exception: " << e.what() << "\n";
}

	return 0;
}