using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

using Info;
using System.Windows.Interop;

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

        static public void Start()  // 创建一个线程处理消息
        {
            Processing();
        }

        // 将消息放入队列
        static public void ProcessMessage(string message)
        {
            ProcessQueue.Enqueue(message);
        }

        //等待并处理消息
        static private async void Processing()
        {
            while (true)
            {
                process(await ProcessQueue.DequeueAsync());
            }
        }

        static private void process(string msgstr)//处理由服务器发来的消息
        {
            MessageBox.Show("客户端process");
            var jsonnode = JsonNode.Parse(msgstr);//字符串消息转化为可处理数据
            int msgtype = (int)jsonnode!["type"]!;//取出消息类型
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            switch (msgtype)
            {
            case 1:  // 注册
            {
                var data = (string)jsonnode!["data"]!;
                Login msg = JsonSerializer.Deserialize<Login>(data, options);//取出消息数据
                DB.Me = new User(msg.id, msg.name);//将用户数据（id和name）放入数据库
                Login?.Invoke(null, msg);
                break;
            }
            
            case 10: // 设置个人信息
            {
                var data = (string)jsonnode!["data"]!;
                UserInfo msg = JsonSerializer.Deserialize<UserInfo>(data, options);//取出消息数据
                DB.Me.SetName(msg.name);//将用户数据放入数据库
                Userinfo?.Invoke(null, msg);

                break;
            }
            case 20: // 用户创建房间，服务器分配 room_id
            {
                var data = (string)jsonnode!["data"]!;
                RoomCreate msg = JsonSerializer.Deserialize<RoomCreate>(data, options);
                if (msg.ec != 0)
                {
                    //Todo，处理异常
                }
                else
                {
                    DB.Room = new(msg.id);//无异常，创建房间
                }
                break;
            }
            case 21: // 用户尝试进入房间
            {
                var data = (string)jsonnode!["data"]!;
                var msg = JsonSerializer.Deserialize<JoinRoom>(data, options);
                if (msg.ec != 0)
                {
                    //Todo，处理异常
                }
                else
                {
                    DB.Room = new(msg.id, msg.num, msg.parts);//无异常，加入房间
                    //Todo
                }
                break;
            }
            case 22: // 在房间中发送消息
            {
                var data = (string)jsonnode!["data"]!;
                var msg = JsonSerializer.Deserialize<RoomMessage>(data, options);// 取出消息数据
                
                break;
            }
            }
        }
    }
}
