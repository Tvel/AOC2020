using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        public static string Inputs = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Input{Path.DirectorySeparatorChar}";

        static void Main(string[] args)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Demo:");
            DoWork(File.ReadAllLines($"{Inputs}demo_input.txt"), 5);
            Console.WriteLine("----------");
            Console.WriteLine("Real:");
            DoWork(File.ReadAllLines($"{Inputs}input.txt"), 25);
            Console.WriteLine("----------");
        }

        public static void DoWork(string[] lines, int preambleCount)
        {

            long[] numbers = new long[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                numbers[i] = long.Parse(lines[i]);
            }

            var number = GetNumber(numbers, preambleCount);
            Console.WriteLine(number);
            
            //part2

            var weakness = GetEncryptionWeakness(numbers, number);
            Console.WriteLine(weakness);
        }

        private static long GetNumber(long[] numbers, int preambleCount)
        {
            for (int i = preambleCount; i < numbers.Length; i++)
            {
                long number = numbers[i];

                bool match = false;
                for (int j = i - preambleCount; j < i; j++)
                {
                    for (int k = j; k < i; k++)
                    {
                        if (numbers[j] + numbers[k] == number)
                        {
                            match = true;
                            break;
                        }
                    }

                    if (match) break;
                }

                if (match == false)
                    return number;
            }

            return 0;
        }

        private static long GetEncryptionWeakness(long[] numbers, long number)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                bool over = false;
                bool match = false;
                long sum = 0;
                List<long> seq = new List<long>();
                
                for (int j = i; j < numbers.Length; j++)
                {
                    sum += numbers[j];
                    seq.Add(numbers[j]);
                    if (sum == number) {match = true; break;}
                    if (sum > number) {over = true; break;}
                }
                
                if(over) continue;

                if (match)
                {
                    if(seq.Count < 2) continue;

                    return seq.Min() + seq.Max();
                }
            }

            return 0;
        }
    }
}