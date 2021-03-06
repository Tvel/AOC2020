using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Day19
{
    public class Cheat
    {
        public string Solve_2(string InputFilePath)
        {
            (List<Rule> rules, List<string> messages) = ParseInput(InputFilePath);

            var rule8 = rules.First(r => r.Id == 8);
            rule8.Content = "42 | 42 8";

            var rule11 = rules.First(r => r.Id == 11);
            rule11.Content = "42 31 | 42 11 31";

            var regexes = GenerateCombinations(rule8, rule11, rules);

            return CountMatches_ParallelForEach_Interlock(messages, regexes)
                .ToString();
        }

        private static Dictionary<int, Rule> ReplaceNestedRules(List<Rule> originalRules)
        {
            originalRules = originalRules
                .OrderBy(r => !r.IsLiteral)
                .ThenBy(r => r.Content.Length).ToList();

            var replacedRules = new Dictionary<int, Rule>();

            var index = 0;
            while (originalRules.Count > 0)
            {
                var current = originalRules[index];

                if (current.IsLiteral)
                {
                    current.RegexExpression = new StringBuilder(current.Content);
                    replacedRules.Add(current.Id, current);
                    originalRules.Remove(current);
                    continue;
                }

                if (!current.RelatedRules.All(replacedRules.ContainsKey))
                {
                    index = ++index % originalRules.Count;
                    continue;
                }

                foreach (var item in current.Content.Split('|', StringSplitOptions.TrimEntries))
                {
                    var rules = item.Split(' ');

                    foreach (var rule in rules.Select(int.Parse))
                    {
                        var alreadyReplacedRule = replacedRules[rule];

                        if (alreadyReplacedRule.IsLiteral)
                        {
                            current.RegexExpression.Append(alreadyReplacedRule.RegexExpression);
                        }
                        else
                        {
                            current.RegexExpression.Append("(?:");
                            current.RegexExpression.Append(alreadyReplacedRule.RegexExpression);
                            current.RegexExpression.Append(')');
                        }
                    }

                    current.RegexExpression.Append('|');
                }

                current.RegexExpression.Remove(current.RegexExpression.Length - 1, 1);
                replacedRules.Add(current.Id, current);
                originalRules.Remove(current);

                if (originalRules.Count == 0)
                {
                    break;
                }

                index = ++index % originalRules.Count;
            }

            return replacedRules;
        }

        internal static List<Regex> GenerateCombinations(Rule rule8, Rule rule11, List<Rule> rules, RegexOptions? options = null)
        {
            var rule0 = rules.First(r => r.Id == 0);

            var replacedRules = ReplaceNestedRules(rules.Except(new[] { rule0, rule8, rule11 }).ToList());
            var rule42 = replacedRules[42];
            var rule31 = replacedRules[31];

            var patterns = new HashSet<string>();

            for (int i42 = 2; i42 <= 6; ++i42)
            {
                for (int i31 = 1; i31 < i42; ++i31)
                {
                    var sb = new StringBuilder("^");

                    for (int i = 0; i < i42; ++i)
                    {
                        sb.Append("(?:").Append(rule42.RegexExpression).Append(')');
                    }

                    for (int i = 0; i < i31; ++i)
                    {
                        sb.Append("(?:").Append(rule31.RegexExpression).Append(')');
                    }

                    sb.Append('$');

                    patterns.Add(sb.ToString());
                }
            }

            return options is null
                ? patterns.Select(pattern => new Regex(pattern)).ToList()
                : patterns.Select(pattern => new Regex(pattern, (RegexOptions)options)).ToList();
        }

        internal static int CountMatches_ParallelForEach_Interlock(IEnumerable<string> messages, IEnumerable<Regex> regexes)
        {
            var counter = 0;

            Parallel.ForEach(regexes, (regex) =>
            {
                Parallel.ForEach(messages, (message) =>
                {
                    if (regex.IsMatch(message))
                    {
                        Interlocked.Increment(ref counter);
                    }
                });
            });

            return counter;
        }

        internal (List<Rule> rules, List<string> messages) ParseInput(string InputFilePath)
        {
            var rules = new List<Rule>();
            var messages = new List<string>();

            var groups = ReadAllGroupsOfLines(InputFilePath);
            Debug.Assert(groups.Count == 2);

            foreach (var rule in groups[0])
            {
                var split = rule.Split(":", StringSplitOptions.TrimEntries);
                Debug.Assert(split.Length == 2);

                rules.Add(new Rule(int.Parse(split[0]), split[1]));
            }

            messages.AddRange(groups[1]);

            return (rules, messages);
        }
        
        public static List<List<string>> ReadAllGroupsOfLines(string path)
        {
            return File.ReadAllText(path)
                .Replace("\r\n", "\n")
                .Split("\n\n")
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList())
                .ToList();
        }

        internal class Rule
        {
            public int Id { get; }

            public string Content { get; set; }

            public bool IsLiteral { get; set; }

            public HashSet<int> RelatedRules { get; }

            public StringBuilder RegexExpression { get; set; }

            public Rule(int id, string content)
            {
                Id = id;
                IsLiteral = content.StartsWith("\"");
                Content = content.Replace("\"", string.Empty);
                RegexExpression = new StringBuilder();

                RelatedRules = IsLiteral
                    ? new HashSet<int>(0)
                    : new HashSet<int>(Content
                            .Split(' ', StringSplitOptions.TrimEntries)
                            .Where(str => str != "|")
                            .Select(int.Parse));
            }
        }
    }
}