using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLI;

namespace Bucket.CLI
{
    public abstract class Function : Component
    {
        public Menu Parent { get; internal set; }

        public Function(string name, string description)
            : base(name, description) { }

        // called by parent when the function is identified
        public abstract void Execute(params string[] args);
        // provides a way to check/request arguments
        public abstract void ValidateArguments(params string[] args);
    }
}