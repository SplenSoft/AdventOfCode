using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        Vector2 ToEnd(Vector2 pos) => end - pos;
        long totalMoves = 0;
        List<Vector2> allTiles = [];
        List<(int, Action)> actions = [];
        Dictionary<Vector2, List<List<Vector2>>> pathsFromTileToEnd = [];
        

        for (int y = 0; y < lines.Length; y++) 
        { 
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#') walls.Add(new Vector2(x, y));
                if (lines[y][x] == 'E') end = new Vector2(x, y);
                if (lines[y][x] == 'S') start = new Vector2(x, y);
            }
        }

        IEnumerable<Vector2> GetSortedDirs(Vector2 pos) 
        {
            Vector2 toEnd = end - pos;
            return dirs.OrderBy(x => Angle(toEnd, x));
            //return dirs;
        }

        double Angle(Vector2 v, Vector2 u) => Math.Acos(Vector2.Dot(v, u) 
            / (v.Length() * u.Length()));

        void Path(Vector2 pos, List<Vector2> path, long score, Vector2 dir)
        {
            if (score > lowestScore) return;
            totalMoves++;
            foreach (var direction in GetSortedDirs(pos))
            {
                Vector2 next = pos + direction;
                if (!walls.Contains(next) && !path.Contains(next))
                {
                    List<Vector2> newPath = [.. path, next];
                    long newScore = score + 1;
                    newScore += GetScoreAndDir(pos, next, dir, out var nextDir);
                    if (next == end)
                    {
                        if (newScore < lowestScore)
                        {
                            bestPath = newPath;
                            lowestScore = newScore;
                        }
                        continue;
                    }
                    actions.Add((newPath.Count, () => Path(next, newPath, newScore, nextDir)));
                    //Path(next, newPath, newScore, nextDir);
                }
            }
        }

        Path(start, [start], 0, new(1, 0));

        while (actions.Count != 0)
        {
            var newActions = actions.OrderBy(x => x.Item1);
            var action = newActions.Take(1);
            actions = newActions.Skip(1).ToList();
            action.ElementAt(0).Item2.Invoke();
        }

        long GetScoreAndDir(Vector2 current, Vector2 next, Vector2 dir, out Vector2 nextDir)
        {
            long score = 0;
            Vector2 pathDir = next - current;
            Vector2 combo = pathDir + dir;
            if (combo == Vector2.Zero)
            {
                score += 2000;
            }
            else if (combo.X != 0 && combo.Y != 0)
            {
                score += 1000;
            }
            nextDir = pathDir;
            return score;
        }

        totals[0] = lowestScore;
        Console.WriteLine($"Total moves: {totalMoves}");
    }
}
