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
            string msg;
            (user, msg) = pair;
            //TODO
        }
    }
}
