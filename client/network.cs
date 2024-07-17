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
        private static SslStream stream;                                              // ？
        private static AsyncQueue<string> messages = new();                           // 消息队列
        public static void Connect(string host, int port)                             // 连接服务器，参数为服务器和端口
        {
            TcpClient client = new TcpClient(host, port);                             // ？
            SslStream sslStream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
            );
            try                                                                       // 连接成功
            {
                sslStream.AuthenticateAsClient("");                                   // ？
            }
            catch (AuthenticationException e)                                         // 连接失败
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)                                         // 内部错误
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                client.Close();                                                       // 关闭连接服务器线程
                return;
            }
            stream = sslStream;                                                       // ？
            Reader();                                                                 // 开启消息接受和处理
            Writer();                                                                 // 开启消息输出
        }

        public static void Send(string message)                                       // 发送消息（下层实践）
        {
            messages.Enqueue(message);                                                // 将发送消息的事件加入队列
        }
        public static void Send(int type, string data)                                // 发送消息（上层整合）
        {
            Send($$"""{"type":{{type}},"data":{{JsonSerializer.Serialize(data)}}}""");
        }
        private static async void Writer()                                            // 输出？？？
        {
            StreamWriter writer = new(stream);                                        // ？
            while (true)
            {
                string msg = await messages.DequeueAsync();                           // 等待并获取刚处理的信息
                await writer.WriteLineAsync(msg);                                     // 输出信息并等待输出完成
                writer.Flush();                                                       // 刷新输出区
            }
        }
        private static async void Reader()                                            // 读取用户发来的信息并处理
        {
            StreamReader sr = new(stream);                                            // ？
            while (true)
            {
                StringBuilder messageData = new StringBuilder();                      // 读取信息队列
                // Read the client's test message.
                string? msg = await sr.ReadLineAsync();                               // 等待信息读取完毕
                if (msg == null) { break; }                                           // ???（蔡和然的疑问）（吴桐的猜测：用return？）
                Process.ProcessMessage(msg);                                          // 处理信息
            }
        }
        private static bool ValidateServerCertificate(                                // 验证服务器证书
                                                                           // 参数列表
                object sender,                                             // （说不清楚）
                X509Certificate certificate,                               // ？
                X509Chain chain,                                           // ？
                SslPolicyErrors sslPolicyErrors)                           // ？
        {
            if (sslPolicyErrors == SslPolicyErrors.None)                                // ？（吴桐的猜测：没有证书？）
                return true;

            string information = $"Server's certificate:\n{certificate}\nAuthenticate?";// 询问信息

            var res = MessageBox.Show($"{information}",                                 // 发起询问
              "确认",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Information);

            return res == DialogResult.Yes;                                             // 验证用户回答
        }
    }
}
