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
using System.Threading.Tasks;

namespace Client
{
    internal static class Server
    {
        private static SslStream stream;
        private static SemaphoreSlim msg_sem = new(1,1);
        private static Queue<string> messages = new();

        public static void Connect(string host, int port)
        {
            TcpClient client = new TcpClient(host, port);
            SslStream sslStream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
                );
            // The server name must match the name on the server certificate.
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
                Console.WriteLine("Authentication failed - closing the connection.");
                client.Close();
                Environment.Exit(0);
                return;
            }
            stream = sslStream;
            Reader();
            Writer();
        }

        public static void Send(string message)
        {
            messages.Enqueue(message);
            msg_sem.Release();
        }
        private static async void Writer()
        {
            StreamWriter writer = new(stream);
            while (true)
            {
                await msg_sem.WaitAsync();
                while (messages.Count > 0)
                {
                    await writer.WriteLineAsync(messages.Dequeue());
                }
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

            Console.WriteLine("Server's certificate:\n{0}\n", certificate);
            Console.Write("Authenticate? y/n ");
        _Check:
            switch (Console.ReadKey().Key)
            {
            case ConsoleKey.Y: return true;
            case ConsoleKey.N: return false;
            default: goto _Check;
            }
        }
    }
}
