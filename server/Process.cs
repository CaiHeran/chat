
using Info;
using System.Text.Json;

namespace Server
{
    internal static class Process
    {
        static private SemaphoreSlim sem = new(1, 1);
        static private Queue<(User, string)> ProcessQueue = new();

        static public void Start()  // 创建一个线程处理信息
        {
            Thread t = new(new ThreadStart(Processing));
            t.Start();
        }

        static public void ProcessMessage(User user, string message)
        {
            ProcessQueue.Enqueue((user, message));
            sem.Release();
        }

        static private async void Processing()
        {
            while (true)
            {
                await sem.WaitAsync();
                while (ProcessQueue.Count > 0)
                    process(ProcessQueue.Dequeue());
            }
        }

        static private void process((User, string) pair)
        {
            User user;
            string msgstr;
            (user, msgstr) = pair;
            var infotype = JsonSerializer.Deserialize<TypeInfo>(msgstr);
            var options = new JsonSerializerOptions {
                IncludeFields = true
            };
            switch (infotype.type)
            {
            case 10: // 设置个人信息
            {
                var msg = JsonSerializer.Deserialize<UserInfo>(msgstr);
                user.Name = msg.name;
                user.Send(msgstr);
                break;
            }
            case 20: // 用户创建房间，服务器分配 room_id
            {
                // var msg = JsonSerializer.Deserialize<RoomCreate>(msgstr);
                user.TryCreateRoom();   //TODO 暂时忽略返回值，认为始终成功
                user.Send($$"""{"type":20,"ec":0,"id":{{user.Room.Id}}}""");
                break;
            }
            case 21: // 用户尝试进入房间
            {
                var msg = JsonSerializer.Deserialize<TryJoinRoom>(msgstr);
                int ec = user.TryJoinRoom(msg.id);
                if (ec != 0)
                {
                    user.Send($$"""{"type":21,"id":{{msg.id}},"ec":{{ec}}}""");
                }
                else
                {
                    JoinRoom info;
                    info.id = msg.id;
                    info.ec = 0;
                    info.num = user.Num;
                    info.parts = user.Room.GetParts();
                    user.Send(JsonSerializer.Serialize(info, options));
                }
                break;
            }
            case 22: // 在房间中发送消息
            {
                var msg = JsonSerializer.Deserialize<RoomMessage>(msgstr);
                user.Room.Deliver(msg.message);
                break;
            }
            }
        }
    }
}
