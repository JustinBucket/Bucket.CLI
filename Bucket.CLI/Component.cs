using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public abstract class Component
    {
        private readonly bool ignoreFromTraversal;
        public string Name { get; set; }
        public string Description { get; set; }
        public Component? Parent { get; set; }
        public ObservableCollection<Component> Children { get; } = new ObservableCollection<Component>();
        public Component(string name, string description, bool ignoreFromTraversal = false)
        {
            Name = name;
            Description = description;
            this.ignoreFromTraversal = ignoreFromTraversal;
            Children.CollectionChanged += HandleCollectionChanged;
        }

        public virtual string GenerateInfo()
        {
            return $"{Name}: {Description}";
        }

        protected internal abstract void Execute(string[] args);
        protected internal abstract void ValidateArguments(string[] args);

        private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (newItem != null)
                    {
                        ((Component)newItem).Parent = this;
                    }
                }
            }
        }

        public void HandleCommand(string[] args)
        {
            var component = FindComponent(args);

            if (component == null)
            {
                throw new InvalidOperationException("Component not found.");
            }

            component.ValidateArguments(args);
            component.Execute(args);
        }

        internal Component? FindComponent(string[] args)
        {
            // remove all optional parameters found, re-attach them when passing them down
            var argsWithoutOptions = args.Where(arg => !arg.StartsWith("--")).ToArray();
            
            // get first arg, check if it matches current component
            // what if we're in the root and want to ignore?
            var firstArg = argsWithoutOptions[0];
            if (
                !firstArg.Equals(Name, StringComparison.InvariantCultureIgnoreCase)
                && !ignoreFromTraversal
            )
            {
                return null;
            }

            // if only one arg, return current component
            if (argsWithoutOptions.Length == 1 && !ignoreFromTraversal)
            {
                return this;
            }

            // else search through children for next arg
            // if we're ignoring the current component, use current arg
            var secondArg = ignoreFromTraversal ? firstArg : argsWithoutOptions[1];
            var child = Children.FirstOrDefault(c => c.Name.Equals(secondArg, StringComparison.InvariantCultureIgnoreCase));

            if (child != null)
            {
                // recurse into child with remaining args
                var remainingArgs = ignoreFromTraversal? args : args.Skip(1).ToArray();
                return child.FindComponent(remainingArgs);
            }
            else
            {
                return child;
            }
        }
    }
}