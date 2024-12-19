using System.Numerics;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/16"/>
/// <para /><see href="https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm"/>
/// </summary>
[Day(2024, 16)]
internal class Day16 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);
        totals[0] = long.MaxValue; // Best path must be a lower score than this
        List<Vector2> dirs = [new(0, 1), new(0, -1), new(1, 0), new(-1, 0)];
        List<(long s, HashSet<Vector2> v)> best = []; // For part 2
        Stack<Action> actions1 = []; // Avoids stack overflow
        Dictionary<char, HashSet<Vector2>> tiles = new() { { '#', [] }, };
        Dictionary<Vector2, long> scoreAtTile = []; // Dijkstra's algorithm

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                if (lines[y][x] is '#') tiles[lines[y][x]].Add(new(x, y));
                else tiles[lines[y][x]] = [new(x, y)];

        void Path(List<Vector2> path, Vector2 dir, long score, int i, Vector2 dest)
        {   // Recurse using Dijkstra's algorithm
            if (score > totals[0]) return;
            foreach (var direction in dirs.Skip(i).Concat(dirs.Take(i)))
            {   
                Vector2 next = path.Last() + direction;
                Vector2 newDir = next - path.Last();
                Vector2 combo = newDir + dir;

                long score2 = score + (combo == Vector2.Zero ? 2001 
                    : (combo.X != 0 && combo.Y != 0) ? 1001 : 1);

                if (score2 > totals[0] || tiles['#'].Contains(next)
                    || (scoreAtTile.TryGetValue(next, out long tileScore) 
                    && score2 > tileScore))
                    continue;

                scoreAtTile[next] = score2;

                if (next == dest)
                {
                    totals[0] = Math.Min(totals[0], score2);
                    best.Add((score2, [.. path, next]));
                    continue;
                }

                actions1.Push(() => 
                    Path([.. path, next], newDir, score2, i, dest));
            }
        }

        void DoPath(List<Vector2> path, Vector2 dir, long score, Vector2 dest)
        {
            for (int y = 0; y < 4; y++) // Try all different first directions
            {
                scoreAtTile.Clear(); // Reset our stored Dijkstra values
                Path(path, dir, score, y, dest);
                while (actions1.Count > 0) actions1.Pop().Invoke();
            }
        }

        DoPath([.. tiles['S']], new Vector2(1, 0), 0, tiles['E'].First());
        DoPath([.. tiles['E']], new Vector2(1, 0), 0, tiles['S'].First());
        totals[1] = best.Where(x => x.s == totals[0]).SelectMany(x => x.v)
            .Distinct().Count(); // Part 2
    }
}
