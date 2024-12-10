using System.Collections.ObjectModel;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/5"/>
/// </summary>
[Day(2024, 5)]
internal class Day5 : Day
{
    public override string Input => Resources._2024_5_Input;

    public override async Task Solve(string input, long[] totals)
    {
        string[] n = input.Split(Environment.NewLine);
        var r = n.Where(x => x.Contains('|')).Select(x => x.Split('|'));
        Dictionary<string, List<string>> rules = [];
        foreach (var x in r.Select(x => x[0]).Distinct())
            rules[x] = [.. r.Where(y => y[0] == x).Select(y => y[1])];

        foreach (var arr in n.Where(x => x.Contains(','))
            .Select(x => x.Split(',')))
        {
            int k = 0;
            Top:
            for (int i = 1; i < arr.Length; i++)
                if (rules.ContainsKey(arr[i]))
                    for (int j = 0; j < i; j++)
                        if (rules[arr[i]].Contains(arr[j]))
                        {
                            k = 1;
                            ObservableCollection<string> col = new(arr);
                            col.Move(i, i - 1);
                            col.CopyTo(arr, 0);
                            goto Top;
                        }

            totals[k] += int.Parse(arr[arr.Length / 2]);
        }
    }
}