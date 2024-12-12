using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/12"/>
/// </summary>
[Day(2024, 12)]
internal class Day12 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);

        List<Vector2> usedPlots = [];

        bool IsOnMap(Vector2 pos) => pos.X >= 0 && pos.Y >= 0 
            && pos.X < lines[0].Length && pos.Y < lines.Length;

        void Path(List<Vector2> plotsSoFar, Vector2 pos, char ch, ref long perim, Dictionary<Vector2, List<Vector2>> edges)
        {
            plotsSoFar.Add(pos);
            edges.Add(pos, []);
            // Move to next
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 && y != 0) continue; // No diagonals
                    Vector2 next = new Vector2(pos.X + x, pos.Y + y);
                    if (!IsOnMap(next))
                    {
                        perim++;
                        edges[pos].Add(new Vector2(x, y));
                        continue;
                    }
                    if (plotsSoFar.Contains(next)) continue;
                    if (ch != lines[(int)next.Y][(int)next.X])
                    {
                        perim++;
                        edges[pos].Add(new Vector2(x, y));
                        continue;
                    }
                    Path(plotsSoFar, next, ch, ref perim, edges);
                }
        }

        for (int y = 0; y < lines.Length; y++) 
        { 
            for (int x = 0; x < lines[y].Length; x++)
            {
                Vector2 tile = new Vector2(x, y);
                if (!usedPlots.Contains(tile))
                {
                    Dictionary<Vector2, List<Vector2>> edges = [];
                    List<Vector2> used = [];
                    long perim = 0;
                    char ch = lines[y][x];
                    Path(used, tile, ch, ref perim, edges);
                    usedPlots.AddRange(used);
                    Console.WriteLine($"{ch}: A = {used.Count}, P = {perim}");
                    totals[0] += (used.Count * perim);
                }
            }
        }
    }
}
