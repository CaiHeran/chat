
#include "server.hpp"

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

    println("This server is running on port {}.", port);
try
{
    asio::io_context io_context(1);

    co_spawn(io_context, listen(tcp::acceptor(io_context, {tcp::v6(), port})), detached);

    asio::signal_set signals(io_context, SIGINT, SIGTERM);
    signals.async_wait([&](auto, auto){ io_context.stop(); });

    io_context.run();
}
catch (std::exception& e)
{
    println("Exception: {}", e.what());
}

    return 0;
}