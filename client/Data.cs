using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    internal class Room
    {
        public int Id { get; private set; }
        public Dictionary<int, User> Parts { get; private set; } = [];

        public Room(int id, Dictionary<int, User> parts)
        {
            Id = id;
            Parts = parts;
        }
    }
}
