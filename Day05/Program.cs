using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Day05
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
            {
                long maxid = 0;
                foreach (var line in lines)
                {
                    //FBFBBFF RLR
                    var test1 = line[0..7];
                    var test2 = line[7..];
                    var row = Convert.ToInt64(line[0..7].Replace('F', '0').Replace('B', '1'), 2);
                    var column = Convert.ToInt64(line[7..].Replace('L', '0').Replace('R', '1'), 2);

                    var id = (row * 8) + column;
                    if (id > maxid) maxid = id;
                }

                Console.WriteLine(maxid);
            }

            //part2
            {
                List<int> ids = new List<int>();
                foreach (var line in lines)
                {
                    //FBFBBFF RLR
                    var test1 = line[0..7];
                    var test2 = line[7..];
                    var row = Convert.ToInt32(line[0..7].Replace('F', '0').Replace('B', '1'), 2);
                    var column = Convert.ToInt32(line[7..].Replace('L', '0').Replace('R', '1'), 2);

                    var id = (row * 8) + column;
                    ids.Add(id);
                }
                
                ids.Sort();

                int prev = ids[0];
                for (int i = 1; i < ids.Count; i++)
                {
                    if (ids[i] == prev + 1)
                    {
                        prev = ids[i];
                    }
                    else
                    {
                        Console.WriteLine(prev + 1);
                        break;
                    }
                }
                
            }
            
            
        }
    }
}