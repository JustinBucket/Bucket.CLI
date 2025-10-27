using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bucket.CLI;

namespace Testing.TestObjects
{
    public class TestFunction : Function
    {
        public TestFunction() : base(
            "test",
            "function for testing"
        )
        { }

        public override void Execute(params string[] args)
        {
            Console.WriteLine("Executed test function");
        }

        public override void ValidateArguments(params string[] args)
        {
            return;
        }
    }
}