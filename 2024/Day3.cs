using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/3"/>
/// </summary>
internal class Day3 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 3;

    public override string Synopsis => @"";

    public override string Input => Resources._2024_3_Input;

    public override string Solve(string input)
    {
        var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

        string getTrimmed(string s)
        {
            while (s.IndexOf("don't()") != -1)
            {
                int dontIndex = s.IndexOf("don't()");
                string beginning = s[..dontIndex];
                string after = s[^(s.Length - dontIndex)..];
                int index = after.IndexOf("do()") + 3;
                string end = after[^(after.Length - index)..];
                s = beginning + end;
            }

            return s;
        }

        int getSum(string s) => regex.Matches(s)
            .Select(x => int.Parse(x.Groups[1].Value))
            .Zip(regex.Matches(s)
            .Select(x => int.Parse(x.Groups[2].Value)), (x, y) => x * y)
            .Sum();

        int part2 = getSum(getTrimmed(input));
        return $"Part 1 solution: {getSum(input)}\nPart 2 solution: {part2}";
    }
}