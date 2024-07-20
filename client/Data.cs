using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Info;

namespace Client
{
    using DB = Database;

    //用户信息
    internal class User
    {
        public int Id { get; private set; } = 0;
        public string Name { get; private set; } = "No name.";

        public User() { }
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public User(UserBriefInfo info)
        {
            Id = info.id;
            Name = info.name;
        }

        public void SetId(int id) => Id = id;
        public void SetName(string name) => Name = name;
    }

    //房间信息
    internal class Room
    {
        public int Id { get; private set; }
        public int Num { get; private set; }
        // 成员列表: num -> user
        public Dictionary<int, User> Parts { get; private set; } = []; // int索引为用户在房间的num，1号为房主

        // 作为房主创建房间时构造
        public Room(int id)
        {
            Id = id;
            Num = 0;
            Parts.Add(1, DB.Me);
        }
        // 加入房间时构造
        public Room(int id, int mynum, List<Info.Entry> parts)
        {
            Id = id;
            Num = mynum;
            foreach (var (num, info) in parts)
            {
                Parts.Add(num, new User(info));
            }
        }

        public void Join(int num, User user)
        {
            Parts.Add(num, user);
        }

        public void Leave(int num)
        {
            Parts.Remove(num);
        }
    }

    //数据库，包含用户信息和房间信息
    internal static class Database
    {
        public static User? Me { get; set; }
        public static Room? Room { get; set; }
    }
}
