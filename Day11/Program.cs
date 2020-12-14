using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using Microsoft.VisualBasic.FileIO;

namespace Day11
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
                static char? Get(int line, int column, string[] lines)
                {
                    if (line < 0) return null;
                    if (line >= lines.Length) return null;
                    if (column < 0) return null;
                    if (column >= lines[line].Length) return null;

                    return lines[line][column];
                }
                
                var linesPrev = (string[])lines.Clone();
                while (true)
                {
                    var linesNext =(string[])linesPrev.Clone();
                    for (int l = 0; l < linesPrev.Length; l++)
                    {
                        var line = linesPrev[l];
                        for (int c = 0; c < line.Length; c++)
                        {
                            char?[] adjN = {
                                              Get(l - 1, c - 1, linesPrev),
                                              Get(l - 1, c - 0, linesPrev),
                                              Get(l - 1, c + 1, linesPrev),
                                              
                                              Get(l + 0, c + 1, linesPrev),
                                              
                                              Get(l + 1, c + 1, linesPrev),
                                              Get(l + 1, c + 0, linesPrev),
                                              Get(l + 1, c - 1, linesPrev),
                                              
                                              Get(l + 0, c - 1, linesPrev),
                                          };

                            char[] adj = adjN.Where(x => x.HasValue).Select(x => x.Value).ToArray();
                            
                            
                            var that = Get(l, c, linesPrev)!.Value;

                            if (that == 'L')
                            {
                                if (adj.Count(x => x == '#') == 0)
                                {
                                    linesNext[l] = linesNext[l].ReplaceAt(c, '#');
                                }
                            } else if (that == '#')
                            {
                                if (adj.Count(x => x == '#') >= 4)
                                {
                                    linesNext[l] = linesNext[l].ReplaceAt(c, 'L');
                                }
                            }
                        }
                    }

                    //verify eq
                    var eq = true;
                    for (int l = 0; l < linesPrev.Length; l++)
                    {
                        for (int c = 0; c < linesPrev[l].Length; c++)
                        {
                            if (linesPrev[l][c] != linesNext[l][c])
                            {
                                eq = false;
                                break;
                            }
                        }
                        if(eq == false) break;
                    }

                    if(eq == true) break;

                    //prep loop
                    linesPrev = linesNext;
                }

                var a = linesPrev.SelectMany(x => x).Count(x => x == '#');
                Console.WriteLine(a);
            }


            //part2
            {
                static char? Get(int line, int column, string[] lines, (int l, int c) vec2)
                {
                    do
                    {
                        line += vec2.l;
                        column += vec2.c;
                        
                        if (line < 0) return null;
                        if (line >= lines.Length) return null;
                        if (column < 0) return null;
                        if (column >= lines[line].Length) return null;

                        var that = lines[line][column];
                        if(that != '.') return lines[line][column];
                    }
                    while (true);
                }

                var linesPrev = (string[])lines.Clone();
                while (true)
                {
                    var linesNext =(string[])linesPrev.Clone();
                    for (int l = 0; l < linesPrev.Length; l++)
                    {
                        var line = linesPrev[l];
                        for (int c = 0; c < line.Length; c++)
                        {
                            char?[] adjN = {
                                              Get(l, c, linesPrev, (-1, -1)),
                                              Get(l, c, linesPrev, (-1, 0)),
                                              Get(l, c, linesPrev, (-1, +1)),
                                              
                                              Get(l, c, linesPrev, (0, +1)),
                                              
                                              Get(l, c, linesPrev, (+1, +1)),
                                              Get(l, c, linesPrev, (+1, 0)),
                                              Get(l, c, linesPrev, (+1, -1)),
                                              
                                              Get(l, c, linesPrev, (0, -1)),
                                          };

                            char[] adj = adjN.Where(x => x.HasValue).Select(x => x.Value).ToArray();
                            
                            var that = linesPrev[l][c];

                            if (that == 'L')
                            {
                                if (adj.Count(x => x == '#') == 0)
                                {
                                    linesNext[l] = linesNext[l].ReplaceAt(c, '#');
                                }
                            } else if (that == '#')
                            {
                                if (adj.Count(x => x == '#') >= 5)
                                {
                                    linesNext[l] = linesNext[l].ReplaceAt(c, 'L');
                                }
                            }
                        }
                    }

                    //verify eq
                    var eq = true;
                    for (int l = 0; l < linesPrev.Length; l++)
                    {
                        for (int c = 0; c < linesPrev[l].Length; c++)
                        {
                            if (linesPrev[l][c] != linesNext[l][c])
                            {
                                eq = false;
                                break;
                            }
                        }
                        if(eq == false) break;
                    }

                    if(eq == true) break;

                    //prep loop
                    linesPrev = linesNext;
                }

                var a = linesPrev.SelectMany(x => x).Count(x => x == '#');
                Console.WriteLine(a);
            }
            
        }
        

    }

    public static class Ext
    {
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }
    }
}