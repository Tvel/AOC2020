using System;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        public static readonly string Inputs =
            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Input{Path.DirectorySeparatorChar}";

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
            //part1
            {
                int timestamp = int.Parse(lines[0]);
                string[] buses = lines[1].Split(",");

                int?[] closestest = buses.Select(x =>
                    {
                        if (x == "x") return (int?)null;
                        else
                        {
                            var val = int.Parse(x);
                            return (( (timestamp / val) + 1) * val) - timestamp;
                        }
                    }).ToArray();

                var min = closestest.Where(x => x is not null).Select(x => x.Value).Min();
                var index = Array.IndexOf(closestest, min);
                
                Console.WriteLine(min * int.Parse(buses[index]));
            }
            
            //part2
            {
                (long b, int i)[] buses = lines[1].Split(",")
                    .Select(((s, i) => (s, i)))
                    .Where( t => t.s != "x")
                    .Select(t => (long.Parse(t.s), t.i))
                    .ToArray();


                var mods = buses.Select(t => t.b).ToArray();
                var res = buses.Select(t => t.b - t.i).ToArray();
                
                var result = ChineseRemainderTheorem.Solve(mods,res);
                Console.WriteLine(result);
            }
        }
    }
    
    /**
     * thank you https://rosettacode.org/wiki/Chinese_remainder_theorem
     */
    public static class ChineseRemainderTheorem
    {
        public static long Solve(long[] n, long[] a)
        {
            long prod = n.Aggregate(1L, (i, j) => i * j);
            long p;
            long sm = 0L;
            for (long i = 0; i < n.Length; i++)
            {
                p = prod / n[i];
                sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
            }
            return sm % prod;
        }
 
        private static long ModularMultiplicativeInverse(long a, long mod)
        {
            long b = a % mod;
            for (long x = 1; x < mod; x++)
            {
                if ((b * x) % mod == 1L)
                {
                    return x;
                }
            }
            return 1L;
        }
    }
}