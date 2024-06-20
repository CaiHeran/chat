using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal static class Process
    {
        static private SemaphoreSlim sem = new(1, 1);
        static private Queue<string> ProcessQueue = new();

        static public void Start()  // 创建一个线程处理信息
        {
            Thread t = new(new ThreadStart(Processing));
            t.Start();
        }

        static public void ProcessMessage(string message)
        {
            ProcessQueue.Enqueue(message);
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

        static private void process(string msgstr)
        {
        }
    }
}
