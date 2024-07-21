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
using System.Windows.Forms;

namespace Client
{
    using DB = Database;

    internal static class Process
    {
        static public event EventHandler<Login>? Login;
        static public event EventHandler<UserInfo>? Userinfo;
        static public event EventHandler<RoomCreate>? Roomcreate;
        static public event EventHandler<MyJoinRoom>? Myjoinroom;
        static public event EventHandler<OtherJoinRoom>? Otherjoinroom;
        static public event EventHandler<RoomMessage>? Roommessage;
        static public event EventHandler<LeaveRoom>? Leaveroom;

        static private AsyncQueue<string> ProcessQueue = new();

        static public void Start()
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
            if (msgstr[0]=='\0')
                msgstr = msgstr.Remove(0, 1);
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
                Login msg = JsonSerializer.Deserialize<Login>(jsonnode!["data"]!, options);
                DB.Me = new User(msg.id, msg.name); // 将用户数据（id和name）放入数据库
                Login?.Invoke(null, msg);
                break;
            }
            case 10: // 设置个人信息
            {
                UserInfo msg = JsonSerializer.Deserialize<UserInfo>(jsonnode!["data"]!, options);
                DB.Me.SetName(msg.name); // 将用户数据存入数据库
                Userinfo?.Invoke(null, msg);
                break;
            }
            case 20: // 用户创建房间，服务器分配 room_id
            {
                var msg = JsonSerializer.Deserialize<RoomCreate>(jsonnode!["data"]!, options);
                DB.Room = new(msg.room); // 创建房间
                Roomcreate?.Invoke(null, msg);
                break;
            }
            case 21: // 进入房间
            {
                if (DB.Room is not null) // 别人进入房间
                {
                    var msg = JsonSerializer.Deserialize<OtherJoinRoom>(jsonnode!["data"]!, options);
                    DB.Room.Join(new User(msg.info));
                    Otherjoinroom?.Invoke(null, msg);
                }
                else    // 我进入房间
                {
                    var msg = JsonSerializer.Deserialize<MyJoinRoom>(jsonnode!["data"]!, options);
                    Myjoinroom?.Invoke(null, msg);
                }
                break;
            }
            case 22: // 在房间中发送消息
            {
                var msg = JsonSerializer.Deserialize<RoomMessage>(jsonnode!["data"]!, options);// 取出消息数据
                Roommessage?.Invoke(null, msg);
                break;
            }
            case 23: // 房间成员退出房间
            {
                var msg = JsonSerializer.Deserialize<LeaveRoom>(jsonnode!["data"]!, options);// 取出消息数据
                Leaveroom?.Invoke(null, msg);
                break;
            }
            }
        }
    }
}
