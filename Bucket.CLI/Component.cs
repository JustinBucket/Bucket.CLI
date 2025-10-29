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
        public string Name { get; set; }
        public string Description { get; set; }
        public Component? Parent { get; set; }
        public ObservableCollection<Component> Children { get; } = [];
        public Component(string name, string description)
        {
            Name = name;
            Description = description;
            Children.CollectionChanged += HandleCollectionChanged;
        }

        public virtual string GenerateInfo()
        {
            return $"{Name}: {Description}";
        }

        public abstract void Execute(params string[] args);
        public abstract void ValidateArguments(params string[] args);

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

        internal Component? FindComponent(string[] args)
        {
            // get first arg, check if it matches current component
            var firstArg = args[0];
            if (!firstArg.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            // if only one arg, return current component
            if (args.Length == 1)
            {
                return this;
            }

            // else search through children for next arg
            var secondArg = args[1];
            var child = Children.FirstOrDefault(c => c.Name.Equals(secondArg, StringComparison.InvariantCultureIgnoreCase));

            if (child != null)
            {
                // recurse into child with remaining args
                var remainingArgs = args.Skip(1).ToArray();
                return child.FindComponent(remainingArgs);
            }
            else
            {
                return child;
            }
        }
    }
}