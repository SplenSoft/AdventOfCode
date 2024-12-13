using System.Numerics;

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
                                if (edges.ContainsKey(n)) continue;
                                if (n.X < 0 || n.X >= lines[0].Length 
                                    || n.Y >= lines.Length || n.Y < 0 
                                    || ch != lines[(int)n.Y][(int)n.X])
                                {   // Next pos is off the map or not ch
                                    edges[pos].Add(new Vector2(x, y));
                                    continue;
                                }
                                Path(n, ch); // Move to next tile
                            }
                    }

                    char ch = lines[y][x];
                    Path(new Vector2(x, y), ch); // Map the garden
                    usedPlots.AddRange(edges.Keys); // No remapping
                    long perim = edges.Values.Select(e => e.Count).Sum();
                    totals[0] += edges.Keys.Count * perim; //Area * perim
                    long sides = 0;
                    List<List<Vector2>> usedCorners = [[], [], [], []];
                    foreach (var e in edges) // e = KVP[Vec2, List<Vec2>]
                    {   // Each corner counts as a unique side!
                        int cornerId = 0;
                        for (int x2 = -1; x2 <= 1; x2++)
                            for (int y2 = -1; y2 <= 1; y2++)
                            {   // Analyze four courners (inside and outside)
                                if (x2 == 0 || y2 == 0) continue; // diag only
                                var used = usedCorners[cornerId++];
                                bool edgeY = e.Value.Contains(new(0, y2));
                                bool edgeX = e.Value.Contains(new(x2, 0));
                                Vector2 pos2 = e.Key + new Vector2(x2, y2);
                                if (edgeY && !edgeX && !used.Contains(e.Key)
                                    && edges.TryGetValue(pos2, out var v)
                                    && v.Contains(new Vector2(x2 * -1, 0)))
                                {
                                    used.Add(e.Key);
                                    sides++;
                                }

                                if (e.Value.Contains(new(x2, 0))
                                    && e.Value.Contains(new(0, y2)))
                                        sides++;
                            }
                    }

                    totals[1] += edges.Keys.Count * sides; //Area * sides
                }
    }
}