using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode._2024;

internal class Day5 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 5;

    public override string Synopsis => throw new NotImplementedException();

    public override string Input => Resources._2024_5_Input;
    //50
    public override string Solve(string input)
    {
        int[] total = new int[2];
        string[] n = input.Split(Environment.NewLine);
        var r = n.Where(x => x.Contains('|')).Select(x => x.Split('|'));
        Dictionary<string, List<string>> rules = [];
        foreach (var x in r.Select(x => x[0]).Distinct())
            rules[x] = [.. r.Where(y => y[0] == x).Select(y => y[1])];

        foreach (var arr in n.Where(x => x.Contains(',')).Select(x => x.Split(','))) 
        {
            int k = 0;
            Top:
            for (int i = 1; i < arr.Length; i++)
                if (rules.ContainsKey(arr[i]))
                    for (int j = 0; j < i; j++)
                        if (rules[arr[i]].Contains(arr[j]))
                        {
                            k = 1;
                            arr.Take(i - 1).Append(arr[i]).Append(arr[i - 1])
                                .Concat(arr.Skip(i + 1)).ToList()
                                .CopyTo(0, arr, 0, arr.Length);
                            goto Top;
                        }

            total[k] += int.Parse(arr[arr.Length / 2]);
        }

        return $"Part 1 solution: {total[0]}\nPart 2 solution: {total[1]}";
    }
}