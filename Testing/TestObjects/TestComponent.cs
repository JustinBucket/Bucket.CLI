using Bucket.CLI;

namespace Testing.TestObjects
{
    public class TestComponent : Component
    {
        public TestComponent(string name, string description) : base(
            name,
            description
        )
        { }

        public TestComponent() : base(
            "test",
            "function for testing"
        )
        { }

        public override void Execute(params string[] args)
        {
            Console.WriteLine($"Executed {Name} function");
        }

        public override void ValidateArguments(params string[] args)
        {
            Console.WriteLine($"Validating arguments for {Name} function");
            return;
        }
    }
}