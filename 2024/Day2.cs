namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/2"/>
/// </summary>
[Day(2024, 2)]
internal class Day2 : Day
{
    public override string Input => Resources._2024_2_Input;

    public override async Task Solve(string input, long[] totals)
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

        totals[0] = reports.Count(isReportGood);
        totals[1] = reports.Count(isAnySubReportGood);
    }
}