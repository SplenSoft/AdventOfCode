using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/9"/>
/// </summary>
[Day(2024, 9)]
internal class Day9 : Day
{
    //public override string Input => Resources._2024_9_Input;
    public override string Input => Resources._2024_9_Test_Input;
    //74
    public override async Task<string> Solve(string input)
    {
        string solution = "";
        int part = 0;
        Start:
        var list = input.Select(x => new List<int>())
            .Take(input.Length / 2 + 1).ToList();

        for (int i = 0; i < input.Length; i++)
            for (int j = 0; j < int.Parse(input[i].ToString()); j++)
                list[i / 2].Add(i % 2 == 0 ? i / 2 : -1);

        int x = list.Count - 1;
        Loop:
        for (; x >= 0; x--)
            for (int y = list[x].Count - 1; y >= 0; y--)
            {
                if (list[x][y] == -1) continue;

                int count = list[x].Where(a => a == list[x][y]).Count();
                var num = new Vector3(x, y, count);

                for (int x1 = 0; x1 < list.Count; x1++)
                    for (int y1 = 0; y1 < list[x1].Count; y1++)
                        if (x1 < x && list[x1][y1] == -1)
                        {
                            count = list[x1].Where(a => a == -1).Count();
                            int range = part == 1 ? (int)num.Z : 1;
                            if (count < range) continue;
                            int n = list[x][y];
                            int numberIndex = part == 0 ? list[x]
                                .LastIndexOf(n) : list[x].IndexOf(n);
                            list[x].RemoveRange(numberIndex, range);
                            list[x1].RemoveRange(y1, range);
                            for (int i = 0; i < range; i++)
                            {
                                list[x1].Insert(y1, n);
                                list[x].Insert(numberIndex, -1);
                            }
                            if (part == 1) x--;
                            goto Loop;
                        }
            }

        long total = 0;
        int pos = -1;

        for (x = 0; x < list.Count; x++)
            for (int y = 0; y < list[x].Count; y++)
                total += list[x][y] == -1 ? ++pos * 0 : ((long)++pos * list[x][y]);

        solution += $"Part {part + 1} solution:{total}\n";

        if (part++ < 1) goto Start;
        return solution;
    }
}
