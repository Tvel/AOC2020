using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
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
            //1-3 a: abcde
            int validCount = 0;
            foreach (var line in lines)
            {
                Match match = Regex.Match(line, @"([0-9]+)-([0-9]+) ([a-z]): (\w+)");
                if (!match.Success) continue;

                int min = int.Parse(match.Groups[1].Value);
                int max = int.Parse(match.Groups[2].Value);
                char letter = match.Groups[3].Value[0];
                string word = match.Groups[4].Value;

                var count = word.Count(x => x == letter);
                
                if (count >= min && count <= max) validCount++;
            }
            Console.WriteLine(validCount);

            //part2
            validCount = 0;
            foreach (var line in lines)
            {
                Match match = Regex.Match(line, @"([0-9]+)-([0-9]+) ([a-z]): (\w+)");
                if (!match.Success) continue;

                int pos1 = int.Parse(match.Groups[1].Value);
                int pos2 = int.Parse(match.Groups[2].Value);
                char letter = match.Groups[3].Value[0];
                string word = match.Groups[4].Value;

                bool match1 = (word[pos1 - 1] == letter);
                bool match2 = (word[pos2 - 1] == letter);

                if (match1 ^ match2) validCount++;
            }
            Console.WriteLine(validCount);
        }
    }
}