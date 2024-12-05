using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

/// <summary>
/// <see href="https://adventofcode.com/2015/day/2"/>
/// </summary>
internal class Day2 : Day
{
    public override int Year => 2015;

    public override int DayNumber => 2;

    public override string Synopsis => @"";

    public override string Input => Resources._2015_2_Input;

    public override string Solve(string input)
    {
        var regex = new Regex(@"(\d+?)x(\d+?)x(\d+?)(?:\n|\r\n|$)");
        List<(int, int, int)> list = [];

        foreach (Match match in regex.Matches(input))
            list.Add((int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value)));

        int paper = 0;
        int ribbon = 0;

        foreach (var present in list)
        {
            int dim1 = present.Item1 * present.Item2;
            int dim2 = present.Item2 * present.Item3;
            int dim3 = present.Item3 * present.Item1;

            int perim1 = (present.Item1 * 2) + (present.Item2 * 2);
            int perim2 = (present.Item2 * 2) + (present.Item3 * 2);
            int perim3 = (present.Item3 * 2) + (present.Item1 * 2);

            int surfaceArea = (dim1 * 2) + (dim2 * 2) + (dim3 * 2);

            int smallestDim = dim1 <= dim2 && dim1 <= dim3 ? dim1
                : dim2 <= dim1 && dim2 <= dim3 ? dim2 : dim3;

            int ribbon1 = perim1 <= perim2 && perim1 <= perim3 ? perim1
                : perim2 <= perim1 && perim2 <= perim3 ? perim2 : perim3;

            int volume = present.Item1 * present.Item2 * present.Item3;

            paper += smallestDim + surfaceArea;
            ribbon += volume + ribbon1;
        }

        return $"Part 1 solution: {paper}\nPart 2 solution: {ribbon}";
    }
}
