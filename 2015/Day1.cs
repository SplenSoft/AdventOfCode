namespace AdventOfCode._2015;

/// <summary>
/// <see href="https://adventofcode.com/2015/day/1"/>
/// </summary>
internal class Day1 : Day
{
    public override int Year => 2015;

    public override int DayNumber => 1;

    public override string Synopsis => @"";

    public override string Input => Resources._2015_1_Input;

    public override async Task<string> Solve(string input)
    {
        int floor = 0;
        int? basementPos = null;

        for (int i = 0; i < input.Length; i++)
        {
            floor += input[i] == '(' ? 1 : -1;

            if (basementPos == null && floor == -1)
                basementPos = i + 1;
        }

        return $"Part 1 solution: {floor}\nPart 2 solution: {basementPos}";
    }
}