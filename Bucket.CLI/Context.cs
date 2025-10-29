using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public class Context
    {
        private Component _rootComponent;
        private Component? _componentToExecute;
        public Context(Component rootComponent, string[] args)
        {
            _rootComponent = rootComponent;
            _componentToExecute = _rootComponent.FindComponent(args);
        }

        public virtual void Execute()
        {
            if (_componentToExecute == null)
            {
                throw new InvalidOperationException("No component found to execute.");
            }

            _componentToExecute.Execute();
        }
    }
}