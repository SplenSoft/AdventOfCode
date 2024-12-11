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
        List<long> stones = [.. input.Split(' ').Select(long.Parse)];
        ConcurrentBag<long> newStones = [];
        for (int i = 0; i < 75; i++)
        {
            Console.WriteLine($"Blink {i + 1} of {75}");
            newStones.Clear();

            long stonesPerProcessor = (long)Math.Ceiling((decimal)stones.Count / Environment.ProcessorCount);

            List<Action> tasks = [];

            for (int j = 0; j < stonesPerProcessor; j++) 
            { 
                
            }

            Parallel.ForEach(stones, stone =>
            {
                string asString = stone.ToString();
                if (stone == 0)
                {
                    newStones.Add(1);
                }
                else if (asString.Length % 2 == 0)
                {
                    int halfIndex = (asString.Length / 2);
                    int remainder = asString.Length - halfIndex;
                    newStones.Add(long.Parse(asString[..halfIndex]));
                    newStones.Add(long.Parse(asString[^remainder..]));
                }
                else
                {
                    newStones.Add(stone * 2024);
                }
            });
            stones = [..newStones];
            if (i == 24) totals[0] = stones.Count;
            if (i == 75) totals[1] = stones.Count;
        }
    }
}
