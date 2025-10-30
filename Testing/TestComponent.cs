using Bucket.CLI;

namespace Testing.TestObjects
{
    public class TestComponent : Component
    {
        public TestComponent(string name, string description, bool ignoreFromTraversal = false) : base(
            name,
            description,
            ignoreFromTraversal   
        )
        { }

        public TestComponent() : base(
            "test",
            "function for testing"
        )
        { }

        protected override void Execute(string[] args)
        {
            Console.WriteLine($"Executed {Name} function");
        }

        protected override void ValidateArguments(string[] args)
        {
            Console.WriteLine($"Validating arguments for {Name} function");
            return;
        }
       
    }
}