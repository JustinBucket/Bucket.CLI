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
        // ├── command1
        // ├── command2
        // ├── financials
        // │   ├── fcommand1
        // │   │   └── reports
        // │   └── fcommand2
        // │       └── sales
        // │           └── scommand1
        // └── banking
        //     ├── bcommand1
        //     └── bcommand2

        public static string GenerateTree(this Component component)
        {
            var lines = component.GenerateTree(0);
            return string.Join(Environment.NewLine, lines);
        }
        
        // what if we had the top level build out the structure?
        private static ICollection<string> GenerateTree(this Component component, int depth = 0)
        {
            // root
            // ├── command1
            // ├── command2

            // start with the root
            var treeLines = new List<string>
            {
                GenerateLeadingCharacters(depth) + component.GenerateInfo()
            };

            foreach (var child in component.Children)
            {
                treeLines.AddRange(child.GenerateTree(depth + 1));
            }

            // once the child lines have been added, go through each line in reverse order
            // , changing TPIPES to LPIPES as needed

            var reversedLines = new List<string>();

            treeLines.Reverse();
            var depthFixed = false;

            foreach (var line in treeLines)
            {
                // want index for next depth down
                var index = CalculateIndex(depth + 1);
                // handles TPIPEs with children info on same line
                if (line.Length >= index)
                {
                    if (
                        line[index] == TPIPE
                        && line[index + 1] == ' '
                        && !depthFixed)
                    {
                        var modifiedLine = line.ToCharArray();
                        modifiedLine[index] = ' ';
                        reversedLines.Add(new string(modifiedLine));
                    }
                    // handles TPIPEs with nothing following
                    else if (line[index] == TPIPE && !depthFixed)
                    {
                        var modifiedLine = line.ToCharArray();
                        modifiedLine[index] = LPIPE;
                        reversedLines.Add(new string(modifiedLine));
                        depthFixed = true;
                    }
                    // handles TPIPEs at the start of child lines, between two children of lower depth
                    else if (
                        line[index] == TPIPE 
                        && line[index + 1] == ' '
                        && depthFixed)
                    {
                        var modifiedLine = line.ToCharArray();
                        modifiedLine[index] = PIPE;
                        reversedLines.Add(new string(modifiedLine));
                    }
                    else
                    {
                        reversedLines.Add(line);
                    }
                }
                
            }

            // reverse again to make it normal
            reversedLines.Reverse();

            return reversedLines;
        }

        private static int CalculateIndex(int depth)
        {
            // depth 1 = 0
            // depth 2 = 4
            // depth 3 = 8
            // depth 4 = 12
            var index = (depth - 1) * 4;
            return index < 0 ? 0 : index;
        }

        private static string GenerateLeadingCharacters(int depth)
        {
            if (depth == 0)
            {
                return string.Empty;
            }

            var numSpaces = CalculateNumberOfSpaces(depth);
            var spaceString = new string(' ', numSpaces);
            spaceString = spaceString + TPIPE + DASH + DASH + ' ';
            
            // depth - 1 because we're genearting space for the previous depth
            for (int i = 0; i < depth - 1; i++)
            {
                spaceString = spaceString.Insert(i * 4, TPIPE.ToString());
            }

            return spaceString;
        }
        
        private static int CalculateNumberOfSpaces(int depth)
        {
            var numSpaces = 3 * (depth - 1);
            return numSpaces > 0 ? numSpaces : 0;
        }

    }
}