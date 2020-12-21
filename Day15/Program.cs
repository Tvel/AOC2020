using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------");
            
            Console.WriteLine("Demo: 0,3,6");
            DoWork("0,3,6");
            Console.WriteLine("----------");

            Console.WriteLine("Real: 1,17,0,10,18,11,6");
            DoWork("1,17,0,10,18,11,6");
            Console.WriteLine("----------");

        }

        public static void DoWork(string line)
        {
            //part1
            {
                var input = line.Split(",").Select(x => int.Parse(x)).ToArray();
                
                var cache = new Dictionary<int, int>();
                
                for (int i = 1; i <= input.Length; i++)
                {
                    cache[input[i - 1]] = i;
                }

                var a = Enumerable.Range(input.Length, 2020 - input.Length).Aggregate(
                    input.Last(),
                    (last, turn) =>
                        {
                            return cache.Insert(last, turn) switch
                                {
                                    null => 0,
                                    int a => turn - a,
                                };
                        });
                

                Console.WriteLine(a);
            }
            
            //part2 
            {
                var input = line.Split(",").Select(x => int.Parse(x)).ToArray();
                
                var cache = new Dictionary<int, int>();
                
                for (int i = 1; i <= input.Length; i++)
                {
                    cache[input[i - 1]] = i;
                }

                var a = Enumerable.Range(input.Length, 30000000 - input.Length).Aggregate(
                    input.Last(),
                    (last, turn) =>
                        {
                            return cache.Insert(last, turn) switch
                                {
                                    null => 0,
                                    int a => turn - a,
                                };
                        });
                

                Console.WriteLine(a);
            }
        }
    }

    public static class Extensions
    {
        /**
         * Like rust hashmap insert. Easier to fold/aggregate over
         */
        public static int? Insert(this Dictionary<int, int> dict, int key, int value)
        {
            int? oldVal = null;
            if (dict.ContainsKey(key))
            {
                oldVal = dict[key];
            }

            dict[key] = value;

            return oldVal;
        }
    }
}