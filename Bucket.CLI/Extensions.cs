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
        // root
        // ├── --command1 └ ├ │
        // ├── --command2
        // ├── financials
        // │   ├── fcommand1
        // │   │   └── reports
        // │   └── fcommand2
        // │       └── sales
        // │           └── --scommand1
        // └── banking
        //     ├── --bcommand1
        //     └── --bcommand2
        public static ICollection<string> GenerateTree(this Component component, int depth)
        {
            // TODO: now it's adding a pipe on top of everything else
            // does recursion make this impossible?
            // throw new NotImplementedException();
            var outputLines = new List<string>();
            var infoString = new StringBuilder();

            if (depth == 0)
            {
                infoString.Append(component.GenerateInfo());
            }
            else
            {
                // first character is ├ pipe - unless it's the last child then it's └
                infoString.Append(depth > 0 ? PIPE : TPIPE);

                // next set is indent # - 1 x ─
                // 0 spaces for first level (depth 1)
                // 3 spaces for second level (depth 2)
                // 3(d - 1) + (d - 2)
                // 3(2 - 1) + (2 - 2) = 3 + 0 = 3
                // 7 spaces for third level (depth 3)
                // 3(3 - 1) + (3 - 2) = 6 + 1 = 7
                // 11 spaces for fourth level (depth 4)
                // 3(4 - 1) + (4 - 2) = 9 + 2 = 11
                // 3 x depth + (depth - 1)

                // 3 space, then 4 spaces for each depth after that
                infoString.Append(new string(' ', CalculateNumberOfSpaces(depth)));

                // then another tpipe for non-depth 0?
                if (depth > 0)
                {
                    infoString.Append(TPIPE);
                }

                // then two dashes
                infoString.Append(new string(DASH, 2));

                // the space + component info
                infoString.Append($" {component.GenerateInfo()}");
            }

            // add generated output line to collection
            outputLines.Add(infoString.ToString());

            // then children below it
            for (int i = 0; i < component.Children.Count; i++)
            {
                // since the child line will be the next one after we add, store count to use as index
                var lineCount = outputLines.Count;

                // add output lines from children
                outputLines.AddRange(component.Children[i].GenerateTree(depth + 1));

                // if we're on last child, turn TPIPE to LPIPE
                // FOR THAT CHILD, NOT ITS CHILDREN
                if (i == component.Children.Count - 1)
                {
                    // find last component at this depth and change TPIPE to LPIPE
                    // count is one higher than the index, since we want the next line this should just work
                    var lineToModify = outputLines[lineCount];
                    outputLines[lineCount] = lineToModify.Replace(TPIPE, LPIPE);
                }
            }

            return outputLines;
        }
        
        private static int CalculateNumberOfSpaces(int depth)
        {
            // ugly
            // 3(d - 1) + (d - 2)
            var numSpaces = (3 * (depth - 1)) + (depth - 2);
            return numSpaces > 0 ? numSpaces : 0;
        }
    }
}