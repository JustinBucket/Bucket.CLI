using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public class Context
    {
        private Component _rootComponent;
        private readonly string[] _args;
        private Component? _componentToExecute;
        public Context(Component rootComponent, string[] args)
        {
            _rootComponent = rootComponent;
            _args = args;
            _componentToExecute = _rootComponent.FindComponent(args);
        }

        public virtual void Execute()
        {
            if (_componentToExecute == null)
            {
                throw new InvalidOperationException("No component found to execute.");
            }

            _componentToExecute.ValidateArguments(_args);
            _componentToExecute.Execute(_args);
        }
    }
}