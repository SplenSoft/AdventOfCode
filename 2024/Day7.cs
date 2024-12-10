using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/7"/>
/// </summary>
[Day(2024, 7)]
internal class Day7 : Day
{
    public override string Input => Resources._2024_7_Input;

    public override async Task Solve(string input, long[] totals)
    {
        bool Solve(long res, long[] n, int o /*op*/, int t /*total # ops*/)
        {
            if (n[0] > res) return false;
            if (n.Length == 1) return n[0] == res;

            long n1 = o == 0 ? n[0] + n[1] : o == 1 ? n[0] * n[1] // +, *
                : long.Parse(n[0].ToString() + n[1].ToString()); // ||

            for (int i = 0; i < t; i++) // Combine n1 + n2 as we go
                if (Solve(res, [..new long[1] { n1 }, ..n.Skip(2)], i, t))
                    return true; // We can match res

            return false; // We can't match res
        }

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
    }
}