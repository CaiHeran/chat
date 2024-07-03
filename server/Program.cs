
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Server
{
    class Server
    {
        static async Task Main(string[] args)
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

            var cert2 = MakeCert("A server");
            Console.WriteLine("\nThis server's certificate:\n{0}", cert2);
            Listener listener = new(cert2);
            Console.WriteLine("Listening...");
            await listener.ListenAsync(port);
        }
        static void ShowIPs()
        {
            IPAddress[] IPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress IP in IPs)
                Console.WriteLine(IP.ToString());
        }
        public static X509Certificate2 MakeCert(string CN)
        {
            var rsa = RSA.Create();
            var req = new CertificateRequest("CN=" + CN, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(9));
            var buffer = cert.Export(X509ContentType.Pfx);
            return new X509Certificate2(buffer);
        }
    }
}