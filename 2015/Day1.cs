namespace AdventOfCode._2015;

/// <summary>
/// <see href="https://adventofcode.com/2015/day/1"/>
/// </summary>
[Day(2015, 1)]
internal class Day1 : Day
{
    public override string Input => Resources._2015_1_Input;

    public override async Task Solve(string input, long[] totals)
    {
        int? basementPos = null;

        for (int i = 0; i < input.Length; i++)
        {
            totals[0] += input[i] == '(' ? 1 : -1;

            if (basementPos == null && totals[0] == -1)
                basementPos = i + 1;
        }

        totals[1] = (long)basementPos;
    }
}