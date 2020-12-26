using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
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
                var rpns = lines.Select(x => LeftToRightShuttingYard.ToPostfix(x)).ToList();
                var values = rpns.Select(x => CalculateRPN.Calculate(x)).ToList();
                
                Console.WriteLine(values.Sum());
            }
            
            //part2
            {
                var rpns = lines.Select(x => AdditionFirstShuntingYard.ToPostfix(x)).ToList();
                var values = rpns.Select(x => CalculateRPN.Calculate(x)).ToList();
                
                Console.WriteLine(values.Sum());
            }
        }
    }
    
    public static class AdditionFirstShuntingYard
    {
        private static readonly Dictionary<string, (string symbol, int precedence, bool rightAssociative)> operators
            = new (string symbol, int precedence, bool rightAssociative) [] {
                ("*", 2, false),
                ("+", 3, false)
        }.ToDictionary(op => op.symbol);
    
        public static string ToPostfix(this string infix) {
            string[] tokens = infix
                .Replace("(", "( ")
                .Replace(")", " )").Split(' ');
            
            var stack = new Stack<string>();
            var output = new List<string>();
            
            foreach (string token in tokens) {
                if (int.TryParse(token, out _)) {
                    output.Add(token);
                    Print(token);
                } else if (operators.TryGetValue(token, out var op1)) {
                    while (stack.Count > 0 && operators.TryGetValue(stack.Peek(), out var op2)) {
                        int c = op1.precedence.CompareTo(op2.precedence);
                        if (c < 0 || !op1.rightAssociative && c <= 0) {
                            output.Add(stack.Pop());
                        } else {
                            break;
                        }
                    }
                    stack.Push(token);
                    Print(token);
                } else if (token == "(") {
                    stack.Push(token);
                    Print(token);
                } else if (token == ")") {
                    string top = "";
                    while (stack.Count > 0 && (top = stack.Pop()) != "(") {
                        output.Add(top);
                    }
                    if (top != "(") throw new ArgumentException("No matching left parenthesis.");
                    Print(token);
                }
            }
            while (stack.Count > 0) {
                var top = stack.Pop();
                if (!operators.ContainsKey(top)) throw new ArgumentException("No matching right parenthesis.");
                output.Add(top);
            }
            Print("pop");
            return string.Join(" ", output);
            
            //void Print(string action) => Console.WriteLine("{0,-4} {1,-18} {2}", action + ":", $"stack[ {string.Join(" ", stack.Reverse())} ]", $"out[ {string.Join(" ", output)} ]");
            void Print(string action)  { return;};
        }
    }
    
    public static class LeftToRightShuttingYard
    {
        private static readonly Dictionary<string, string> operators
            = new [] {
                "^",
                "*",
                "/",
                "+",
                "-"
        }.ToDictionary(op => op);
        
        public static string ToPostfix(this string infix) {
            string[] tokens = infix
                .Replace("(", "( ")
                .Replace(")", " )").Split(' ');
            
            var stack = new Stack<string>();
            var output = new List<string>();
            foreach (string token in tokens) {
                if (int.TryParse(token, out _)) {
                    output.Add(token);
                    Print(token);
                } else if (operators.TryGetValue(token, out var op1)) {
                    while (stack.Count > 0 && operators.TryGetValue(stack.Peek(), out var op2)) {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token);
                    Print(token);
                } else if (token == "(") {
                    stack.Push(token);
                    Print(token);
                } else if (token == ")") {
                    string top = "";
                    while (stack.Count > 0 && (top = stack.Pop()) != "(") {
                        output.Add(top);
                    }
                    if (top != "(") throw new ArgumentException("No matching left parenthesis.");
                    Print(token);
                }
            }
            while (stack.Count > 0) {
                var top = stack.Pop();
                if (!operators.ContainsKey(top)) throw new ArgumentException("No matching right parenthesis.");
                output.Add(top);
            }
            Print("pop");
            return string.Join(" ", output);
        
            //void Print(string action) => Console.WriteLine("{0,-4} {1,-18} {2}", action + ":", $"stack[ {string.Join(" ", stack.Reverse())} ]", $"out[ {string.Join(" ", output)} ]");
            void Print(string action)  { return;};
        
        }
    }
    
    public static class CalculateRPN {
        public static decimal Calculate(string rpn)
            {
                string[] rpnTokens = rpn.Split(' ');
                Stack<decimal> stack = new Stack<decimal>();
                decimal number = decimal.Zero;
     
                foreach (string token in rpnTokens)
                {
                    if (decimal.TryParse(token, out number))
                    {
                        stack.Push(number);
                    }
                    else
                    {
                        switch (token)
                        {
                            // case "^":
                            // case "pow":
                            //     {
                            //         number = stack.Pop();
                            //         stack.Push((decimal)Math.Pow((double)stack.Pop(), (double)number));
                            //         break;
                            //     }
                            // case "ln":
                            //     {
                            //         stack.Push((decimal)Math.Log((double)stack.Pop(), Math.E));
                            //         break;
                            //     }
                            // case "sqrt":
                            //     {
                            //         stack.Push((decimal)Math.Sqrt((double)stack.Pop()));
                            //         break;
                            //     }
                            case "*":
                                {
                                    stack.Push(stack.Pop() * stack.Pop());
                                    break;
                                }
                            // case "/":
                            //     {
                            //         number = stack.Pop();
                            //         stack.Push(stack.Pop() / number);
                            //         break;
                            //     }
                            case "+":
                                {
                                    stack.Push(stack.Pop() + stack.Pop());
                                    break;
                                }
                            // case "-":
                            //     {
                            //         number = stack.Pop();
                            //         stack.Push(stack.Pop() - number);
                            //         break;
                            //     }
                            default:
                                throw new Exception("Error in CalculateRPN(string) Method!");
                        }
                    }
                }
     
                return stack.Pop();
            }
        }
    
    // public static class ShuntingYard
    // {
    //     private static readonly Dictionary<string, (string symbol, int precedence, bool rightAssociative)> operators
    //         = new (string symbol, int precedence, bool rightAssociative) [] {
    //             ("^", 4, true),
    //             ("*", 3, false),
    //             ("/", 3, false),
    //             ("+", 2, false),
    //             ("-", 2, false)
    //     }.ToDictionary(op => op.symbol);
    //
    //     public static string ToPostfix(this string infix) {
    //         string[] tokens = infix
    //              .Replace("(", "( ")
    //              .Replace(")", " )").Split(' ');
    //         var stack = new Stack<string>();
    //         var output = new List<string>();
    //         foreach (string token in tokens) {
    //             if (int.TryParse(token, out _)) {
    //                 output.Add(token);
    //                 Print(token);
    //             } else if (operators.TryGetValue(token, out var op1)) {
    //                 while (stack.Count > 0 && operators.TryGetValue(stack.Peek(), out var op2)) {
    //                     int c = op1.precedence.CompareTo(op2.precedence);
    //                     if (c < 0 || !op1.rightAssociative && c <= 0) {
    //                         output.Add(stack.Pop());
    //                     } else {
    //                         break;
    //                     }
    //                 }
    //                 stack.Push(token);
    //                 Print(token);
    //             } else if (token == "(") {
    //                 stack.Push(token);
    //                 Print(token);
    //             } else if (token == ")") {
    //                 string top = "";
    //                 while (stack.Count > 0 && (top = stack.Pop()) != "(") {
    //                     output.Add(top);
    //                 }
    //                 if (top != "(") throw new ArgumentException("No matching left parenthesis.");
    //                 Print(token);
    //             }
    //         }
    //         while (stack.Count > 0) {
    //             var top = stack.Pop();
    //             if (!operators.ContainsKey(top)) throw new ArgumentException("No matching right parenthesis.");
    //             output.Add(top);
    //         }
    //         Print("pop");
    //         return string.Join(" ", output);
    //  
    //         //Yikes!
    //         //void Print(string action) => Console.WriteLine($"{action + ":",-4} {$"stack[ {string.Join(" ", stack.Reverse())} ]",-18} {$"out[ {string.Join(" ", output)} ]"}");
    //         //A little more readable?
    //         void Print(string action) => Console.WriteLine("{0,-4} {1,-18} {2}", action + ":", $"stack[ {string.Join(" ", stack.Reverse())} ]", $"out[ {string.Join(" ", output)} ]");
    //         
    //         //void Print(string action)  { return;};
    //
    //     }
    // }
}

