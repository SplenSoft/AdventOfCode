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
    //59
    public override string Solve(string input)
    {
        string[] n = input.Split(Environment.NewLine);
        var rules1 = n.Where(x => x.Contains('|')).Select(x => x.Split('|'));
        var ups1 = n.Where(x => x.Contains(',')).Select(x => x.Split(','));
        Dictionary<string, List<string>> rules = [];
        int[] total = new int[2];
        int badIndex;

        foreach (var nums in rules1)
        {
            if (!rules.ContainsKey(nums[0])) rules[nums[0]] = [];
            rules[nums[0]].Add(nums[1]);
        }

        foreach (var arr in ups1) 
        {
            int k = 0;
            do
            {
                badIndex = -1;
                for (int i = 1; i < arr.Length; i++)
                    if (badIndex < 0 && rules.ContainsKey(arr[i]))
                        for (int j = 0; j < i; j++)
                            if (rules[arr[i]].Contains(arr[j]))
                                badIndex = i;
                if (badIndex == -1) break;
                k = 1;
                var val = arr[badIndex];
                var list = arr.ToList();
                list.RemoveAt(badIndex);
                list.Insert(badIndex - 1, val);
                list.CopyTo(0, arr, 0, list.Count);
            } while (badIndex > -1);
            total[k] += int.Parse(arr[arr.Length / 2]);
        }

        return $"Part 1 solution: {total[0]}\nPart 2 solution: {total[1]}";
    }
}