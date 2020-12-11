using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        public static string Inputs =
            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Input{Path.DirectorySeparatorChar}";

        static void Main(string[] args)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Demo:");
            DoWork(File.ReadAllLines($"{Inputs}demo_input.txt"));
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
            var nums = ParseAndSort(lines);

            var allNums = nums.ToList();
            allNums.Add(allNums.Last() + 3);
            allNums.Insert(0, 0);

            //part1
            {
                var diffs = new int[allNums.Count];
                for (int i = 1; i < allNums.Count; i++)
                {
                    diffs[i] = allNums[i] - allNums[i - 1];
                }

                var ones = diffs.Count(x => x == 1);
                var threes = diffs.Count(x => x == 3);

                Console.WriteLine(ones * threes);
            }

            //part2
            {
                var count = CountPossibilities(allNums, 0, new Dictionary<int, long>());

                Console.WriteLine(count);
            }
        }

        public static long CountPossibilities(List<int> nums, int i, Dictionary<int, long> cache)
        {
            if (cache.ContainsKey(i))
            {
                return cache[i];
            }

            if (i == nums.Count - 1)
            {
                return 1;
            }

            long count = 0;
            for (int j = i + 1; j <= (i + 3); j++)
            {
                if (j >= nums.Count) continue;

                if (nums[j] - nums[i] <= 3)
                {
                    count += CountPossibilities(nums, j, cache);
                }
            }

            cache.Add(i, count);

            return count;
        }

        public static int[] ParseAndSort(string[] lines)
        {
            var nums = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                nums[i] = int.Parse(lines[i]);
            }

            Array.Sort(nums);
            return nums;
        }
    }
}