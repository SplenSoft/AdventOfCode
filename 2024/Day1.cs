using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/1"/>
/// </summary>
internal class Day1 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 1;

    public override string Synopsis => @"";

    public override string Input => Resources._2024_1_Input;

    /// <param name="input">
    /// A string consisting of multiple lines of two side-by-side 
    /// numbers, separated by three spaces
    /// </param>
    public override string Solve(string input)
    {
        var regex = new Regex(@"(\d{5})\s{3}(\d{5})(?:\n|\r\n|$)");
        List<List<int>> lists = [[], []];

        foreach (Match match in regex.Matches(input))
            for (int i = 0; i < lists.Count; i++)
                lists[i].Add(int.Parse(match.Groups[i + 1].Value));

        for (int i = 0; i < lists.Count; i++)
            lists[i] = [.. lists[i].Order()];

        int part1 = 0;
        int part2 = 0;

        for (int i = 0; i < lists[0].Count; i++)
        {
            int item1 = lists[0][i];
            part1 += Math.Abs(item1 - lists[1][i]);
            int appearances = 0;

            for (int j = 0; j < lists[1].Count; j++)
                if (item1 == lists[1][j]) appearances++;

            part2 += item1 * appearances;
        }

        return $"Part 1 solution: {part1}\nPart 2 solution: {part2}";
    }
}