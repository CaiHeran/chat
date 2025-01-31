
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Concurrent;

using Microsoft.VisualStudio.Threading;
using System.Net.Quic;
using System.Text;
using System.Runtime.Versioning;

namespace Server
{
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macOS")]
    [SupportedOSPlatform("windows")]
    internal static class GeneralFunctions
    {
        private static async Task SendAsync(User user, QuicStream stream, string msg)
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes(msg+'\n'));
            Console.WriteLine($"[{user.Id}]<{stream.Id}> Wrote: {msg}");
        }
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        static public async Task Register(User user, QuicStream stream, JsonNode json)
        {
            if (user.Id > 0) {
                await SendAsync(user, stream, $$"""{"_type":1,"errors":"1"}""");
                return;
            }
            string name = json["name"]!.GetValue<string>();
            // Check name
            user.Name = name;
            user.SetId(-user.Id);
            Console.WriteLine($"[-{user.Id}] --> [{user.Id}] `{user.Name}`");
            Data.Join(user);
            await SendAsync(user, stream, $$"""{"_type":1,"id":{{user.Id}}}""");
        }
        static public async Task Login(User user, QuicStream stream, JsonNode json)
        {
        }
        static public async Task SetUserInfo(User user, QuicStream stream, JsonNode json)
        {
            user.ParseInfo(json);
            await SendAsync(user, stream, """{"type":12,"success":true""");
        }
        static public async Task CreateRoom(User user, QuicStream stream, JsonNode json)
        {
            if (user.Room != null)
            {
                await SendAsync(user, stream, $$"""{"_type":20,"errors":"1"}""");
                return;
            }
            user.Room = Data.CreateRoom(user);
            await SendAsync(user, stream, $$"""{"_type":20,"roomid":{{user.Room.Id}}}""");
        }
        static public async Task JoinRoom(User user, QuicStream stream, JsonNode json)
        {
            if (user.Room != null) {
                await SendAsync(user, stream, $$"""{"_type":21,"errors":"1"}""");
                return;
            }
            var roomid_node = json["roomid"];
            if (roomid_node is null) {
                await SendAsync(user, stream, $$"""{"_type":21,"errors":"2"}""");
                return;
            }
            if (!Data.TryGetRoom(roomid_node.GetValue<int>(), out Room? room)) {
                await SendAsync(user, stream, $$"""{"_type":21,"errors":"3"}""");
                return;
            }
            room.Join(user);
            user.Room = room;
            await SendAsync(user, stream, new JsonObject() {
                { "_type", 21 },
                { "roomid", room.Id },
                { "parts", room.GetPartsInfo()}
            }.ToJsonString());
            room.Deliver(new JsonObject() {
                { "_type", 21 },
                { "roomid", room.Id },
                { "info", user.GetBreifInfo() }
            }.ToJsonString());
        }
        static public async Task RoomMessage(User user, QuicStream stream, JsonNode json)
        {
            if (user.Room is null) {
                await SendAsync(user, stream, $$"""{"_type":22,"errors":"1"}""");
                return;
            }
            var roomid_node = json["roomid"];
            if (roomid_node is null || json["time"] is null || json["message"] is null) {
                await SendAsync(user, stream, $$"""{"_type":22,"errors":"2"}""");
                return;
            }
            if (user.Room.Id != roomid_node.GetValue<int>()) {
                await SendAsync(user, stream, $$"""{"_type":22,"errors":"3"}""");
                return;
            }
            await SendAsync(user, stream, $$"""{"_type":22,"success":true}""");
            user.Room.Deliver(new JsonObject() {
                ["_type"] = 22,
                ["time"] = json["time"].GetValue<DateTime>(),
                ["roomid"] = user.Room.Id,
                ["userid"] = user.Id,
                ["message"] = json["message"].GetValue<string>()
            }.ToJsonString());
        }
        static public async Task LeaveRoom(User user, QuicStream stream, JsonNode json)
        {
            if (user.Room is null) {
                await SendAsync(user, stream, $$"""{"_type":29,"errors":"1"}""");
                return;
            }
            var roomid_node = json["roomid"];
            if (roomid_node is null) {
                await SendAsync(user, stream, $$"""{"_type":29,"errors":"2"}""");
                return;
            }
            if (user.Room.Id != roomid_node.GetValue<int>()) {
                await SendAsync(user, stream, $$"""{"_type":29,"errors":"3"}""");
                return;
            }
            user.Room.Leave(user);
            user.Room.Deliver(new JsonObject() {
                ["_type"] = 29,
                ["roomid"] = user.Room.Id,
                ["userid"] = user.Id
            }.ToJsonString());
            user.Room = null;
            await SendAsync(user, stream, $$"""{"_type":29,"roomid":{{roomid_node.GetValue<int>()}}}""");
        }
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
    }

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macOS")]
    [SupportedOSPlatform("windows")]
    internal static class Process
    {
        static public ConcurrentDictionary<int, ConcurrentBag<Func<User, QuicStream, JsonNode, Task>>>
            EventHandlers = new();
        static private readonly AsyncQueue<(User, QuicStream, string)> ProcessQueue = new();

        static public void Start()  // 创建线程处理信息
        {
            RegisterHandler(1, GeneralFunctions.Register);
            RegisterHandler(10, GeneralFunctions.SetUserInfo);
            RegisterHandler(20, GeneralFunctions.CreateRoom);
            RegisterHandler(21, GeneralFunctions.JoinRoom);
            RegisterHandler(22, GeneralFunctions.RoomMessage);
            RegisterHandler(29, GeneralFunctions.LeaveRoom);
            _ = Task.Run(async () => { while (true) Handle(await ProcessQueue.DequeueAsync()); });
        }

        static public void PushMessage(User user, QuicStream stream, string message)
        {
            ProcessQueue.Enqueue((user, stream, message));
        }

        static public void RegisterHandler(int type, Func<User, QuicStream, JsonNode, Task> handler)
        {
            EventHandlers.AddOrUpdate(type, [ handler ], (key, old) => { old.Add(handler); return old; });
        }

        static public void Handle(User user, QuicStream stream, string msgstr)
        {
            JsonNode? jsonnode = JsonNode.Parse(msgstr);
            if (jsonnode is null)
                return;
            var typenode = jsonnode["_type"];
            if (typenode is null)
                return;

            var infotype = typenode.GetValue<int>();

            if (EventHandlers.TryGetValue(infotype, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    _ = Task.Run(async () => await handler(user, stream, jsonnode));
                }
            }
        }

        static private void Handle((User, QuicStream, string) tuple)
        {
            Handle(tuple.Item1, tuple.Item2, tuple.Item3);
        }
    }
}
