using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Server
{
    internal class Listener
    {
        private readonly X509Certificate2 cert2;

        public Listener(X509Certificate2 cert2)
        {
            this.cert2 = cert2;
        }

        public async Task ListenAsync(int port)
        {
            TcpListener listener = new(IPAddress.IPv6Any, port);
            listener.Start();
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                var sslstream = await ProcessClient(client);
                if (sslstream is not null)
                {
                    var newuser = new User(sslstream);
                    GlobalRoom.Join(newuser);
                    newuser.Send($$"""{"type":01, "id":{{newuser.Id}}}""");
                }
            }
        }

        private async Task<SslStream?> ProcessClient(TcpClient client)
        {
            // A client has connected. Create the
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(client.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                await sslStream.AuthenticateAsServerAsync(cert2, clientCertificateRequired: true, checkCertificateRevocation: false);

                // Display the properties and settings for the authenticated stream.
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return null;
            }
            return sslStream;
        }
    }
}
