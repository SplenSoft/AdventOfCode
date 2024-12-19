using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/17"/>
/// </summary>
[Day(2024, 17)]
internal class Day17 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);
        
        //for (int i = 0; i < 3; i++)
        //{
        //    Regex regex = new(@"Register\s(\w):\s(\d+)");
        //    var g = regex.Match(lines[i]).Groups;
        //    registers[g[1].Value[0]] = long.Parse(g[2].Value);
        //}
        List<long> masterProgram = lines[4][9..].Split(',').Select(long.Parse).ToList();
        string programString = string.Join(',', masterProgram);
        long p = Environment.ProcessorCount;
        long per = 100000;
        long iter = 0;
        totals[1] = long.MaxValue;
        bool found = false;
        object locker = new();
        while (!found)
        {
            List<Action> tasks = new List<Action>();

            for (int j = 0; j < p; j++)
            {
                long start = iter * j * per;
                var program = masterProgram.ToList();
                tasks.Add(() =>
                {
                    for (long q = start; q < start + per; q++)
                    {
                        List<long> output = [];
                        Dictionary<char, long> registers = new() { { 'A', q }, { 'B', 0 }, { 'C', 0 } };
                        long GetComboOperand(long operand) => operand switch
                        {
                            4 => registers['A'],
                            5 => registers['B'],
                            6 => registers['C'],
                            _ => operand,
                        };
                        for (int i = 0; i < program.Count;)
                        {
                            int opcode = (int)program[i];
                            if (opcode == 0)
                            {
                                registers['A'] = (long)Math.Floor(registers['A'] / Math.Pow(2, GetComboOperand(program[i + 1])));
                            }
                            else if (opcode == 1)
                            {
                                registers['B'] ^= program[i + 1];
                            }
                            else if (opcode == 2)
                            {
                                registers['B'] = GetComboOperand(program[i + 1]) % 8;
                            }
                            else if (opcode == 3)
                            {
                                if (registers['A'] != 0)
                                {
                                    i = (int)program[i + 1];
                                    continue;
                                }
                            }
                            else if (opcode == 4)
                            {
                                registers['B'] ^= registers['C'];
                            }
                            else if (opcode == 5)
                            {
                                output.Add(GetComboOperand(program[i + 1]) % 8);
                            }
                            else if (opcode == 6)
                            {
                                registers['B'] = (long)Math.Floor(registers['A'] / Math.Pow(2, GetComboOperand(program[i + 1])));
                            }
                            else if (opcode == 7)
                            {
                                registers['C'] = (long)Math.Floor(registers['A'] / Math.Pow(2, GetComboOperand(program[i + 1])));
                            }
                            i += 2;
                        }

                        if (string.Join(',', output) == programString)
                        {
                            found = true;
                            lock (locker)
                            {
                                totals[1] = Math.Min(q, totals[1]);
                            } 
                        }
                    }
                    
                });
            }

            Parallel.Invoke(tasks.ToArray());
            iter++;
            Console.Write($"\r{iter * per}");
        }

        //Console.WriteLine('\n' + string.Join(',', output)); 
    }
}
