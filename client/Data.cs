using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void SetId(int id) => Id = id;
        public void SetName(string name) => Name = name;
    }

    //房间信息
    internal class Room
    {
        public int Id { get; private set; }
        public int Num { get; private set; }
        // 成员映射
        public Dictionary<int, User> Parts { get; private set; } = [];

        // 创建房间时构造
        public Room(int id)
        {
            Id = id;
            Num = 0;
            Parts.Add(0, DB.Me);
        }
        // 加入房间时构造
        public Room(int id, int mynum, Dictionary<int, Info.UserBriefInfo> parts)
        {
            Id = id;
            Num = mynum;
            foreach (var (num, info) in parts)
            {
                Parts.Add(num, new User(info.id, info.name));
            }
        }
        //public Join(int id, int num)
    }

    //数据库，包含用户信息和房间信息
    internal static class Database
    {
        public static User? Me { get; set; }
        public static Room? Room { get; set; }
    }
}
