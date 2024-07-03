using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    using DB = Database;

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

    internal class Room
    {
        public int Id { get; private set; }
        public int Num { get; private set; }
        public Dictionary<int, User> Parts { get; private set; } = [];

        public Room(int id)
        {
            Id = id;
            Num = 0;
            Parts.Add(0, DB.Me);
        }
        public Room(int id, int mynum, Dictionary<int, Info.UserBriefInfo> parts)
        {
            Id = id;
            Num = mynum;
            foreach (var (num, info) in parts)
            {
                Parts.Add(num, new User(info.id, info.name));
            }
        }
    }

    internal static class Database
    {
        public static User Me { get; set; } = new();
        public static Room? Room { get; set; }
    }
}
