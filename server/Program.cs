using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Versioning;

namespace Server
{
    class Server
    {
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macOS")]
        [SupportedOSPlatform("windows")]
        static async Task Main(string[] args)
        {
            int port;
            if (args.Length == 0)
            {
                while (true)
                {
                    Console.Write("Port(1024~65535): ");
                    var input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input) || !int.TryParse(input, out port)
                        || port <= 0 || port > 65535)
                        Console.WriteLine("Illegal port number.");
                    else
                        break;
                }
            }
            else
            {
                if (!int.TryParse(args[0], out port) || port < 0 || port > 65535)
                {
                    Console.WriteLine("Illegal port number");
                    return;
                }
            }

            Console.WriteLine($"This server is listening on port {port}");
            Console.WriteLine("This server's IP addresses:");
            ShowIPs();

            var cert2 = CreateCert("A server");
            Console.WriteLine($"\nThis server's certificate:\n{cert2}");
            Listener listener = new(cert2);
            Console.WriteLine("Listening...");
            Process.Start();
            await listener.ListenAsync(port);
        }
        static void ShowIPs()
        {
            IPAddress[] IPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress IP in IPs)
                Console.WriteLine(IP.ToString());
        }
        public static X509Certificate2 CreateCert(string CN)
        {
            var rsa = RSA.Create();
            var req = new CertificateRequest("CN=" + CN, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert2 = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(9));
            var buffer = cert2.Export(X509ContentType.Pfx);
            return new X509Certificate2(buffer);
        }
    }
}