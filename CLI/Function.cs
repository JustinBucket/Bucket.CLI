using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public abstract class Function
    {
        public Menu Parent { get; internal set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Function(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract void Execute(params string[] args);
        public abstract void ValidateArguments(params string[] args);
    }
}