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
        Vector2 ToEnd(Vector2 pos) => end - pos;
        long totalMoves = 0;
        HashSet<Vector2> allTiles = [];
        List<Vector2> bestPathTiles = [];
        List<List<Vector2>> bestPaths = [];
        List<Action> actions1 = [];
        Dictionary<Vector2, long> scoreAtTile = new Dictionary<Vector2, long>();
        long totalKilled = 0;

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
        }

        double Angle(Vector2 v, Vector2 u) => Math.Acos(Vector2.Dot(v, u) 
            / (v.Length() * u.Length()));

        void Path(Vector2 pos, List<Vector2> path, Vector2 dir, long score, int shuffle, Vector2 ending)
        {
            totalMoves++;
            //if (totalMoves % 100000 == 0) Draw();
            if (score > lowestScore)
            {
                totalKilled++;
                return;
            }

            //foreach (var direction in GetSortedDirs(pos))
            foreach (var direction in dirs.Skip(shuffle).Concat(dirs.Take(shuffle)))
            {
                Vector2 next = pos + direction;
                if (!walls.Contains(next))
                {
                    //if (useAllTiles && allTiles.Contains(next)) continue;
                    List<Vector2> newPath = [.. path, next];

                    if (path.Last() == new Vector2(11, 3))
                    {
                        //Console.WriteLine(dir);
                    }

                    long newScore = GetScoreBetweenTiles(path.Last(), next, dir, out var newDir) + 1;

                    if (newScore + score > lowestScore)
                    {
                        //Console.WriteLine("killed!");
                        totalKilled++;
                        continue;
                    }

                    if (scoreAtTile.TryGetValue(next, out long tileScore))
                    {
                        if (newScore + score > tileScore) continue;
                    }

                    scoreAtTile[next] = newScore + score;

                    if (next == ending)
                    {
                        if (newScore + score <= lowestScore)
                        {
                            lowestScore = newScore + score;
                            Console.Write($"\rLowest = {lowestScore}. Killed = {totalKilled}");
                        }
                        bestPaths.Add(newPath);
                        continue;
                    }

                    Action action = () => Path(next, newPath, newDir, newScore + score, shuffle, ending);
                    //Path(next, newPath, newDir, newScore + score, shuffle, ending);
                    actions1.Add(action);
                }
            }
        }

        void DoPath(Vector2 pos, List<Vector2> soFar, Vector2 dir, long score, Vector2 ending)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < 4; y++)
                {
                    scoreAtTile.Clear();
                    Path(pos, soFar, dir, score, y, ending);

                    while (actions1.Count > 0)
                    {
                        var todo = actions1.First();
                        actions1.RemoveAt(0);
                        todo.Invoke();
                    }
                }
                dirs.Reverse();
            }

        }

        DoPath(start, [start], new Vector2(1, 0), 0, end);
        DoPath(end, [end], new Vector2(1, 0), 0, start);

        long GetScoreBetweenTiles(Vector2 current, Vector2 next, Vector2 dir, out Vector2 newDir)
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

            newDir = pathDir;

            return score;
        }

        long GetScore(List<Vector2> path, Vector2 dir)
        {
            long score = 0;
            score = path.Count - 1;
            for (int i = 0; i < path.Count - 1; i += 2)
            {
                Vector2 current = path[i];
                Vector2 next = path[i + 1];
                score += GetScoreBetweenTiles(current, next, dir, out dir);
            }
            
            return score;
        }

        totals[0] = lowestScore;
        totals[1] = bestPaths.Where(x => GetScore(x, new(1, 0)) == lowestScore).SelectMany(x => x).Distinct().Count();
        Console.WriteLine($"\nTotal moves: {totalMoves}");
    }
}
