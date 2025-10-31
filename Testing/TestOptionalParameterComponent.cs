using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bucket.CLI;

namespace Testing
{
    public class TestOptionalParameterComponent : Component
    {
        public TestOptionalParameterComponent() : base("OptionalParameterTest", "test for optional parameters")
        {
            
        }

        protected override void Execute(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
        }

        protected override void ValidateArguments(string[] args)
        {
            return;
        }
    }
}