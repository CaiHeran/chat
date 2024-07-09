using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Threading;

namespace Client
{
    internal static class Server
    {
        private static SslStream stream;
        private static AsyncQueue<string> messages = new();

        public static void Connect(string host, int port)
        {
            TcpClient client = new TcpClient(host, port);
            SslStream sslStream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
            );
            try
            {
                sslStream.AuthenticateAsClient("");
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                client.Close();
                return;
            }
            stream = sslStream;
            Reader();
            Writer();
        }

        public static void Send(string message)
        {
            messages.Enqueue(message);
        }
        public static void Send(int type, string data)
        {
            Send($$"""{"type":{{type}},"data":{{JsonSerializer.Serialize(data)}}}""");
        }
        private static async void Writer()
        {
            StreamWriter writer = new(stream);
            while (true)
            {
                var msg = await messages.DequeueAsync();
                await writer.WriteLineAsync(msg);
                writer.Flush();
            }
        }
        private static async void Reader()
        {
            StreamReader sr = new(stream);
            while (true)
            {
                StringBuilder messageData = new StringBuilder();
                // Read the client's test message.
                string? msg = await sr.ReadLineAsync();
                if (msg == null) { break; } // ???
                Process.ProcessMessage(msg);
            }
        }
        private static bool ValidateServerCertificate(
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            string information = $"Server's certificate:\n{certificate}\nAuthenticate?";

            var res = MessageBox.Show($"{information}",
              "确认",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Information);

            return res == DialogResult.Yes;
        }
    }
}
