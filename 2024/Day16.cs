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
        List<Action> actions0 = [];
        List<Action> actions1 = [];
        Stack<Action> actions2 = [];
        Stack<Action> actions3 = [];
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

        void Path(Vector2 pos, List<Vector2> path, Vector2 dir, long score, bool useAllTiles)
        {
            totalMoves++;
            //if (totalMoves % 100000 == 0) Draw();
            if (score > lowestScore)
            {
                totalKilled++;
                return;
            }

            //foreach (var direction in GetSortedDirs(pos))
            foreach (var direction in dirs)
            {
                Vector2 next = pos + direction;
                if (!walls.Contains(next) && !path.Contains(next) && !allTiles.Contains(next))
                {
                    //if (useAllTiles && allTiles.Contains(next)) continue;
                    List<Vector2> newPath = [.. path, next];

                    long newScore = GetScoreBetweenTiles(path.Last(), next, dir, out var newDir) + 1;

                    if (newScore + score > lowestScore)
                    {
                        //Console.WriteLine("killed!");
                        totalKilled++;
                        continue;
                    }

                    if (next == end)
                    {
                        if (newScore + score < lowestScore)
                        {
                            lowestScore = newScore + score;
                            Console.Write($"\rLowest = {lowestScore}. Killed = {totalKilled}");
                        }
                        allTiles.Clear();
                        continue;
                    }

                    Action action = () => Path(next, newPath, newDir, newScore + score, useAllTiles);

                    //if (allTiles.Add(next))
                    //{
                    //    //action.Invoke();
                    //    actions1.Insert(0,action);
                    //}
                    /*else */if (newScore < 1000)
                    {
                        //actions1.Add(action);
                        actions1.Insert(0, action);
                    }
                    else if (newScore < 2000)
                    {
                        actions2.Push(action);
                    }
                    else
                    {
                        actions3.Push(action);
                    }

                    //Path(next, newPath, newDir, newScore + score, useAllTiles);
                }
            }
            allTiles.Add(pos);
        }

        void Draw()
        {
            Console.Clear();
            for (int y = 0; y < lines.Length; y++)
            {
                string line = "";
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (walls.Contains(new(x, y))) line += "#";
                    else if (start == new Vector2(x, y)) line += "S";
                    else if (end == new Vector2(x, y)) line += "E";
                    else if (allTiles.Contains(new(x, y))) line += "X";
                    else line += " ";
                }
                Console.WriteLine(line);
            }
            Console.ReadLine();
        }

        Path(start, [start], new Vector2(1, 0), 0, true);

        while (actions1.Count > 0 ||  actions2.Count > 0 || actions3.Count > 0)
        {
            while (actions1.Count > 0)
            {
                var todo = actions1.First();
                actions1.RemoveAt(0);
                todo.Invoke();
            }

            while (actions2.Count > 0)
            {
                var todo = actions2.Pop();
                todo.Invoke();
                if (actions1.Count > 0) break;
            }

            while (actions3.Count > 0)
            {
                var todo = actions3.Pop();
                todo.Invoke();
                if (actions1.Count > 0 || actions2.Count > 0) break;
            }
        }

        //Console.WriteLine($"First pathing finished in {Time.Elapsed} with lowest = {lowestScore}");
        //Path(start, [start], new Vector2(1, 0), 0, false);

        //while (actions1.Count > 0)
        //{
        //    var sorted = actions1.Where(x => x.Item1 < lowestScore).OrderBy(x => x.Item1).ToList();
        //    if (sorted.Count == 0) break;
        //    var todo = sorted.First().Item2;
        //    actions1 = sorted.Skip(1).ToList();
        //    todo.Invoke();
        //}

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

        //totals[0] = finalPaths.Select(x => GetScore(x, new (1, 0))).Order().First();
        totals[0] = lowestScore;
        Console.WriteLine($"\nTotal moves: {totalMoves}");
    }
}
