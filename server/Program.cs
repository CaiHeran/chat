
using System.Net;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            /*if (args.Length == 0)
            {
                Console.WriteLine("Usage: server <port>\nPress any key to exit.");
                Console.ReadKey();
                return;
            }*/
            int port = args.Length > 0 ? int.Parse(args[0]) : 23456;
            Console.WriteLine("This server is running on port {0}", port);
            Console.WriteLine("Server's IPs:");
            ShowIPs();
        }
        static void ShowIPs()
        {
            IPAddress[] IPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress IP in IPs)
                Console.WriteLine(IP.ToString());
        }
    }
}