using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
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
            Console.WriteLine("Demo2:");
            DoWork(File.ReadAllLines($"{Inputs}demo_2.txt"));
            Console.WriteLine("----------");
            Console.WriteLine("Real:");
            DoWork(File.ReadAllLines($"{Inputs}input.txt"));
            Console.WriteLine("----------");
        }

        public static void DoWork(string[] lines)
        {

            var rules = new Dictionary<string, List<(int num, string bag)>>();
            foreach (var line in lines)
            {
                var split = line.Split("contain");
                var key = split[0].Replace(" bags", string.Empty).Trim();
                var valuesSplit = split[1]
                    .Split(",")
                    .Select(x => x
                        .Replace(" bags", string.Empty)
                        .Replace(" bag", "")
                        .Replace(".", "")
                        .Trim())
                    .ToArray();

                rules[key] = new List<(int num, string bag)>();
                
                if(valuesSplit.Length == 1 && valuesSplit[0] == "no other") continue;

                foreach (var value in valuesSplit)
                {
                    var first = value.IndexOf(" ");
                    var num = int.Parse(value[..first].Trim());
                    var valuekey = value[first..].Trim();
                    
                    rules[key].Add((num, valuekey));
                }
            }
            
            //1 find gold bags in bags
            {
                int count = 0;
                foreach (var (rule, contains) in rules)
                {
                    var res = CanContainGold(rule);
                    if (res) count++;
                }

                Console.WriteLine(count);


                bool CanContainGold(string bag)
                {
                    var contains = rules[bag];
                    foreach (var (num, inbag) in contains)
                    {
                        if (inbag == "shiny gold") return true;
                        if (CanContainGold(inbag) == true) return true;
                    }

                    return false;
                }
            }


            // part 2 - how many bags inside shiny gold
            int bags = 0;
            foreach (var data in rules["shiny gold"])
            {
                bags += CountBags(data);
            }
            Console.WriteLine(bags);


            int CountBags((int num, string bag) bagData)
            {
                int count = bagData.num;
                foreach (var insideBag in rules[bagData.bag])
                {
                    count += CountBags(insideBag) * bagData.num;
                }

                return count;
            }
            
        }
        
        
    }
}