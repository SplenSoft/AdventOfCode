﻿using System.Numerics;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/9"/>
/// </summary>
[Day(2024, 9)]
internal class Day9 : Day
{
    public override async Task Solve(string input, dynamic[] tot)
    {
        int p = 0; // Part 1 or part 2, increments at end
        Start:
        var list = input.Select(x => new List<int>())
            .Take(input.Length / 2 + 1).ToList();
        int x = list.Count - 1, pos = -1;

        for (int i = 0; i < input.Length; i++) // Decompress
            for (int j = 0; j < int.Parse(input[i].ToString()); j++)
                list[i / 2].Add(i % 2 == 0 ? i / 2 : -1);
        Loop:
        for (; x >= 0; x--)
            for (int y = list[x].Count - 1; y >= 0; y--)
            { // Search from the back until we find a number
                if (list[x][y] == -1) continue;
                int count = list[x].Where(a => a == list[x][y]).Count();
                var num = new Vector3(x, y, count);

                for (int x1 = 0; x1 < list.Count; x1++)
                    for (int y1 = 0; y1 < list[x1].Count; y1++)
                        if (x1 < x && list[x1][y1] == -1)
                        { // Search from the front until we find emtpy space
                            count = list[x1].Where(a => a == -1).Count();
                            int range = p == 1 ? (int)num.Z : 1;
                            if (count < range) continue;
                            int n = list[x][y];
                            int numberIndex = p == 0 ? list[x]
                                .LastIndexOf(n) : list[x].IndexOf(n);
                            list[x].RemoveRange(numberIndex, range);
                            list[x1].RemoveRange(y1, range);
                            for (int i = 0; i < range; i++)
                            { // Swap empty(s) with numbers
                                list[x1].Insert(y1, n);
                                list[x].Insert(numberIndex, -1);
                            }
                            if (p == 1) x--; // In part 2, move to next block
                            goto Loop;
                        }
            }

        for (x = 0; x < list.Count; x++)
            for (int y = 0; y < list[x].Count; y++)
                tot[p] += list[x][y] == -1 ? ++pos * 0 : (++pos * list[x][y]);
        if (p++ < 1) goto Start;
    }
}