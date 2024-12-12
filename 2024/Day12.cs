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

        void Path(List<Vector2> plotsSoFar, Vector2 pos, char ch, Dictionary<Vector2, List<Vector2>> edges)
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
                        edges[pos].Add(new Vector2(x, y));
                        continue;
                    }
                    if (plotsSoFar.Contains(next)) continue;
                    if (ch != lines[(int)next.Y][(int)next.X])
                    {
                        edges[pos].Add(new Vector2(x, y));
                        continue;
                    }
                    Path(plotsSoFar, next, ch, edges);
                }
        }

        long GetCorners(Dictionary<Vector2, List<Vector2>> edges)
        {
            long outsideCorners = 0;
            
            long insideTopRight = 0;
            long insideBottomRight = 0;
            long insideTopLeft = 0;
            long insideBottomLeft = 0;
            List<Vector2> usedInsideTopRight = [];
            List<Vector2> usedInsideTopLeft = [];
            List<Vector2> usedInsideBottomRight = [];
            List<Vector2> usedInsideBottomLeft = [];

            foreach (var item in edges)
            {
                // Outside corners
                bool rightEdge = item.Value.Contains(new Vector2(1, 0));
                bool leftEdge = item.Value.Contains(new Vector2(-1, 0));
                bool topEdge = item.Value.Contains(new Vector2(0, -1));
                bool bottomEdge = item.Value.Contains(new Vector2(0, 1));

                bool topRight = topEdge && rightEdge;
                bool topLeft = topEdge && leftEdge;
                bool bottomRight = bottomEdge && rightEdge;
                bool bottomLeft = bottomEdge && leftEdge;

                outsideCorners += new List<bool>() { topRight, topLeft, bottomRight, bottomLeft }.Where(x => x).Count();

                Vector2 other = new Vector2(item.Key.X + 1, item.Key.Y + 1);
                if (bottomEdge && !rightEdge && !usedInsideTopRight.Contains(item.Key) 
                    && edges.TryGetValue(other, out var value) && value.Contains(new Vector2(-1, 0)))
                { // Are we a Top square that needs a bottom right?
                    usedInsideTopRight.Add(item.Key);
                    insideTopRight++;
                }

                
                other = new Vector2(item.Key.X - 1, item.Key.Y + 1);
                if (bottomEdge && !leftEdge && !usedInsideTopLeft.Contains(item.Key) 
                    && edges.TryGetValue(other, out value) && value.Contains(new Vector2(1, 0)))
                { // Are we a Top square that needs a bottom left?
                    usedInsideTopLeft.Add(item.Key);
                    insideTopLeft++;
                }

                other = new Vector2(item.Key.X - 1, item.Key.Y - 1);
                if (topEdge && !leftEdge && !usedInsideBottomLeft.Contains(item.Key) 
                    && edges.TryGetValue(other, out value) && value.Contains(new Vector2(1, 0)))
                { // Are we a Bottom square that needs a top left?
                    usedInsideBottomLeft.Add(item.Key);
                    insideBottomLeft++;
                }

                other = new Vector2(item.Key.X + 1, item.Key.Y - 1);
                if (topEdge && !rightEdge && !usedInsideBottomRight.Contains(item.Key) 
                    && edges.TryGetValue(other, out value) && value.Contains(new Vector2(-1, 0)))
                { // Are we a Bottom square that needs a top right?
                    usedInsideBottomRight.Add(item.Key);
                    insideBottomRight++;
                }
            }

            return outsideCorners + insideBottomRight + insideBottomLeft + insideTopLeft + insideTopRight;
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
                    char ch = lines[y][x];
                    Path(used, tile, ch, edges);
                    usedPlots.AddRange(used);
                    long perim = edges.Values.Select(e => e.Count).Sum();
                    totals[0] += (used.Count * perim);
                    var edges2 = edges.Keys.Where(x => edges[x].Count > 0).ToList();
                    long sides = GetCorners(edges);
                    totals[1] += (used.Count * sides);
                }
            }
        }
    }
}
