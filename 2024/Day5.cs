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
    //63
    public override string Solve(string input)
    {
        string[] lines = input.Split(Environment.NewLine);
        var ruleStrings = lines.Where(x => x.Contains('|'))
            .Select(x => x.Split('|').Select(int.Parse).ToArray());
        var updateStrings = lines.Where(x => x.Contains(','))
            .Select(x => x.Split(',').Select(int.Parse).ToArray());
        Dictionary<int, List<int>> rules = [];
        int[] total = new int[2];
        int badIndex;

        foreach (var nums in ruleStrings)
        {
            if (!rules.ContainsKey(nums[0])) rules[nums[0]] = [];
            rules[nums[0]].Add(nums[1]);
        }

        foreach (var arr in updateStrings) 
        {
            int k = 0;
            do
            {
                badIndex = -1;
                for (int i = 1; i < arr.Length; i++)
                    if (badIndex < 0 && rules.TryGetValue(arr[i], out var val))
                        for (int j = 0; j < i; j++)
                            if (val.Contains(arr[j]))
                                badIndex = i;
                if (badIndex != -1)
                {
                    k = 1;
                    int val = arr[badIndex];
                    var list = arr.ToList();
                    list.RemoveAt(badIndex);
                    list.Insert(badIndex - 1, val);
                    list.CopyTo(0, arr, 0, list.Count);
                }
            } while (badIndex > -1);
            total[k] += arr[arr.Length / 2];
        }

        return $"Part 1 solution: {total[0]}\nPart 2 solution: {total[1]}";
    }
}