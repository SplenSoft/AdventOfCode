using System;
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
        for (int i = 0; i < 75; i++)
        {
            Console.WriteLine($"Blink {i + 1} of {75}");
            for (int j = 0; j < stones.Count; j++)
            {
                long stone = stones[j];
                if (stone == 0) stones[j] = 1;
                else if (stone.ToString().Length % 2 == 0)
                {
                    int halfIndex = (stone.ToString().Length / 2);
                    int remainder = stone.ToString().Length - halfIndex;
                    stones[j] = long.Parse(stone.ToString()[..halfIndex]);
                    stones.Insert(j + 1, long.Parse(stone.ToString()[^remainder..]));
                    j++;
                }
                else
                {
                    stones[j] = stones[j] * 2024;
                }
            }

            if (i == 24) totals[0] = stones.Count;
            if (i == 75) totals[1] = stones.Count;
        }
    }
}
