using System;
using System.Net;
using System.Net.Quic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Threading;

namespace Client
{
    internal static class Server
    {
        private static QuicConnection? connection;
        private static QuicStream? mainstream;
        private static QuicStream? httpstream;
        public static bool IsConnected {
            get {
                return connection != null;
            }
        }
        public static async Task<bool> ConnectAsync(string hostname, int port) // 连接服务器，参数为服务器和端口
        {
            var clientConnectionOptions = new QuicClientConnectionOptions
            {
                RemoteEndPoint = new DnsEndPoint(hostname, port),
                // Used to abort stream if it's not properly closed by the user.
                DefaultStreamErrorCode = 0x0A,
                // Used to close the connection if it's not done by the user.
                DefaultCloseErrorCode = 0x0B,
                // Optionally set limits for inbound streams.
                MaxInboundUnidirectionalStreams = 5,
                MaxInboundBidirectionalStreams = 9,
                HandshakeTimeout = new TimeSpan(0,3,0),
                IdleTimeout = TimeSpan.MaxValue,
                KeepAliveInterval = new TimeSpan(0, 0, 55),
                // Same options as for client side SslStream.
                ClientAuthenticationOptions = new SslClientAuthenticationOptions
                {
                    // List of supported application protocols.
                    ApplicationProtocols = [new SslApplicationProtocol("QUIC")],
                    // The name of the server the client is trying to connect to. Used for server certificate validation.
                    TargetHost = hostname,
                    RemoteCertificateValidationCallback = new(ValidateServerCertificate)
                }
            };

            // Initialize, configure and connect to the server.
            connection = await QuicConnection.ConnectAsync(clientConnectionOptions);

            mainstream = await connection.OpenOutboundStreamAsync(QuicStreamType.Bidirectional);
            await mainstream.WriteAsync(new byte[4], 0, 4);     // 发送客户端版本号
            mainstream.CompleteWrites();
            httpstream = await connection.OpenOutboundStreamAsync(QuicStreamType.Bidirectional);

            _ = Task.Run(ReaderAsync);
            Process.Start();
            return true;
        }

        // 读取服务器主动推送的信息
        private static async Task ReaderAsync()
        {
            try
            {
                StreamReader sr = new(mainstream!);
                while (true)
                {
                    string? msg = await sr.ReadLineAsync().ConfigureAwait(false);
                    if (msg == null)
                    {
                        if (!mainstream!.CanRead)
                            throw new Exception("Connection closed.");
                        continue;
                    }
                    Process.PushMessage(msg);
                }
            }
            catch (Exception)
            {
                Reconnect();
            }
        }

        public static async Task WriteAsync(string message)
        {
            await httpstream!.WriteAsync(Encoding.UTF8.GetBytes(message + '\n'));
        }

        public static async Task SendAsync(string message)
        {
            await WriteAsync(message);
            await httpstream!.FlushAsync();
        }

        public static async Task<JsonNode> PostAsync(int type, string message)
        {
            await SendAsync(message);
            StreamReader sr = new(httpstream!);
            while (true)
            {
                string? msg = await sr.ReadLineAsync();
                if (msg == null) {
                    if (!httpstream.CanRead)
                        throw new Exception("Stream closed.");
                    continue;
                }
                if (msg.Length == 0)
                    continue;
                JsonNode json = JsonNode.Parse(msg)!;
                if (json["_type"]!.GetValue<int>() != type)
                    MessageBox.Show(msg);
                return json;
            }
        }
        public static async Task<JsonNode> PostAsync(JsonObject json)
        {
            var typenode = json["_type"];
            if (typenode is null)
                throw new ArgumentException("No _type field in the message.", nameof(json));
            return await PostAsync(typenode.GetValue<int>(), json.ToJsonString());
        }
        /*
        private static async Task WriterAsync()
        {
            try {
                StreamWriter writer = new(mainstream!);
                while (true)
                {
                    string msg = await messages.DequeueAsync();                // 等待并获取刚处理的信息
                    await writer.WriteLineAsync(msg);                          // 输出信息并等待输出完成
                    await writer.FlushAsync();                                 // 刷新缓冲区
                }
            }
            catch (Exception e)
            {
                Reconnect();
            }
        }
        */
        private static void Reconnect()
        {
            //TODO: Reconnect
        }
        public static async Task DisconnectAsync()
        {
            await connection.CloseAsync(0x0C);
            await connection.DisposeAsync();
        }
        private static bool ValidateServerCertificate(                         // 自定义验证服务器证书
                object sender,
                X509Certificate? certificate,
                X509Chain? chain,
                SslPolicyErrors errors)
        {
            string information;
            if (errors == SslPolicyErrors.None)
                information = "The server's certificate is valid.\n" +
                             $"Content:\n{certificate}\nConnect?";

            else if (errors == SslPolicyErrors.RemoteCertificateNotAvailable)
                information = "The server's certificate is not available.\n" +
                             $"Content:\n{certificate}\nConnect?";

            else {
                information = $"Errors: {errors}\n";

                if (0 < (errors | SslPolicyErrors.RemoteCertificateNameMismatch))
                    information += "The server's name does not match the certificate.\n";

                if (0 < (errors | SslPolicyErrors.RemoteCertificateChainErrors))
                    information += "The certificate chain validation failed.\n";
                
                information += $"The server's certificate:\n{certificate}\nConnect?";
            }

            var res = MessageBox.Show(
                $"{information}",
                "连接确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            return res == DialogResult.Yes;
        }
    }
}
