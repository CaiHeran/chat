using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server
{
    internal class User
    {
        private SslStream stream;
        private SemaphoreSlim msg_sem = new(1, 1);
        private Queue<string> messages = new();

        public int Id { get; private set; }
        public string Name { get; set; } = "default name";

        public int Num { get; private set; } = -1; // 处于房间中的序号，-1表示未进入任何房间
        public Room? Room { get; private set; } = null; // 进入的房间

        public User(SslStream stream)
        {
            this.stream = stream;
            Id = get_new_id();
        }

        private static int get_new_id()
        {
            int userid = 10;
            return ++userid;
        }

        public void ParseInfo(string info)
        {
            //TODO
        }

        public void Start()
        {
            Reader();
            Writer();
        }

        public void Send(string message)
        {
            messages.Enqueue(message);
            msg_sem.Release();
        }

        public int TryJoinRoom(int roomid)
        {
            if (this.Num != -1) return 1;
            Room? room;
            if (!GlobalRoom.TryGetRoom(roomid, out room))
                return 2;   // 不存在此房间
            int num = room.Join(this);
            if (num == -1)
            {
                return 3;
            }
            else
            {
                Num = num;
                return 0;
            }
        }
        public bool TryCreateRoom()
        {
            if (this.Room is not null)
                return false;
            this.Room = GlobalRoom.CreateRoom(this);
            Num = 0;
            return true;
        }
        /* 以后再写
        public void LeaveRoom()
        {
            if (Num == 0)   // 房主退出房间即解散房间
            {
            }
            Num = -1;
            this.Room = null;
        }*/

        // 使用换行符做分隔符
        private async void Writer()
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

        private async void Reader()
        {
            // Read the message sent by the client.
            // The client signals the end of the message using the
            // '\n' marker.
            StreamReader sr = new(stream);
            while (true)
            {
                StringBuilder messageData = new StringBuilder();
                // Read the client's test message.
                string? msg = await sr.ReadLineAsync();
                if (msg == null) { break; } // ???
                Process.ProcessMessage(this, msg);
            }
        }
    }

    internal static class GlobalRoom
    {
        static private Dictionary<int, User> users = new();
        static private Dictionary<int, Room> rooms = new();

        static public void Join(User user)
        {
            users.Add(user.Id, user);
            user.Start();
        }

        static public void Leave(User user)
        {
            users.Remove(user.Id);
        }

        static public User GetUser(int userid)
        {
            return users[userid];
        }

        static public Room CreateRoom(User host)
        {
            int roomid = 0;
            Room newroom = new(++roomid, host);
            rooms.Add(roomid, newroom);
            return newroom;
        }

        static public Room GetRoom(int roomid)
        {
            return rooms[roomid];
        }
        static public bool TryGetRoom(int roomid, out Room? room)
        {
            return rooms.TryGetValue(roomid, out room);
        }
    }
}
