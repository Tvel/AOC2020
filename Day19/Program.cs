using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
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
            
            
            Console.WriteLine("Cheat:");
            var ch = new Cheat();
            Console.WriteLine(ch.Solve_2($"{Inputs}input.txt"));
            Console.WriteLine("----------");
        }

        public static void DoWork(string[] lines)
        {
            Dictionary<int, IRule> rules = new();
            List<string> stringsToTest = new();
            {
                bool doRules = true;
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        doRules = false;
                        continue;
                    }

                    if (doRules)
                    {
                        var split1 = line.Split(":");
                        int ruleNumber = int.Parse(split1[0]);
                        string ruleData = split1[1];

                        if (ruleData.Contains("\""))
                        {
                            var ruleChar = ruleData.Replace("\"", "").Trim()[0];
                            rules.Add(ruleNumber, new CharRule(ruleChar));
                        }
                        else if (ruleData.Contains("|"))
                        {
                            var sides = ruleData.Split("|");
                            var leftSide = sides[0].Trim().Split(" ").Select(x => int.Parse(x)).ToList();
                            var left = new MultiRefRule(leftSide.Select(x => new RefRule(x)).ToArray());


                            var rightSide = sides[1].Trim().Split(" ").Select(x => int.Parse(x)).ToList();
                            var right = new MultiRefRule(rightSide.Select(x => new RefRule(x)).ToArray());

                            rules.Add(ruleNumber, new OrRule(left, right));
                        }
                        else
                        {
                            var ruleDatas = ruleData.Trim().Split(" ").Select(x => int.Parse(x)).ToList();
                            if (ruleDatas.Count == 1)
                            {
                                rules.Add(ruleNumber, new RefRule(ruleDatas[0]));
                            }
                            else
                            {
                                rules.Add(
                                    ruleNumber,
                                    new MultiRefRule(ruleDatas.Select(x => new RefRule(x)).ToArray()));
                            }
                        }
                    }
                    else
                    {
                        stringsToTest.Add(line);
                    }
                }
            }

            //part1
            {
             var count = 0;
             foreach (var test in stringsToTest)
             {
                 var rule = new StartRule();
                 var result = rule.Eval(test.ToCharArray().AsSpan(), rules);
                 if (result.match) count++;
                 //Console.WriteLine(result);
             }

             Console.WriteLine(count);
            }

            //part2
            {
                // Cheating: ok something is wrong with checking stuff at the end of the string. Resulting at false positives. Fail.
                
                //8: 42 | 42 8
                //11: 42 31 | 42 11 31

                rules[8] = new OrRule(new RefRule(42), new MultiRefRule(new[] { new RefRule(42), new RefRule(8) }));
                rules[11] = new OrRule(
                     new MultiRefRule(new[] { new RefRule(42), new RefRule(31) }), 
                    new MultiRefRule(new[] { new RefRule(42), new RefRule(11), new RefRule(31) })
                    );
                
                
                // foreach (var (number,rule) in rules.ToArray())
                // {
                //     rules[number] = Simplify(rule, number);
                // }
                
                IRule Simplify(IRule r, int index)
                {
                    if (r is MultiRefRule multiRefRule)
                    {
                        var list = new List<IRule>();
                        foreach (var refRule in multiRefRule.RefRules)
                        {
                            if (refRule.RuleNumber == index)
                            {
                                list.Add(refRule);
                                continue;
                            }
                            var nr = Simplify(refRule, refRule.RuleNumber);
                            list.Add(nr);
                        }
                
                        int count = 0;
                        foreach (var nr in list)
                        {
                            if (nr is CharRule cr)
                            {
                                count++;
                            }
                            if (nr is SolvedRule sr)
                            {
                                count++;
                            }
                        }
                
                        if (count == list.Count)
                        {
                            var chArr = new List<char>();
                            foreach (var nr in list)
                            {
                                if (nr is CharRule cr)
                                {
                                    chArr.Add(cr.Char);
                                }
                                if (nr is SolvedRule sr)
                                {
                                    chArr.AddRange(sr.Chars);
                                }
                            }
                
                            return new SolvedRule(chArr.ToArray());
                        }
                        
                    }
                    else if (r is RefRule refRule)
                    {
                        var nr = Simplify(rules[refRule.RuleNumber], refRule.RuleNumber);
                        if (nr is CharRule cr)
                        {
                            return cr;
                        }
                        if (nr is SolvedRule sr)
                        {
                            return sr;
                        }
                    }
                    else if (r is OrRule orRule)
                    {
                        return new OrRule(Simplify(orRule.Left, index), Simplify(orRule.Right, index));
                    }
                
                    return r;
                }
                
                var count = 0;
                foreach (var test in stringsToTest)
                {
                    var rule = new StartRule();
                    var result = rule.Eval(test.ToCharArray().AsSpan(), rules);
                    if (result.match) count++;
                    // Console.WriteLine($"{result} : {test}");
                }

                Console.WriteLine(count);
            }
            
        }
    }

    interface IRule
    {
        (bool match, int takenChars) Eval(Span<char> text, Dictionary<int, IRule> rules);
        
        
    }

    record SolvedRule(char[] Chars) : IRule
    {
        public (bool, int) Eval(Span<char> text, Dictionary<int, IRule> rules)
        {
            if (Chars.Length >= text.Length)
            {
                return (false, Chars.Length);
            }
    
            var ok = true;
            for (int i = 0; i < Chars.Length; i++)
            {
                if (text[i] != Chars[i])
                {
                    ok = false;
                    break;
                }
            }
            return (ok, Chars.Length);
        }
    }
    
    record CharRule(char Char) : IRule
    {
        public (bool, int) Eval(Span<char> text, Dictionary<int, IRule> rules)
        {
            if (text.Length == 0)
            {
                return (false, 0);
            }
            
            return (text[0] == Char, 1);
        }
    }

    record RefRule(int RuleNumber) : IRule
    {
        public (bool, int) Eval(Span<char> text, Dictionary<int, IRule> rules)
        {
            if (text.Length == 0)
            {
                return (false, 0);
            }
            
            var res = rules[RuleNumber].Eval(text, rules);
            return res;
        }
    }

    record MultiRefRule(RefRule[] RefRules) : IRule
    {
        public (bool match, int takenChars) Eval(Span<char> text, Dictionary<int, IRule> rules)
        {
            if (text.Length == 0)
            {
                // return (false, 0);
            }
            
            int taken = 0;
            for (int i = 0; i < RefRules.Length; i++)
            {
                if (taken > text.Length) return (false, taken);
                var (m, t) = RefRules[i].Eval(text[taken..], rules);
                if (m)
                {
                    taken += t;
                }
                else
                {
                    return (false, taken);
                }
            }
            return (true, taken);
        }
    }

    record OrRule(IRule Left, IRule Right) : IRule
    {
        public (bool match, int takenChars) Eval(Span<char> text, Dictionary<int, IRule> rules)
        {
            if (text.Length == 0)
            {
                return (false, 0);
            }
            
            var lResult = Left.Eval(text, rules);
            if (lResult.match)
            {
                return lResult;
            }
            var right = Right.Eval(text, rules);
            return right;
        }
    }

    class StartRule : IRule
    {
        public (bool match, int takenChars) Eval(Span<char> text, Dictionary<int, IRule> rules)
        {
            var result = rules[0].Eval(text, rules);
            if (result.match == true && result.takenChars == text.Length) return (true, result.takenChars);

            return (false, result.takenChars);
        }
    }
}