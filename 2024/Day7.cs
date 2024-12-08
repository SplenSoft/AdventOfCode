using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

internal class Day7 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 7;

    public override string Synopsis => throw new NotImplementedException();

    public override string Input => Resources._2024_7_Input;

    public override async Task<string> Solve(string input)
    {
        bool Solve(long res, long[] n, int o /*op*/, int t /*total # ops*/)
        {
            if (n.Length == 1) return n[0] == res;

            long n1 = o == 0 ? n[0] + n[1] : o == 1 ? n[0] * n[1] // +, *
                : long.Parse(n[0].ToString() + n[1].ToString()); // ||

            for (int i = 0; i < t; i++) // Combine n1 + n2 as we go
                if (Solve(res, [..new long[1] { n1 }, ..n.Skip(2)], i, t))
                    return true; // We can match res

            return false; // We can't match res
        }

        long[] totals = [0 /*part1*/, 0 /*part2*/];
        string[] lines = input.Split(Environment.NewLine);
        for (int totalOps = 2; totalOps < 4; totalOps++) // + and *, then ||
            for (int i = 0; i < lines.Length; i++)
            {
                var g = new Regex(@"(\d+?):\s(.+)").Match(lines[i]).Groups;
                var ints = g[2].Value.Split(' ').Select(long.Parse).ToArray();

                for (int j = 0; j < totalOps; j++)
                    if (Solve(long.Parse(g[1].Value), ints, j, totalOps))
                    {
                        totals[totalOps - 2] += long.Parse(g[1].Value);
                        break;
                    }
            }
        return $"Part 1 solution: {totals[0]}\nPart 2 solution: {totals[1]}";
    }
}