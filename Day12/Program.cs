using System;
using System.IO;

namespace Day12
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
                (int ns, int ew) position = (0, 0);
                int direction = 0; //east

                foreach (var commandString in lines)
                {
                    var command = commandString[0];
                    var value = int.Parse(commandString[1..]);

                    switch (command)
                    {
                        case 'N': position = MovePosition(value, position, (-1, 0)); break;
                        case 'S': position = MovePosition(value, position, ( 1, 0)); break;
                        case 'E': position = MovePosition(value, position, ( 0, 1)); break;
                        case 'W': position = MovePosition(value, position, ( 0,-1)); break;
                        case 'F': position = MovePosition(value, position, ((int)Math.Sin(direction * (Math.PI/180)) , (int)Math.Cos(direction * (Math.PI/180)))); break;
                        
                        case 'L':
                            direction -= value;
                            break;
                        case 'R':
                            direction += value;
                            break;
                    }
                }
                
                Console.WriteLine(Math.Abs(position.ns) + Math.Abs(position.ew));
                
                static (int ns, int ew) MovePosition(int value, (int ns, int ew) current, (int ns, int ew) direction )
                {
                    return (current.ns + (value * direction.ns), current.ew + (value * direction.ew));
                }
            }
            
            
            //part2
            {
                (int ns, int ew) shipPosition = (0, 0);
                (int ns, int ew) waypointPosition = (-1, 10);

                foreach (var commandString in lines)
                {
                    var command = commandString[0];
                    var value = int.Parse(commandString[1..]);

                    switch (command)
                    {
                        case 'N': waypointPosition = MovePosition(value, waypointPosition, (-1, 0)); break;
                        case 'S': waypointPosition = MovePosition(value, waypointPosition, ( 1, 0)); break;
                        case 'E': waypointPosition = MovePosition(value, waypointPosition, ( 0, 1)); break;
                        case 'W': waypointPosition = MovePosition(value, waypointPosition, ( 0,-1)); break;
                        
                        case 'F':
                            shipPosition = (shipPosition.ns += waypointPosition.ns * value, shipPosition.ew += waypointPosition.ew * value); break;
                        
                        case 'L':
                            int times1 = value / 90;
                            for (int i = 0; i < times1; i++)
                            {
                                waypointPosition = (waypointPosition.ew * -1, waypointPosition.ns);
                            }
                            break;
                        case 'R':
                            int times2 = value / 90;
                            for (int i = 0; i < times2; i++)
                            {
                                waypointPosition = (waypointPosition.ew, waypointPosition.ns * -1);
                            }
                            break;
                    }
                }
                
                Console.WriteLine(Math.Abs(shipPosition.ns) + Math.Abs(shipPosition.ew));
                
                static (int ns, int ew) MovePosition(int value, (int ns, int ew) current, (int ns, int ew) direction )
                {
                    return (current.ns + (value * direction.ns), current.ew + (value * direction.ew));
                }
            }
            

        }
    }
}