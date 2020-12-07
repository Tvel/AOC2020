using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Day3
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
            int rowLen = lines[0].Length;

            {
                int currentRow = 0;
                int currentRight = 0;

                int treesHit = 0;
                while (currentRow != lines.Length - 1)
                {
                    currentRow += 1;
                    currentRight += 3;

                    if (lines[currentRow][currentRight % rowLen] == '#')
                        treesHit++;

                }

                Console.WriteLine(treesHit);
            }
            
            // part 2
            {
                (int Right, int Down)[] paths = new[]
                     {
                         (1, 1), (3,1), (5,1), (7,1), (1,2)
                     };

                long treesHitTotal = 1;

                foreach ((int Right, int Down) in paths)
                {
                    int currentRow = 0;
                    int currentRight = 0;
                    
                    int treesHit = 0;

                    while (currentRow != lines.Length - 1)
                    {
                        currentRow += Down;
                        currentRight += Right;

                        if (lines[currentRow][currentRight % rowLen] == '#')
                            treesHit++;
                    }
                    
                    Console.WriteLine(treesHit);
                    treesHitTotal *= treesHit;
                }
                
                Console.WriteLine(treesHitTotal);

            }
        }
    }
}