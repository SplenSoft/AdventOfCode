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
    public override async Task Solve(string input, dynamic[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);
        long part1Target = long.Parse(lines[0][12..]);
        List<long> prog = lines[4][9..].Split(',').Select(long.Parse).ToList();
        string programString = string.Join(',', prog);      
        int p = Environment.ProcessorCount;
        List<long>[] outs = new List<long>[p];
        Dictionary<char, long>[] reg = new Dictionary<char, long>[p];
        for (int i = 0; i < p; i++) outs[i] = [];
        long per = 100000;
        long iter = 0;
        totals[1] = long.MaxValue;
        bool found = false;
        bool found2 = false;
        object locker = new();
        List<Action> tasks = [];
        while (!found || !found2)
        {
            tasks.Clear();
            for (int j = 0; j < p; j++)
            {
                long start = iter + (j * per);
                int k = j;
                long end = start + per;
                tasks.Add(() =>
                {
                    for (long q = start; q < end; q++)
                    {
                        outs[k].Clear();
                        foreach (var item in reg[k].Keys)
                            reg[k][item] = item == 'A' ? q : 0;

                        long Combo(long operand) => operand switch
                        {
                            4 => reg[k]['A'],
                            5 => reg[k]['B'],
                            6 => reg[k]['C'],
                            _ => operand,
                        };
                        long GetAdv(long op) => (long)Math.Floor(reg[k]['A'] / Math.Pow(2, Combo(op)));
                        for (int i = 0; i < prog.Count;)
                        {
                            int oc = (int)prog[i];
                            if (oc == 0) reg[k]['A'] = GetAdv(prog[i + 1]);
                            else if (oc == 1) reg[k]['B'] ^= prog[i + 1];
                            else if (oc == 2)
                                reg[k]['B'] = Combo(prog[i + 1]) % 8;
                            else if (oc == 3 && reg[k]['A'] != 0)
                            {
                                i = (int)prog[i + 1];
                                continue;
                            }
                            else if (oc == 4) reg[k]['B'] ^= reg[k]['C'];
                            else if (oc == 5) 
                                outs[k].Add(Combo(prog[i + 1]) % 8);
                            else if (oc == 6) reg[k]['B'] = GetAdv(prog[i + 1]);
                            else if (oc == 7) reg[k]['C'] = GetAdv(prog[i + 1]);
                            i += 2;
                        }

                        string output = string.Join(',', outs[k]);
                        if (q == part1Target)
                        {
                            found2 = true;
                            totals[0] = output;
                        }

                        if (output == programString)
                        {
                            found = true;
                            lock (locker)
                                totals[1] = Math.Min(q, totals[1]);
                        }
                    }
                    
                });
            }

            Parallel.Invoke(tasks.ToArray());
            iter += p * per;
            Console.Write($"\r{iter}");
        }

        //Console.WriteLine('\n' + string.Join(',', output)); 
    }
}
