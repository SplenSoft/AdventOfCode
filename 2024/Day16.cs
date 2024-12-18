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
        Vector2 start = Vector2.Zero;
        Vector2 end = Vector2.Zero;
        long lowestScore = long.MaxValue;
        HashSet<Vector2> walls = [];
        List<Vector2> bestPath = [];
        List<Vector2> dirs = [new(0, 1), new(0, -1), new(1, 0), new(-1, 0)];
        List<(long, List<Vector2>)> bestPaths = [];
        Stack<Action> actions1 = [];
        Dictionary<Vector2, long> scoreAtTile = [];

        for (int y = 0; y < lines.Length; y++) 
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#') walls.Add(new Vector2(x, y));
                if (lines[y][x] == 'E') end = new Vector2(x, y);
                if (lines[y][x] == 'S') start = new Vector2(x, y);
            }

        void Path(List<Vector2> path, Vector2 dir, long score, int i, Vector2 dest)
        {
            if (score > lowestScore) return;

            //foreach (var direction in GetSortedDirs(pos))
            foreach (var direction in dirs.Skip(i).Concat(dirs.Take(i)))
            {
                Vector2 next = path.Last() + direction;
                Vector2 newDir = next - path.Last();
                Vector2 combo = newDir + dir;

                long newScore = combo == Vector2.Zero ? 2001 
                    : (combo.X != 0 && combo.Y != 0) ? 1001 : 1;

                if (newScore + score > lowestScore || walls.Contains(next)
                    || (scoreAtTile.TryGetValue(next, out long tileScore) 
                    && newScore + score > tileScore))
                    continue;

                scoreAtTile[next] = newScore + score;

                if (next == dest)
                {
                    lowestScore = Math.Min(lowestScore, newScore + score);
                    bestPaths.Add((newScore + score, [.. path, next]));
                    continue;
                }

                actions1.Push(() => Path([.. path, next], newDir, newScore + score, i, dest));
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

        DoPath([start], new Vector2(1, 0), 0, end);
        DoPath([end], new Vector2(1, 0), 0, start);
        totals[0] = lowestScore;
        totals[1] = bestPaths.Where(x => x.Item1 == lowestScore).SelectMany(x => x.Item2).Distinct().Count();
    }
}
