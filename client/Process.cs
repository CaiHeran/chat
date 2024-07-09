using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

using Info;

namespace Client
{
    using DB = Database;

    internal static class Process
    {
        static public event EventHandler<Login>? Login;
        static public event EventHandler<UserInfo>? Userinfo;
        static public event EventHandler<RoomCreate>? Roomcreate;
        static public event EventHandler<TryJoinRoom>? Tryjoinroom;

        static private AsyncQueue<string> ProcessQueue = new();

        static public void Start()  // 创建一个线程处理信息
        {
            Thread t = new(new ThreadStart(Processing));
            t.Start();
        }

        static public void ProcessMessage(string message)
        {
            ProcessQueue.Enqueue(message);
        }

        static private async void Processing()
        {
            while (true)
            {
                process(await ProcessQueue.DequeueAsync());
            }
        }

        static private void process(string msgstr)
        {
            var jsonnode = JsonNode.Parse(msgstr);
            int msgtype = (int)jsonnode!["type"]!;
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            switch (msgtype)
            {
            case 1:  // 注册
            {
                var data = (string)JsonNode.Parse((string)jsonnode!["data"]!)!;
                var msg = JsonSerializer.Deserialize<Login>(data);
                DB.Me = new User(msg.id, msg.name);
                Login?.Invoke(null, msg);
                break;
            }
            case 10: // 设置个人信息
            {
                var data = (string)JsonNode.Parse((string)jsonnode!["data"]!)!;
                var msg = JsonSerializer.Deserialize<UserInfo>(data);
                DB.Me.SetName(msg.name);
                Userinfo?.Invoke(null, msg);
                break;
            }
            case 20: // 用户创建房间，服务器分配 room_id
            {
                var data = (string)JsonNode.Parse((string)jsonnode!["data"]!)!;
                var msg = JsonSerializer.Deserialize<RoomCreate>(data);
                if (msg.ec != 0)
                {
                    //
                }
                else
                {
                    DB.Room = new(msg.id);
                }
                break;
            }
            case 21: // 用户尝试进入房间
            {
                var data = (string)JsonNode.Parse((string)jsonnode!["data"]!)!;
                var msg = JsonSerializer.Deserialize<JoinRoom>(data);
                if (msg.ec != 0)
                {
                    //
                }
                else
                {
                    DB.Room = new(msg.id, msg.num, msg.parts);
                    //
                }
                break;
            }
            case 22: // 在房间中发送消息
            {
                var data = (string)JsonNode.Parse((string)jsonnode!["data"]!)!;
                var msg = JsonSerializer.Deserialize<RoomMessage>(data);
                // TODO 通知发送成功
                break;
            }
            }
        }
    }
}
