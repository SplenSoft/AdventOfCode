using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/11"/>
/// </summary>
[Day(2024, 11)]
internal class Day11 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        int part = 0;
        foreach (int max in new List<int> { 25, 75 })
        {
            Dictionary<long, long> stones = [];
            long total = 0;
            input.Split(' ').Select(long.Parse).ToList()
                .ForEach(x => Process(x, 1, ref total));
            
            void Process(long stoneNum, long toAdd, ref long total)
            {
                stones.TryGetValue(stoneNum, out long amt);
                stones[stoneNum] = amt + toAdd;
                total += toAdd;
            }

            for (int blink = 1; blink <= max; blink++)
            {
                total = 0;
                var list = stones.ToList();
                stones.Clear();
                foreach (var stone in list)
                    if (stone.Key == 0) Process(1, stone.Value, ref total);
                    else if (Math.Floor(Math.Log10(stone.Key) + 1) % 2 == 0)
                    {
                        long tens = (long)Math.Pow(10, 
                            (int)Math.Floor(Math.Log10(stone.Key) + 1) / 2);
                        var left = stone.Key / tens;
                        var right = stone.Key % tens;
                        Process(left, stone.Value, ref total);
                        Process(right, stone.Value, ref total);
                    }
                    else Process(stone.Key * 2024, stone.Value, ref total);
            }
            totals[part++] = total;
        }
    }
}

