using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        public static string Inputs = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Input{Path.DirectorySeparatorChar}";

        static void Main(string[] args)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Demo:");
            DoWork(File.ReadAllLines($"{Inputs}demo_input.txt"));
            Console.WriteLine("----------");
            Console.WriteLine("Real:");
            DoWork(File.ReadAllLines($"{Inputs}input.txt"));
            Console.WriteLine("----------");
        }

        public static void DoWork(string[] lines)
        {
            var groupLines = SplitToGroupLines(lines);
            
            var groupQuestions = CountYesQuestions(groupLines);
            
            Console.WriteLine( groupQuestions.Select(x => x.questions.Keys.Count).Sum());


            Console.WriteLine(groupQuestions.Select(x => x.questions.Values.Count(v => v == x.num)).Sum());

        }

        private static List<(int num, Dictionary<char, int> questions)> CountYesQuestions(List<List<string>> groupLines)
        {
            var groups = new List<(int, Dictionary<char, int>)>();
            
            foreach (var groupLine in groupLines)
            {
                Dictionary<char, int> questions = new Dictionary<char, int>();

                foreach (var line in groupLine)
                {
                    foreach (var ch in line)
                    {
                        if (!questions.ContainsKey(ch)) questions[ch] = 1;
                        else questions[ch] = questions[ch] + 1;
                    }
                }
                
                groups.Add((groupLine.Count, questions));
            }

            return groups;
        }
        
        private static List<List<string>> SplitToGroupLines(string[] lines)
        {
            List<List<string>> groupLines = new List<List<string>>();
            int currentGrp = 0;
            groupLines.Add(null);
            groupLines[currentGrp] = new List<string>();
            foreach (var line in lines)
            {
                if (groupLines[currentGrp] is null) groupLines[currentGrp] = new List<string>();

                if (line == "")
                {
                    currentGrp++;
                    groupLines.Add(null);
                    continue;
                }

                groupLines[currentGrp].Add(line);
            }

            return groupLines;
        }
    }
}