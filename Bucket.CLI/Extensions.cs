using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public static class Extensions
    {
        // TODO: review and complete implementation + test
        public static string GenerateTree(this Component component, string indent = "")
        {
            throw new NotImplementedException();

            var result = $"{indent}- {component.Name}\n";

            foreach (var child in component.Children)
            {
                result += child.GenerateTree(indent + "  ");
            }

            return result;
        }
    }
}