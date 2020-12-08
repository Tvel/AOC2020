using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

namespace Day8
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
            List<(Op instr, int val)> ops = ParseInstructions(lines);

            {
                int register = 0;
                int programAddress = 0;
                List<int> history = new List<int>(ops.Count);

                while (!history.Contains(programAddress))
                {
                    history.Add(programAddress);
                    var op = ops[programAddress];
                    switch (op.instr)
                    {
                        case Op.Nop:
                            programAddress++;
                            break;
                        case Op.Acc:
                            register += op.val;
                            programAddress++;
                            break;
                        case Op.Jmp:
                            programAddress += op.val;
                            break;
                    }
                }

                Console.WriteLine(register);
            }
            
            //part2
            {
                for (int i = 0; i < ops.Count; i++)
                {
                    var newOp = ops[i];
                    if (ops[i].instr == Op.Nop) newOp.instr = Op.Jmp;
                    else if (ops[i].instr == Op.Jmp) newOp.instr = Op.Nop;
                    else continue;
                    
                    var newOps = ops.Select(x => x).ToArray();
                    newOps[i] = newOp;
                    
                    int register = 0;
                    int programAddress = 0;
                    List<int> history = new List<int>(newOps.Length);
                    while (!history.Contains(programAddress))
                    {
                        history.Add(programAddress);
                        if (programAddress > newOps.Length - 1)
                        {
                            Console.WriteLine(register);
                            goto exit;
                        }
                        var op = newOps[programAddress];
                        switch (op.instr)
                        {
                            case Op.Nop:
                                programAddress++;
                                break;
                            case Op.Acc:
                                register += op.val;
                                programAddress++;
                                break;
                            case Op.Jmp:
                                programAddress += op.val;
                                break;
                        }
                    }
                }
                exit: ;
            }


        }

        private static List<(Op, int)> ParseInstructions(string[] lines)
        {
            var ops = new List<(Op, int)>();
            foreach (var line in lines)
            {
                var split = line.Split(" ").Select(x => x.Trim()).ToArray();
                var op = split[0] switch
                    {
                        "nop" => Op.Nop,
                        "acc" => Op.Acc,
                        "jmp" => Op.Jmp,
                        _ => throw new Exception("Invalid op")
                    };
                var val = int.Parse(split[1]);

                ops.Add((op, val));
            }

            return ops;
        }
    }

    public enum Op
    {
        Nop,
        Acc,
        Jmp
    }
}