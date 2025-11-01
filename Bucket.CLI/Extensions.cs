using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bucket.CLI
{
    public static class Extensions
    {
        public const char TPIPE = '├';
        public const char LPIPE = '└';
        public const char DASH = '─';
        public const char PIPE = '│';
        // TODO: review and complete implementation + test
        // should be fairly simple with recursion
        // root
        //  ├── --command1 └ ├ │
        //  ├── --command2
        //  ├── financials
        //  │   ├── fcommand1 
        //  │   └── fcommand2
        //  │       └── sales
        //  │           └── --scommand1
        //  ├── banking
        //  │    ├── --bcommand1
        //  │    └── --bcommand2
        public static string GenerateTree(this Component component, int depth)
        {
            throw new NotImplementedException();
            var infoString = new StringBuilder();

            // first character is ├ pipe - unless it's the last child then it's └
            // fix at the parent level by converting last ├ to └
            // problem is that we don't know if the last line is actually a component's child
            infoString.Append(depth > 0 ? PIPE : TPIPE);

            // next set is indent # - 1 x ─
            // 3 spaces for first level (depth 1)
            // 7 spaces for second level (depth 2)
            // 3 + 1
            // 11 spaces for third level (depth 3)
            // 3 + 3 + 1 + 3 + 1
            // 3 x depth + (depth - 1)

            // 3 space, then 4 spaces for each depth after that
            infoString.Append(new string(' ', CalculateNumberOfSpaces(depth)));

            // then two dashes
            infoString.Append(new string(DASH, 2));

            // the space + component info
            infoString.Append($" {component.GenerateInfo()}");

            // then children below it
            foreach (var child in component.Children)
            {
                infoString.Append(Environment.NewLine + child.GenerateTree(depth + 1));
            }

            // find the last component at one depth down
            // need to be able to parse a line to determine what depth it's at
            // each time we hit a line with the desired depth, we store it as the last found
            // then we replace the TPIPE with an LPIPE and hope the references keeps everything in sync
            // use CalculateNumberOfSpaces to find out how many spaces to look for

            // get the info line for current component
            var infoLines = infoString.ToString().Split(Environment.NewLine);
            var curCompInfoLine = infoLines.FirstOrDefault(x => x.Contains(component.GenerateInfo()));

            // get index of current component info line
            var curCompInfoLineIndex = infoLines.ToList().IndexOf(curCompInfoLine);
            

            return infoString.ToString();
        }
        
        private static int CalculateNumberOfSpaces(int depth)
        {
            return (3 * depth) + (depth - 1);
        }
    }
}