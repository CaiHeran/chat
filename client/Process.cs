using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Forms;

using Microsoft.VisualStudio.Threading;

namespace Client
{
    using DB = Database;

    internal static class Process
    {
        private static readonly ConcurrentDictionary<int, ConcurrentDictionary<Action<JsonNode>, byte>> EventHandlers = new();

        private static readonly AsyncQueue<string> MessageQueue = new();

        static public void Start()
        {
            _ = Task.Run(async () => { while (true) Handle(await MessageQueue.DequeueAsync()); });
        }

        // 将消息放入队列
        static public void PushMessage(string message)
        {
            MessageQueue.Enqueue(message);
        }

        static public void Register(int type, Action<JsonNode> f)
        {
            EventHandlers.AddOrUpdate(type, new ConcurrentDictionary<Action<JsonNode>, byte> { [f] = 0 },
                                        (_, l) => { l.TryAdd(f, 0); return l; });
        }

        static public void Unregister(int type, Action<JsonNode> f)
        {
            if (EventHandlers.TryGetValue(type, out var handlers))
            {
                handlers.TryRemove(f, out _);
                if (handlers.IsEmpty)
                {
                    EventHandlers.AddOrUpdate(type, [], (__, latesthandlers)
                        => { if (latesthandlers.IsEmpty) EventHandlers.TryRemove(type, out _); return new(); });
                }
            }
        }

        static public void Clear(int type)
        {
            EventHandlers.TryRemove(type, out _);
        }

        static private void Handle(string msgstr)
        {
            if (msgstr[0] == '\0')
                msgstr = msgstr.Remove(0, 1);
            JsonNode jsonnode = JsonNode.Parse(msgstr)!;
            int type = jsonnode!["_type"]!.GetValue<int>();
            if (EventHandlers.TryGetValue(type, out var handlers))
                foreach (var handler in handlers.Keys)
                    handler(jsonnode);
            else //TODO
                return;
        }
    }
}
