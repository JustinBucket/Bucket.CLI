using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLI
{
    public abstract class Component
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Component(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual string GenerateInfo()
        {
            return $"{Name}: {Description}";
        }
    }
}