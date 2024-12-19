using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/13"/>
/// </summary>
[Day(2024, 13)]
internal class Day13 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        string[] lines = input.Split(Environment.NewLine)
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var regexButton = new Regex(@"Button (?:A|B): X\+(\d+), Y\+(\d+)");

        Vector2 Match(Regex regex, string line)
        {
            var g = regex.Match(line).Groups;
            return new (int.Parse(g[1].Value), int.Parse(g[2].Value));
        }

        for (int j = 0; j < 2; j++)
            for (int i = 0; i < lines.Length; i += 3)
            {   // Good ol' algebra
                Vector2 a = Match(regexButton, lines[i]);
                Vector2 b = Match(regexButton, lines[i + 1]);
                var pr = Match(new(@"Prize: X=(\d+), Y=(\d+)"), lines[i + 2]);
                long[] p = [(long)pr.X, (long)pr.Y];
                p[0] = j == 1 ? /*Part 2*/ p[0] + 10000000000000 : p[0]; 
                p[1] = j == 1 ? /*Part 2*/ p[1] + 10000000000000 : p[1];
                double y = (p[1] * (double)a.X - p[0] * (double)a.Y) 
                    / (b.Y * (double)a.X - b.X * (double)a.Y);
                double x = (p[0] - y * b.X) / a.X;
                if (x % 1 != 0 || y % 1 != 0) continue; // Not an int
                totals[j] += (long)x * 3 + (long)y;
            }
    }

    // Some algebraic notes for posterity.
    //x * a1 + y * b1 = p1
    //x * a1 = p1 - y * b1
    //x = (p1 - y * b1) / a1

    //x * a2 + y * b2 = p2
    //y * b2 = p2 - x * a2
    //y = (p2 - x * a2) / b2

    //p2 = (p1 - y * b1) / a1 * a2 + y * b2
}
