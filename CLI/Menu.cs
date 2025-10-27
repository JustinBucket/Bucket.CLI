using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public class Menu
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Menu? Parent { get; set; }
        public ObservableCollection<Function> Functions { get; } = [];
        public ObservableCollection<Menu> Menus { get; } = [];
        
        public Menu(string name, string description)
        {
            Name = name;
            Description = description;
            Functions.CollectionChanged += HandleCollectionChanged;
            Menus.CollectionChanged += HandleCollectionChanged;
        }

        private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (newItem != null)
                    {
                        var newItemType = newItem.GetType();
                        if (typeof(Function).IsAssignableFrom(newItemType))
                        {
                            ((Function)newItem).Parent = this;
                        }
                        else if (newItemType == typeof(Menu) || newItemType.IsAssignableFrom(typeof(Menu)))
                        {
                            ((Menu)newItem).Parent = this;
                        }
                    }
                }
            }
        }
    }
}