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
        // ├── command1 └ ├ │
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

        // what if we had the top level build out the structure?
        public static ICollection<string> GenerateTree(this Component component, int depth = 0)
        {
            // root
            // ├── command1 └ ├ │
            // ├── command2

            // if we generate a dictionary of string, depth, does the top level have enough info to know which pipes to use?

            // start with the root
            var treeLines = new List<string>();
            treeLines.Add(GenerateLeadingCharacters(depth) + component.GenerateInfo());

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
        public static ICollection<string> GenerateTree(this Component component, int depth, bool lastChild = false)
        {
            // TODO: now it's adding a pipe on top of everything else
            // TODO: not putting enough space after children after depth 1
            // for each depth beyond the first, add a pipe
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
                // space if lastchild
                if (lastChild)
                {
                    infoString.Append(' ');
                }
                else
                {
                    infoString.Append(depth > 0 ? PIPE : TPIPE);
                }

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
                infoString.Append(GenerateLeadingCharacters(depth));

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

                // check if child is last
                var isLastChild = i == component.Children.Count - 1;

                // add output lines from children
                outputLines.AddRange(component.Children[i].GenerateTree(depth + 1, isLastChild));

                // if we're on last child, turn TPIPE to LPIPE
                // FOR THAT CHILD, NOT ITS CHILDREN
                if (isLastChild)
                {
                    // find last component at this depth and change TPIPE to LPIPE
                    // count is one higher than the index, since we want the next line this should just work
                    var lineToModify = outputLines[lineCount];
                    outputLines[lineCount] = lineToModify.Replace(TPIPE, LPIPE);
                }
            }

            return outputLines;
        }

        private static string GenerateLeadingCharacters(int depth)
        {
            // if we're at depth 1, no pipe
            // if we're at depth 2, pipe, but all dependent on whether we're on the the last child

            if (depth == 0)
            {
                return string.Empty;
            }

            var numSpaces = CalculateNumberOfSpaces(depth);
            var spaceString = new string(' ', numSpaces);
            spaceString = spaceString + TPIPE + DASH + DASH + ' ';
            if (depth > 1)
            {
                spaceString = spaceString.Insert(0, TPIPE.ToString());
            }

            // TODO: intersperse pipes for each depth level beyond 2
            
            return spaceString;
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