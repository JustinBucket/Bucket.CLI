# Bucket.CLI

Bucket.CLI is a library that provides a way organize to commands in a CLI application

## Installation

Use the dotnet add command to install Bucket.CLI.

```bash
dotnet add Bucket.CLI
```

## Usage
### Example component
```csharp
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
```
### Example usage
```csharp
class Program
{
    static void Main(string[] args)
    {
        // Create the component that will serve as the root
        // by configuring the ValidateArguments method, this component can be set up to perform an action,
        // display an error message, or return a list of all components in the CLI application
        var root = new TestComponent("root", "this is the root component");
        // this component provides a parent for all photo-based components 
        // displays all child components and requests user input on which one to call
        var photoComponent = new PhotoComponent();
        // this component goes through a photos folder, moving them to specific date-based folders
        var photoFileComponent = new PhotoFileComponent();

        // organize the components as desired
        root.Children.Add(photoComponent);
        photoComponent.Children.Add(photoFileComponent);

        // generate the execution context, passing the root component
        // this will search through the components for the target one to be executed
        // if our root was setup to ignore itself in the command the args would be:
        // photo photofile
        var context = new Context(root, args);
        
        // execute the command
        // context.Execute();
    }

}
```

## License

[MIT](https://choosealicense.com/licenses/mit/)