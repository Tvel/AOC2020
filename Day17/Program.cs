using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
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
                Dictionary<(int x, int y, int z), char> cubes = new();

                for (int y = 0; y < lines.Length; y++)
                {
                    var line = lines[y];
                    
                    for (int x = 0; x < line.Length; x++)
                    {
                        cubes[(x, y, 0)] = line[x];

                        GetAndSetToNewIfNotExists(x - 1, y - 1, 0, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y - 1, 0, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y + 0, 0, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y + 0, 0, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y + 1, 0, cubes);
                        GetAndSetToNewIfNotExists(x + 0, y + 1, 0, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y + 1, 0, cubes);

                        GetAndSetToNewIfNotExists(x, y, 1, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y - 1, 1, cubes);
                        GetAndSetToNewIfNotExists(x + 0, y - 1, 1, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y - 1, 1, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y + 0, 1, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y + 0, 1, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y + 1, 1, cubes);
                        GetAndSetToNewIfNotExists(x + 0, y + 1, 1, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y + 1, 1, cubes);

                        GetAndSetToNewIfNotExists(x, y, -1, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y - 1, -1, cubes);
                        GetAndSetToNewIfNotExists(x + 0, y - 1, -1, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y - 1, -1, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y + 0, -1, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y + 0, -1, cubes);
                        GetAndSetToNewIfNotExists(x - 1, y + 1, -1, cubes);
                        GetAndSetToNewIfNotExists(x + 0, y + 1, -1, cubes);
                        GetAndSetToNewIfNotExists(x + 1, y + 1, -1, cubes);
                    }
                }

                
                for (int cycle = 1; cycle <= 6; cycle++)
                {
                    var current = CloneCubes();

                    foreach (var (key, value) in cubes)
                    {
                        var neighbours = new List<char>();
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y - 1, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 0, key.y - 1, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y - 1, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y + 0, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y + 0, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y + 1, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 0, key.y + 1, key.z, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y + 1, key.z, current));

                        neighbours.Add(GetAndSetToNewIfNotExists(key.x, key.y, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y - 1, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 0, key.y - 1, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y - 1, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y + 0, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y + 0, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y + 1, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 0, key.y + 1, key.z + 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y + 1, key.z + 1, current));

                        neighbours.Add(GetAndSetToNewIfNotExists(key.x, key.y, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y - 1, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 0, key.y - 1, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y - 1, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y + 0, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y + 0, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x - 1, key.y + 1, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 0, key.y + 1, key.z - 1, current));
                        neighbours.Add(GetAndSetToNewIfNotExists(key.x + 1, key.y + 1, key.z - 1, current));

                        var active = neighbours.Count(x => x == '#');

                        if (cubes[key] == '.')
                        {
                            if (active == 3) current[key] = '#';
                            else current[key] = '.';
                        }
                        else if(cubes[key] == '#')
                        {
                            if (active == 3 || active == 2) current[key] = '#';
                            else current[key] = '.';
                        }
                    }

                    cubes = current;
                }

                var result = cubes.Count(x => x.Value == '#');
                Console.WriteLine(result);

                char GetAndSetToNewIfNotExists(int x, int y, int z, Dictionary<(int x, int y, int z), char> current)
                {
                    if(!cubes.ContainsKey((x, y, z)))
                    {
                        current[(x, y, z)] = '.';
                        return '.';
                    }

                    return cubes[(x, y, z)];
                }

                Dictionary<(int x, int y, int z), char> CloneCubes()
                {
                    Dictionary<(int x, int y, int z), char> newCubes = new();
                    foreach (var (key, val) in cubes)
                    {
                        newCubes[key] = val;
                    }

                    return newCubes;
                }
            }
            
            //part2
            {
                Dictionary<(int x, int y, int z, int w), char> cubes = new();

                for (int y = 0; y < lines.Length; y++)
                {
                    var line = lines[y];
                    
                    for (int x = 0; x < line.Length; x++)
                    {
                        cubes[(x, y, 0, 0)] = line[x];
                        _ = GetNeighbours((x,y,0,0), cubes);
                    }
                }

                for (int cycle = 1; cycle <= 6; cycle++)
                {
                    var current = CloneCubes();

                    foreach (var (key, value) in cubes)
                    {
                        var neighbours = GetNeighbours(key, current);
                        var active = neighbours.Count(x => x == '#');

                        if (cubes[key] == '.')
                        {
                            if (active == 3) current[key] = '#';
                            else current[key] = '.';
                        }
                        else if(cubes[key] == '#')
                        {
                            if (active == 3 || active == 2) current[key] = '#';
                            else current[key] = '.';
                        }
                    }

                    cubes = current;
                }

                var result = cubes.Count(x => x.Value == '#');
                Console.WriteLine(result);
                
                char GetAndSetToNewIfNotExists(int x, int y, int z, int w, Dictionary<(int x, int y, int z, int w), char> current)
                {
                    if(!cubes.ContainsKey((x, y, z, w)))
                    {
                        current[(x, y, z, w)] = '.';
                        return '.';
                    }

                    return cubes[(x, y, z, w)];
                }
                
                Dictionary<(int x, int y, int z, int w), char> CloneCubes()
                {
                    Dictionary<(int x, int y, int z, int w), char> newCubes = new();
                    foreach (var (key, val) in cubes)
                    {
                        newCubes[key] = val;
                    }

                    return newCubes;
                }

                List<char> GetNeighbours((int x, int y, int z, int w) key, Dictionary<(int x, int y, int z, int w), char> current)
                {
                    var neighbours = new List<char>();

                    for (int x = key.x - 1; x <= key.x + 1; x++)
                    {
                        for (int y = key.y - 1; y <= key.y + 1; y++)
                        {
                            for (int z = key.z - 1; z <= key.z + 1; z++)
                            {
                                for (int w = key.w - 1; w <= key.w + 1; w++)
                                {
                                    if(key == (x,y,z,w)) continue;

                                    neighbours.Add(GetAndSetToNewIfNotExists(x, y, z, w, current));
                                }
                            }
                        }
                    }

                    return neighbours;
                }
            }
        }
    }
}