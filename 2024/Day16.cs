using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/16"/>
/// </summary>
[Day(2024, 16)]
internal class Day16 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);
        long lowest = long.MaxValue;
        List<Vector2> dirs = [new(0, 1), new(0, -1), new(1, 0), new(-1, 0)];
        List<(long, List<Vector2>)> best = [];
        Stack<Action> actions1 = [];
        Dictionary<char, HashSet<Vector2>> tiles = new() { { '#', [] }, { '.', [] } };
        Dictionary<Vector2, long> scoreAtTile = [];

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                if (lines[y][x] is '#' or '.') 
                    tiles[lines[y][x]].Add(new(x, y));
                else tiles[lines[y][x]] = [new(x, y)];

        void Path(List<Vector2> path, Vector2 dir, long score, int i, Vector2 dest)
        {
            if (score > lowest) return;
            foreach (var direction in dirs.Skip(i).Concat(dirs.Take(i)))
            {
                Vector2 next = path.Last() + direction;
                Vector2 newDir = next - path.Last();
                Vector2 combo = newDir + dir;

                long score2 = score + (combo == Vector2.Zero ? 2001 
                    : (combo.X != 0 && combo.Y != 0) ? 1001 : 1);

                if (score2 > lowest || tiles['#'].Contains(next)
                    || (scoreAtTile.TryGetValue(next, out long tileScore) 
                    && score2 > tileScore))
                    continue;

                scoreAtTile[next] = score2;

                if (next == dest)
                {
                    lowest = Math.Min(lowest, score2);
                    best.Add((score2, [.. path, next]));
                    continue;
                }

                actions1.Push(() => 
                    Path([.. path, next], newDir, score2, i, dest));
            }
        }

        void DoPath(List<Vector2> soFar, Vector2 dir, long score, Vector2 ending)
        {
            for (int y = 0; y < 4; y++)
            {
                scoreAtTile.Clear();
                Path(soFar, dir, score, y, ending);
                while (actions1.Count > 0) actions1.Pop().Invoke();
            }
        }

        DoPath([.. tiles['S']], new Vector2(1, 0), 0, tiles['E'].First());
        DoPath([.. tiles['E']], new Vector2(1, 0), 0, tiles['S'].First());
        totals[0] = lowest;
        totals[1] = best.Where(x => x.Item1 == lowest)
            .SelectMany(x => x.Item2).Distinct().Count();
    }
}
