using System.Numerics;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/8"/>
/// </summary>
[Day(2024, 8)]
internal class Day8 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);
        List<List<Vector2>> nodes = [[/*part1*/], [/*part2*/]];
        List<(char c, Vector2 v)> ants = [..lines.SelectMany((y, i) => y
            .Select((x, j) => (x, new Vector2(j, i)))).Where(x => x.x != '.')];

        ants.ForEach(a1 => ants
            .Where(a2 => a1 != a2 && a1.c == a2.c).ToList().ForEach(a2 =>
            {
                var node = a1.v + (a1.v - a2.v);
                nodes[0].Add(node);
                nodes[1] = [.. nodes[1], node, a1.v];
                while (IsInBounds(node)) nodes[1].Add(node += a1.v - a2.v);
            }));

        bool IsInBounds(Vector2 pos)=> pos.X >= 0 && pos.X < lines[0].Length 
            && pos.Y >= 0 && pos.Y < lines.Length;

        totals[0] = nodes[0].Distinct().Where(IsInBounds).Count();
        totals[1] = nodes[1].Distinct().Where(IsInBounds).Count();
    }
}