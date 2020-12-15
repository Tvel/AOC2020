using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        public static readonly string Inputs =
            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Input{Path.DirectorySeparatorChar}";

        static void Main(string[] args)
        {
            Console.WriteLine("----------");
            // Console.WriteLine("Demo:");
            // DoWork(File.ReadAllLines($"{Inputs}demo_input.txt"));
            Console.WriteLine("----------");
            Console.WriteLine("Demo2:");
            DoWork(File.ReadAllLines($"{Inputs}demo_input2.txt"));
            Console.WriteLine("----------");
            Console.WriteLine("Real:");
            DoWork(File.ReadAllLines($"{Inputs}input.txt"));
            Console.WriteLine("----------");
        }

        public static void DoWork(string[] lines)
        {
            //part1
            {
                var memory = new Dictionary<long, long>();
                int[] mask = new int[64];
                foreach (var line in lines)
                {

                    if (line.StartsWith("mask = "))
                    {
                        foreach (var (ch, i) in line[7..].Reverse().Select((x, i) => (x, i)) )
                        {
                            mask[i] = ch switch
                                {
                                    'X' => 2,
                                    '1' => 1,
                                    '0' => 0,
                                    _ => throw new Exception("uh")
                                };
                        }
                    }
                    else
                    {
                        Match match = Regex.Match(line, @"mem\[([0-9]+)\] = ([0-9]+)");
                        var address = long.Parse(match.Groups[1].Value);
                        var value = long.Parse(match.Groups[2].Value);

                        for (int position = 0; position < 64; position++)
                        {
                            if(mask[position] == 1)
                                value |= 1L << position;
                            else if(mask[position] == 0)
                                value &= ~(1L << position);
                        }

                        memory[address] = value;
                    }
                }

                var result = memory.Values.Sum();
                Console.WriteLine(result);
            }
            
            //part2
            {
                var memory = new Dictionary<long, long>();
                int[] mask = new int[64];
                foreach (var line in lines)
                {

                    if (line.StartsWith("mask = "))
                    {
                        foreach (var (ch, i) in line[7..].Reverse().Select((x, i) => (x, i)))
                        {
                            mask[i] = ch switch
                                {
                                    'X' => 2,
                                    '1' => 1,
                                    '0' => 0,
                                    _ => throw new Exception("uh")
                                };
                        }
                    }
                    else
                    {
                        Match match = Regex.Match(line, @"mem\[([0-9]+)\] = ([0-9]+)");
                        var address = long.Parse(match.Groups[1].Value);
                        var value = long.Parse(match.Groups[2].Value);
                        
                        var addrMask = (int[])mask.Clone();
                        for (int position = 0; position < 64; position++)
                        {
                            if (mask[position] == 1)
                                addrMask[position] = 1;
                            else if (mask[position] == 0)
                                addrMask[position] = (int)(address >> position) & 1;
                        }
                        
                        var addr = GenerateAddresses(addrMask);

                        foreach (var a in addr)
                        {
                            memory[a] = value;
                        }
                    }
                }
                
                var result = memory.Values.Sum();
                Console.WriteLine(result);
            }
            
        }

        public static IEnumerable<long> GenerateAddresses(int[] mask)
        {
            var addr = new HashSet<long>();
            GenerateAddresses(mask, 0, addr);
            return addr;
        }

        /**
         * Can be optimised way better...
         */
        public static void GenerateAddresses(int[] mask, int pos, HashSet<long> set) {
            
            for (int i = pos; i < 36; i++)
            {
                if (mask[i] != 2) continue;
                
                var one = (int[])mask.Clone();
                var two = (int[])mask.Clone();
                one[i] = 1;
                two[i] = 0;

                if (!set.Contains(ArrToAddr(one)))
                {
                    set.Add(ArrToAddr(one));
                    GenerateAddresses(one, i + 1, set);
                }

                if (!set.Contains(ArrToAddr(two)))
                {
                    set.Add(ArrToAddr(two));
                    GenerateAddresses(two, i + 1, set);
                }
            }
        }

        public static long ArrToAddr(int[] addr)
        {
            long result = 0L;
            for (int position = 0; position < 36; position++)
            {
                if(addr[position] == 1)
                    result |= 1L << position;
                else if(addr[position] == 0)
                    result &= ~(1L << position);
            }

            return result;
        }
    }
}