﻿using System;

namespace UserBehavior.Objects
{
    [Serializable]
    public class User
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public User(int id, string name)
        {
            UserID = id;
            Name = name;
        }

        public override string ToString()
        {
            return UserID + "," + Name;
        }
    }
}
