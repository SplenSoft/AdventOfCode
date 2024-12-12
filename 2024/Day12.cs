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

        for (int y = 0; y < lines.Length; y++) 
            for (int x = 0; x < lines[y].Length; x++)
                if (!usedPlots.Contains(new Vector2(x, y)))
                {
                    Dictionary<Vector2, List<Vector2>> edges = [];

                    void Path(Vector2 pos, char ch)
                    {   // Recursive pathfinding in garden plot made of ch
                        edges.Add(pos, []);
                        for (int x = -1; x <= 1; x++)
                            for (int y = -1; y <= 1; y++)
                            {   // Gather edge data and move to next tile
                                if (x != 0 && y != 0) continue; // No diagonal
                                Vector2 n = pos + new Vector2(x, y);
                                if (n.X < 0 || n.X >= lines[0].Length 
                                    || n.Y >= lines.Length || n.Y < 0)
                                {   // Next pos is off the map
                                    edges[pos].Add(new Vector2(x, y));
                                    continue;
                                }
                                if (edges.ContainsKey(n)) continue;
                                if (ch != lines[(int)n.Y][(int)n.X])
                                {   // Next pos is not same character
                                    edges[pos].Add(new Vector2(x, y));
                                    continue;
                                }
                                Path(n, ch); // Move to next tile
                            }
                    }

                    char ch = lines[y][x];
                    Path(new Vector2(x, y), ch);
                    usedPlots.AddRange(edges.Keys);
                    long perim = edges.Values.Select(e => e.Count).Sum();
                    totals[0] += edges.Keys.Count * perim; //Area * perim
                    long sides = 0;
                    List<List<Vector2>> usedCorners = [[], [], [], []];

                    foreach (var item in edges)
                    {   // Each corner counts as a unique side!
                        bool r = item.Value.Contains(new Vector2(1, 0));
                        bool l = item.Value.Contains(new Vector2(-1, 0));
                        bool t = item.Value.Contains(new Vector2(0, -1));
                        bool b = item.Value.Contains(new Vector2(0, 1));

                        void GetInsideCorner(bool edgeY, bool edgeX, 
                            List<Vector2> used, int x, int y)
                        {
                            Vector2 pos2 = item.Key + new Vector2(x, y);
                            if (edgeY && !edgeX && !used.Contains(item.Key)
                                && edges.TryGetValue(pos2, out var value)
                                && value.Contains(new Vector2(x * -1, 0)))
                            {
                                used.Add(item.Key);
                                sides++;
                            }
                        }
                        // Count corners to get all unique sides
                        sides += new bool[] {t && r, t && l, b && r, b && l}
                            .Where(x => x).Count(); // Outside corners
                        GetInsideCorner(b, r, usedCorners[0], 1, 1);
                        GetInsideCorner(b, l, usedCorners[1], -1, 1);
                        GetInsideCorner(t, l, usedCorners[2], -1, -1);
                        GetInsideCorner(t, r, usedCorners[3], 1, -1);
                    }

                    totals[1] += edges.Keys.Count * sides; //Area * sides
                }
    }
}