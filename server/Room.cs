using System.Text.Json.Nodes;

using Microsoft.VisualStudio.Threading;

namespace Server
{
    internal class Room
    {
        public int Id { get; private set; }
        public Dictionary<int, User> Parts { get; private set; } // id -> user
        private AsyncQueue<string> messages = new();

        public Room(int roomid, User host)
        {
            Id = roomid;
            Parts = new() { {host.Id, host} };
            _ = Task.Run(DeliverAsync);
        }

        // 给房间中所有人发送消息
        public void Deliver(string msg)
        {
            Console.WriteLine($"Room[{Id}] Push message: `{msg}`");
            messages.Enqueue(msg);
        }

        private async Task DeliverAsync()
        {
            while (true)
            {
                string msg = await messages.DequeueAsync();
                foreach (var user in Parts.Values)
                    user.Send(msg);
            }
        }

        public void Join(User user)
        {
            Parts.Add(user.Id, user);
        }

        public void Leave(User user)
        {
            Parts.Remove(user.Id);
        }

        public JsonArray GetPartsInfo()
        {
            JsonArray arr = [];
            foreach (var part in Parts.Values)
                arr.Add(part.GetBreifInfo());
            return arr;
        }

        public JsonObject GetInfo()
        {
            return new JsonObject() {
                {"id", Id},
                {"parts", GetPartsInfo()}
            };
        }
    }
}
