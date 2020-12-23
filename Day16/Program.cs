using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
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
                Dictionary<string, HashSet<int>> rules = new();
                HashSet<int> yourTicket = new();
                List<HashSet<int>> nearbyTickets = new();
                int flag = 0;
                foreach (var line in lines)
                {
                    if(string.IsNullOrWhiteSpace(line)) continue;
                    
                    if (line == "your ticket:")
                    {
                        flag = 1; continue;
                    }

                    if (line == "nearby tickets:")
                    {
                        flag = 2; continue;
                    }
                    
                    switch (flag)
                    {
                        case 0:
                            {
                                var match = Regex.Match(line, @"([A-Za-z\s]+): ([0-9]+)-([0-9]+) or ([0-9]+)-([0-9]+)");
                                var set = new HashSet<int>();
                                for (int i = int.Parse(match.Groups[2].Value); i <= int.Parse(match.Groups[3].Value); i++)
                                {
                                    set.Add(i);
                                }
                                for (int i = int.Parse(match.Groups[4].Value); i <= int.Parse(match.Groups[5].Value); i++)
                                {
                                    set.Add(i);
                                }

                                rules[match.Groups[1].Value] = set;

                                break;
                            }
                        case 1:
                            {
                                yourTicket = line.Split(",").Select(x => int.Parse(x)).ToHashSet();
                                break;
                            }
                        case 2:
                            {
                                nearbyTickets.Add(line.Split(",").Select(x => int.Parse(x)).ToHashSet());
                                break;
                            }
                    }
                }


                var combinedRules = new HashSet<int>();
                foreach (var set in rules.Values)
                {
                    combinedRules.UnionWith(set);
                }
                
                var sum = 0;
                foreach (var ticket in nearbyTickets)
                {
                    foreach (var value in ticket)
                    {
                        if (!combinedRules.Contains(value)) sum += value;
                    }
                    
                }
                
                Console.WriteLine(sum);
            }
            
            //part2
            {
                Dictionary<string, HashSet<int>> rules = new();
                List<int> yourTicket = new();
                List<List<int>> nearbyTickets = new();
                {
                    int flag = 0;
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        if (line == "your ticket:")
                        {
                            flag = 1;
                            continue;
                        }

                        if (line == "nearby tickets:")
                        {
                            flag = 2;
                            continue;
                        }

                        switch (flag)
                        {
                            case 0:
                                {
                                    var match = Regex.Match(line, @"([A-Za-z\s]+): ([0-9]+)-([0-9]+) or ([0-9]+)-([0-9]+)");
                                    var set = new HashSet<int>();
                                    for (int i = int.Parse(match.Groups[2].Value);
                                         i <= int.Parse(match.Groups[3].Value);
                                         i++)
                                    {
                                        set.Add(i);
                                    }

                                    for (int i = int.Parse(match.Groups[4].Value);
                                         i <= int.Parse(match.Groups[5].Value);
                                         i++)
                                    {
                                        set.Add(i);
                                    }

                                    rules[match.Groups[1].Value] = set;

                                    break;
                                }
                            case 1:
                                {
                                    yourTicket = line.Split(",").Select(x => int.Parse(x)).ToList();
                                    break;
                                }
                            case 2:
                                {
                                    nearbyTickets.Add(line.Split(",").Select(x => int.Parse(x)).ToList());
                                    break;
                                }
                        }
                    }
                }
                
                List<List<int>> validTickets = new();
                {
                    var combinedRules = new HashSet<int>();
                    foreach (var set in rules.Values)
                    {
                        combinedRules.UnionWith(set);
                    }

                    var sum = 0;
                    foreach (var ticket in nearbyTickets)
                    {
                        bool ok = true;
                        foreach (var value in ticket)
                        {
                            if (!combinedRules.Contains(value))
                            {
                                ok = false;
                                break;
                            }
                        }

                        if (ok) validTickets.Add(ticket);
                    }
                }

                Dictionary<string, HashSet<int>> ruleIndexes = new();
                {
                    foreach (var (ruleName, rule) in rules)
                    {
                        ruleIndexes[ruleName] = new();
                        bool match = true;
                        int matchingIndex = -1;
                        for (int i = 0; i < yourTicket.Count; i++)
                        {
                            match = true;
                            matchingIndex = i;
                            if (!rule.Contains(yourTicket[i]))
                            {
                                match = false;
                                continue;
                            }

                            foreach (var nearbyTicket in validTickets)
                            {
                                if (!rule.Contains(nearbyTicket[i]))
                                {
                                    match = false;
                                    break;
                                }
                            }

                            if (match)
                            {
                                ruleIndexes[ruleName].Add(matchingIndex);
                            }
                        }
                    }
                }

                Dictionary<string, int> finalRuleIndexes = new();
                while (true)
                {
                    var arr = ruleIndexes.Where(x => x.Value.Count == 1).ToArray();
                    if(arr.Length == 0 ) break;
                    foreach (var (name, set) in arr)
                    {
                        var index = set.First();
                        finalRuleIndexes.Add(name, index);
                        foreach (var (_, rmset) in ruleIndexes)
                        {
                            rmset.Remove(index);
                        }
                    }
                }
                    

                var result = finalRuleIndexes
                    .Where(x => x.Key.Contains("departure"))
                    .Select(x => x.Value)
                    .Aggregate(1L, (acc, source) => acc * yourTicket[source]);
                
                
                Console.WriteLine(result);
            }
        }
    }
}