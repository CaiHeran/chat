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
        // 成员列表: id -> user
        public Dictionary<int, User> Parts { get; private set; } = [];

        // 作为房主创建房间时构造
        public Room(int roomid)
        {
            Id = roomid;
            Parts.Add(DB.Me.Id, DB.Me);
        }
        // 加入房间时构造
        public Room(int id, List<UserBriefInfo> parts)
        {
            Id = id;
            foreach (var info in parts)
            {
                Parts.Add(info.id, new User(info));
            }
        }

        public void Join(User user)
        {
            Parts.Add(user.Id, user);
        }

        public void Leave(int id)
        {
            Parts.Remove(id);
        }
    }

    //数据库，包含用户信息和房间信息
    internal static class Database
    {
        public static User? Me { get; set; }
        public static Room? Room { get; set; }
    }
}
