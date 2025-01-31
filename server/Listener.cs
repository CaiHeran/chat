using System.Net;
using System.Net.Quic;
using System.Net.Security;
using System.Runtime.Versioning;
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

        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macOS")]
        [SupportedOSPlatform("windows")]
        public async Task ListenAsync(int port)
        {
            // Share configuration for each incoming connection.
            var serverConnectionOptions = new QuicServerConnectionOptions
            {
                HandshakeTimeout = new TimeSpan(0,2,0),
                IdleTimeout = new TimeSpan(0,1,0),
                MaxInboundBidirectionalStreams = 9,
                MaxInboundUnidirectionalStreams = 5,
                // Used to abort stream if it's not properly closed by the user.
                DefaultStreamErrorCode = 0x0A,
                // Used to close the connection if it's not done by the user.
                DefaultCloseErrorCode = 0x0B,
                ServerAuthenticationOptions = new SslServerAuthenticationOptions
                {
                    // Specify the application protocols that the server supports. This list must be a subset of the protocols specified in QuicListenerOptions.ApplicationProtocols.
                    ApplicationProtocols = [new SslApplicationProtocol("QUIC")],
                    // Server certificate, it can also be provided via ServerCertificateContext or ServerCertificateSelectionCallback.
                    ServerCertificate = cert2
                }
            };
            QuicListener listener4 = await QuicListener.ListenAsync(new QuicListenerOptions
            {
                // Define the endpoint on which the server will listen for incoming connections. The port number 0 can be replaced with any valid port number as needed.
                ListenEndPoint = new IPEndPoint(IPAddress.Any, port),
                // List of all supported application protocols by this listener.
                ApplicationProtocols = [new SslApplicationProtocol("QUIC")],
                // Callback to provide options for the incoming connections, it gets called once per each connection.
                ConnectionOptionsCallback = (_, _, _) => ValueTask.FromResult(serverConnectionOptions)
            });
            QuicListener listener6 = await QuicListener.ListenAsync(new QuicListenerOptions
            {
                ListenEndPoint = new IPEndPoint(IPAddress.IPv6Any, port),
                ApplicationProtocols = [new SslApplicationProtocol("QUIC")],
                ConnectionOptionsCallback = (_, _, _) => ValueTask.FromResult(serverConnectionOptions)
            });

            while (true)
            {
                QuicConnection connection;
                QuicStream mainstream;
                try {
                    var connectionTask = await Task.WhenAny(listener4.AcceptConnectionAsync().AsTask(), listener6.AcceptConnectionAsync().AsTask());
                    connection = await connectionTask;
                    Console.WriteLine($"New Connection from {connection.RemoteEndPoint}");
                    mainstream = await connection.AcceptInboundStreamAsync();
                    byte[] buffer = new byte[4];
                    await mainstream.ReadExactlyAsync(buffer);  // 客户端版本号
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                new User(connection, mainstream).Start();
            }
        }
    }
}
