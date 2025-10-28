using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CLI;

namespace Bucket.CLI
{
    public class Menu : Component
    {
        public Menu? Parent { get; set; }
        public ObservableCollection<Function> Functions { get; } = [];
        public ObservableCollection<Menu> Menus { get; } = [];

        public Menu(string name, string description) 
            : base(name, description)
        {
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