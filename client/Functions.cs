using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Client
{
    using DB = Database;

    internal static class Functions
    {
        static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            IncludeFields = true
        };

        delegate void SelfApplicable<T>(SelfApplicable<T> self, T arg1);
        static EventHandler<T> Make<T>(SelfApplicable<T> self)
        {
            return (_, x) => self(self, x);
        }

        public static void Login(string name, EventHandler<UserInfo> f)
        {
            UserInfo info;
            info.name = name;
            Server.Send(1, JsonSerializer.Serialize(info, options));
            Process.Userinfo += f;
        }

        public static void SetMyInfo(string name, EventHandler<UserInfo> f)
        {
            UserInfo info;
            info.name = name;
            Server.Send(10, JsonSerializer.Serialize(info, options));
            Process.Userinfo += f;
        }

        public static void CreateRoom(EventHandler<RoomCreate> f)
        {
            // check..
            Server.Send("""{"type":20}""");
            Process.Roomcreate += f;
        }

        public static void JoinRoom(int id, EventHandler<TryJoinRoom> f)
        {
            var msg = new TryJoinRoom { id = id };
            Server.Send(21, JsonSerializer.Serialize(msg, options));
            Process.Tryjoinroom += f;
        }

        public static void SendMessage(string message)
        {
            // check
            var msg = new RoomMessage {
                id = DB.Room!.Id,
                message = message
            };
            Server.Send(22, JsonSerializer.Serialize(msg, options));
        }
    }
}
