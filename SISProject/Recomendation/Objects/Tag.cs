﻿using System;

namespace UserBehavior.Objects
{
    [Serializable]
    public class Tag
    {
        public string Name { get; set; }

        public Tag(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
