namespace Server
{
    internal class Room
    {
        public int Id { get; private set; }
        private int user_cnt = 0;
        private Dictionary<int, User> users = new(); // 其中users[0]为房主
        private SemaphoreSlim msg_sem = new(1, 1);
        private Queue<string> messages = new();

        public Room(int roomid, User host)
        {
            Id = roomid;
            users.Add(user_cnt++, host);
            deliver();
        }

        // 给房间中所有人发送消息
        public void Deliver(string msg)
        {
            messages.Enqueue(msg);
            msg_sem.Release();
        }

        private async void deliver()
        {
            while (true)
            {
                await msg_sem.WaitAsync();
                while (messages.Count > 0)
                {
                    string msg = messages.Dequeue();
                    foreach (var (_, user) in users)
                        user.Send(msg);
                }
            }
        }

        public bool Join(User user)
        {
            users.Add(user_cnt++, user);
            return true;
        }
    }
}
