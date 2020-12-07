using System;
using System.IO;
using System.Linq;

using Microsoft.VisualBasic.CompilerServices;

namespace Day1
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
            var ints = lines.Select(x => int.Parse(x)).ToList();

            for (var ai = 0; ai < ints.Count; ai++)
            {
                var a = ints[ai];
                if (a > 2021) continue;
                
                for (var bi = ai; bi < ints.Count; bi++)
                {
                    var b = ints[bi];
                    if (b > 2021) continue;
                    if (a + b == 2020)
                    {
                        Console.WriteLine(a * b);
                    }
                }
            }
            
            //part 2 
            for (var ai = 0; ai < ints.Count; ai++)
            {
                var a = ints[ai];
                if (a > 2021) continue;
                
                for (var bi = ai; bi < ints.Count; bi++)
                {
                    var b = ints[bi];
                    if (b > 2021) continue;

                    for (int ci = bi; ci < ints.Count; ci++)
                    {
                        var c = ints[ci];
                        if (a + b + c == 2020)
                        {
                            Console.WriteLine(a * b * c);
                        }
                    }
                }
            }
        }
    }
}