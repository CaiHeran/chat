#include "server.hpp"

void print_ipv6_address()
{
	using asio::ip::tcp;
	asio::io_service io_service;

    tcp::resolver resolver(io_service);
    tcp::resolver::query query(asio::ip::host_name(),"");
    tcp::resolver::iterator it=resolver.resolve(query);

	fmt::println("Server IPv6 addresses: ");
    while (it!=tcp::resolver::iterator())
    {
        asio::ip::address addr = (it++)->endpoint().address();
        if (!addr.is_v6()) continue;
		auto addr6 = addr.to_v6();
		fmt::println("{}", addr6.to_string());
    }
}

int main(int argc, char* argv[])
{
	asio::ip::port_type port;
	if (argc < 2)
	{
		port = 23456;
	}
	else {
		port = std::atoi(argv[1]);
	}

	fmt::println("This server is running on port {}.", port);
try
{
	print_ipv6_address();

	asio::io_context io_context(1);

	co_spawn(io_context, listen(tcp::acceptor(io_context, {tcp::v6(), port})), detached);

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