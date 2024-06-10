using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class User
    {
        private SslStream stream;
        private SemaphoreSlim msg_sem = new(1, 1);
        private Queue<string> messages = new();

        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public int num { get; private set; } = -1; // 处于房间中的序号，-1表示未进入任何房间
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
            Read();
            Write();
        }

        public void Send(string message)
        {
            messages.Enqueue(message);
            msg_sem.Release();
        }

        public bool TryJoinRoom(int roomid)
        {
            if (num != -1) return false; // throw
            Room? room = null;
            if (!GlobalRoom.TryGetRoom(roomid, room))
                return false;
            return true;
        }
        public bool TryCreateRoom()
        {
            if (this.Room is not null)
                return false;
            this.Room = GlobalRoom.CreateRoom(this);
            num = 0;
            return true;
        }
        /* 以后再写
        public void LeaveRoom()
        {
            if (num == 0)   // 房主退出房间即解散房间
            {
            }
            num = -1;
            this.Room = null;
        }*/

        private async void Write()
        {
            while (true)
            {
                await msg_sem.WaitAsync();
                while (messages.Count > 0)
                {
                    var bytes = Encoding.UTF8.GetBytes(messages.Dequeue() + '\n');
                    await stream.WriteAsync(bytes);
                }
            }
        }

        private async void Read()
        {
            // Read the  message sent by the client.
            // The client signals the end of the message using the
            // '\n' marker.
            StreamReader sr = new(stream);
            while (true)
            {
                StringBuilder messageData = new StringBuilder();
                // Read the client's test message.
                string? msg = await sr.ReadLineAsync();
                if (msg == null) { continue; } // ???
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
        static public bool TryGetRoom(int roomid, Room? room)
        {
            return rooms.TryGetValue(roomid, out room);
        }
    }

    internal class Room
    {
        public int Id { get; private set; }
        private List<User> users = new(); // 其中users[0]为房主

        public Room(int roomid, User host)
        {
            Id = roomid;
            users.Add(host);
        }
    }

    internal class MyListener
    {
        private X509Certificate2 cert2;

        public MyListener(X509Certificate2 cert2)
        {
            this.cert2 = cert2;
        }

        public async void Listen(int port)
        {
            TcpListener listener = new(IPAddress.IPv6Any, port);
            listener.Start();
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                var sslstream = await ProcessClient(client);
                if (sslstream is not null)
                    GlobalRoom.Join(new User(sslstream));
            }
        }

        private async Task<SslStream?> ProcessClient(TcpClient client)
        {
            // A client has connected. Create the
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(client.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                await sslStream.AuthenticateAsServerAsync(cert2, clientCertificateRequired: true, checkCertificateRevocation: false);

                // Display the properties and settings for the authenticated stream.

            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return null;
            }
            return sslStream;
        }
    }

    static class Process
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
