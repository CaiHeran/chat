using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Net.Security;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Threading;

namespace Server
{
    internal class User // Session
    {
        public int Id { get; private set; }
        public string Name { get; set; } = "default name";

        public Room? Room { get; set; } = null; // 进入的房间
        private readonly AsyncQueue<string> msgqueue = new();

        private QuicConnection connection;
        private QuicStream mainstream;

        public bool IsOnline { get; private set; }

        public User(QuicConnection connection, QuicStream mainstream)
        {
            this.connection = connection;
            this.mainstream = mainstream;
            Id = -get_new_id();             // 为了方便日志记录；当用户注册时取回正值，或用户登录时取登录账号的ID
            Console.WriteLine($"New session [{Id}]: {connection.RemoteEndPoint}");
            // 通常立即运行Start()
        }

        private static int _userid = 10;
        private static int get_new_id()
        {
            return ++_userid;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void ParseInfo(JsonNode info)
        {
            var name = info["name"]!.GetValue<string>();
            if (name != null)
            {
                // Check name
                Name = name;
            }
        }

        public JsonObject GetBreifInfo()
        {
            return new JsonObject(){ ["id"] = Id, ["name"] = Name };
        }

        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macOS")]
        [SupportedOSPlatform("windows")]
        public void Start()
        {
            _ = Task.Run(WriterAsync);
            // 监听新连接
            _ = Task.Run(async () => {
                try {
                    while (true)
                    {
                        QuicStream stream = await connection.AcceptInboundStreamAsync();
                        Console.WriteLine($"[{Id}] Accepted an inbound stream <{stream.Id}>.");
                        _ = Task.Run(() => RespondAsync(stream));
                    }
                }
                catch (Exception e) {
                    Console.WriteLine($"[{Id}] Stopped accepting stream. Error: {e.Message}");
                }
            });
        }

        public void Send(string message)
        {
            msgqueue.Enqueue(message);
        }

        // 使用换行符做分隔符
        private async Task WriterAsync()
        {
            Console.WriteLine($"[{Id}] Start writer");
            StreamWriter writer = new(mainstream);
            while (true)
            {
                var msg = await msgqueue.DequeueAsync();
                await writer.WriteLineAsync(msg);
                await writer.FlushAsync();
                Console.WriteLine($"[{Id}] Wrote: {msg}");
            }
        }

        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macOS")]
        [SupportedOSPlatform("windows")]
        private async Task RespondAsync(QuicStream stream)
        {
            // Respond messages sent by the client.
            // The client signals the end of the message using the
            // '\n' marker.
            Console.WriteLine($"[{Id}]<{stream.Id}> Start reader");
            StreamReader sr = new(stream);
            while (true)
            {
                string? msg = await sr.ReadLineAsync();
                Console.WriteLine($"[{Id}]<{stream.Id}> Received: {msg}");
                if (msg == null) {
                    Console.WriteLine($"Error: [{Id}]<{stream.Id}> Received null message.");
                    break;
                }
                Process.Handle(this, stream, msg);
            }
        }

        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macOS")]
        [SupportedOSPlatform("windows")]
        public async Task LogoutAsync()
        {
            await mainstream.DisposeAsync();
            mainstream = null!;
            await connection.DisposeAsync();
            connection = null!;
        }
    }

    internal static class Data
    {
        public static Dictionary<int, User> Users { get; set; } = [];
        public static Dictionary<int, Room> Rooms { get; set; } = [];

        static public User GetUser(int userid)
        {
            return Users[userid];
        }

        static public bool TryGetUser(int userid, out User? user)
        {
            return Users.TryGetValue(userid, out user);
        }

        static public Room GetRoom(int roomid)
        {
            return Rooms[roomid];
        }
        static public bool TryGetRoom(int roomid, out Room? room)
        {
            return Rooms.TryGetValue(roomid, out room);
        }

        private static int _roomid = 1;
        private static int Get_new_roomid()
        {
            return Interlocked.Increment(ref _roomid);
        }
        static public Room CreateRoom(User host)
        {
            int roomid = Get_new_roomid();
            Room newroom = new(roomid, host);
            Rooms.Add(roomid, newroom);
            return newroom;
        }

        static public void Join(User user)
        {
            Console.WriteLine($"[{user.Id}] logged in.");
            Users.Add(user.Id, user);
            user.Start();
        }

        static public void Leave(User user)
        {
            Users.Remove(user.Id);
        }
    }
}
