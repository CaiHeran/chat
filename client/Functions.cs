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

        public static void Login(string name)
        {
            Server.Send(1, $$"""{"name":"{{name}}"}""");
        }

        public static void SetMyInfo(string name)
        {
            UserInfo info;
            info.name = name;
            Server.Send(10, JsonSerializer.Serialize(info, options));
        }

        public static void CreateRoom()
        {
            // check..
            Server.Send("""{"type":20}""");
        }

        public static void JoinRoom(int id)
        {
            var msg = new TryJoinRoom { id = id };
            Server.Send(21, JsonSerializer.Serialize(msg, options));
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
