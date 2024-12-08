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
        bool Solve(long res, long[] ints, int o, int t)
        {
            if (ints[0] > res) return false;
            if (ints.Length == 1) return ints[0] == res;

            long n1 = o == 0 ? ints[0] + ints[1] : o == 1 ? ints[0] * ints[1] 
                : long.Parse(ints[0].ToString() + ints[1].ToString());

            for (int i = 0; i < t; i++)
                if (Solve(res, [..new long[1] { n1 }, ..ints.Skip(2)], i, t))
                    return true;

            return false;
        }

        long[] totals = [0, 0];
        string[] lines = input.Split(Environment.NewLine);
        for (int totalOps = 2; totalOps < 4; totalOps++)
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