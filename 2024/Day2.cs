namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/2"/>
/// </summary>
internal class Day2 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 2;

    public override string Synopsis => @"";

    public override string Input => Resources._2024_2_Input;

    public override async Task<string> Solve(string input)
    {
        IEnumerable<IEnumerable<int>> reports = input.Split("\r\n")
            .Select(x => x.Split(" ")
            .Select(x => int.Parse(x)));

        bool isReportGood(IEnumerable<int> report)
        {
            var translated = report.Zip(report.Skip(1), (x, y) => y - x);
            return translated.All(x => x is < 0 and > -4)
                || translated.All(x => x is > 0 and < 4);
        }

        bool isAnySubReportGood(IEnumerable<int> report)
        {
            IEnumerable<IEnumerable<int>> subReports = [];

            subReports = reports.Select((x, i) => x.Take(i)
                .Concat(x.TakeLast(report.Count() - (i + 1))));

            return subReports.Any(isReportGood);
        }

        int part1 = reports.Count(isReportGood);
        int part2 = reports.Count(isAnySubReportGood);

        return $"Part 1 solution: {part1}\nPart 2 solution: {part2}";
    }
}