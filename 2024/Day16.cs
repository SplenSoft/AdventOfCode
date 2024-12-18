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
        List<Vector2> allTiles = [];
        List<(int, Action)> actions = [];
        Dictionary<Vector2, List<List<Vector2>>> pathsFromTileToEnd = [];
        Dictionary<Vector2, Dictionary<Vector2, List<List<Vector2>>>> pathsFromTileToTile = [];
        List<List<Vector2>> unfinishedPaths = [];

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

        void Path(Vector2 pos, List<Vector2> path)
        {

            totalMoves++;
            if (totalMoves % 4 == 0) Draw();
            foreach (var direction in GetSortedDirs(pos))
            {
                Vector2 next = pos + direction;
                if (!walls.Contains(next) && !path.Contains(next))
                {
                    
                    List<Vector2> newPath = [.. path, next];
                    if (allTiles.Contains(next))
                    {
                        unfinishedPaths.Add(newPath);
                        GenerateShortcuts(newPath);  
                        continue;
                    }
                    if (next == end)
                    {
                        foreach (var tile in path)
                        {
                            if (!pathsFromTileToEnd.ContainsKey(tile))
                            {
                                pathsFromTileToEnd[tile] = [];
                            }

                            int index = path.IndexOf(tile);

                            pathsFromTileToEnd[tile].Add(newPath.Skip(index).ToList());
                        }
                        GenerateShortcuts(newPath);
                        continue;
                    }
                    allTiles.Add(next);
                    Path(next, newPath);
                }
            }
        }

        void GenerateShortcuts(List<Vector2> path)
        {
            foreach (var tile in path)
                foreach (var tile2 in path)
                {
                    if (tile == tile2) continue;
                    int index1 = path.IndexOf(tile);
                    int index2 = path.IndexOf(tile2);

                    if (index1 > index2) continue;

                    if (!pathsFromTileToTile.ContainsKey(tile))
                        pathsFromTileToTile[tile] = [];

                    if (!pathsFromTileToTile[tile].ContainsKey(tile2))
                        pathsFromTileToTile[tile][tile2] = [];

                    if (!pathsFromTileToTile.ContainsKey(tile2))
                        pathsFromTileToTile[tile2] = [];

                    if (!pathsFromTileToTile[tile2].ContainsKey(tile))
                        pathsFromTileToTile[tile2][tile] = [];

                    var snip = path.Skip(index1).Take((index2 - index1) + 1);

                    pathsFromTileToTile[tile][tile2].Add(snip.ToList());
                    pathsFromTileToTile[tile2][tile].Add(snip.Reverse().ToList());
                }
                    
        }

        void Draw()
        {
            //Console.Clear();
            //for (int y = 0; y < lines.Length; y++)
            //{
            //    string line = "";
            //    for (int x = 0; x < lines[y].Length; x++)
            //    {
            //        if (walls.Contains(new(x, y))) line += "#";
            //        else if (start == new Vector2(x, y)) line += "S";
            //        else if (end == new Vector2(x, y)) line += "E";
            //        else if (allTiles.Contains(new(x, y))) line += "X";
            //        else line += " ";
            //    }
            //    Console.WriteLine(line);
            //}
            //Console.ReadLine();
        }

        Path(start, [start]);

        //while (unfinishedPaths.Count > 0) 
        //{
        //    bool foundAtLeast1 = false;
        //    foreach (var unfinishedPath in unfinishedPaths)
        //    {
        //        if (pathsFromTileToEnd.ContainsKey(unfinishedPath.Last()))
        //        {
        //            foreach (var item1 in pathsFromTileToEnd[unfinishedPath.Last()])
        //            {
        //                foreach (var tile in unfinishedPath)
        //                {
        //                    int index = unfinishedPath.IndexOf(tile);

        //                    if (!pathsFromTileToEnd.ContainsKey(tile))
        //                    {
        //                        pathsFromTileToEnd[tile] = [];
        //                    }

        //                    pathsFromTileToEnd[tile].Add(unfinishedPath.Skip(index).Concat(item1).ToList());
        //                }
        //            }
        //            unfinishedPaths.Remove(unfinishedPath);
        //            foundAtLeast1 = true;   
        //            break;
        //        }
        //    }
        //    if (!foundAtLeast1) break;
        //}

        //foreach (var item in pathsFromTileToEnd.Keys)
        //{
        //    pathsFromTileToEnd[item] = pathsFromTileToEnd[item].OrderBy(GetScore).Take(1).ToList();
        //    //pathsFromTileToEnd[item] = pathsFromTileToEnd[item].Distinct().ToList();
        //}

        // Shorten paths
        List<List<Vector2>> finalPaths = [];
        long shortcutsFound = 0;
        long shortcutsExamined = 0;
        List<List<Vector2>> pathToUse = pathsFromTileToEnd[start].ToList();
        Short:
        finalPaths.Clear();
        bool goAgain = false;
        foreach (var item in pathToUse)
        {
            List<Vector2> newPath = new List<Vector2>(item);
            Loop:
            for (int j = 0; j < newPath.Count - 1; j++)
            {
                var tile1 = newPath[j];
                if (!pathsFromTileToTile.ContainsKey(tile1)) continue;
                if (tile1 == end) continue;

                for (int i = newPath.Count - 1; i >= 0; i--)
                {
                    var tile2 = newPath[i];
                    if (tile1 == tile2) continue;
                    if (!pathsFromTileToTile[tile1].ContainsKey(tile2)) continue;
                    shortcutsExamined++;
                    // Does a shortcut exist?
                    int index1 = j;
                    int index2 = i;
                    if (index1 > index2) continue;

                    var currentPath = newPath.Skip(index1).Take((index2 - index1) + 1);
                    Vector2 dir = j == 0 ? new Vector2(1, 0) : newPath[j - 1] - tile1;
                    long score = GetScore(currentPath.ToList(), dir);

                    var shortcut = pathsFromTileToTile[tile1][tile2]
                        .OrderBy(x => GetScore(x, dir)).First();

                    if (GetScore(shortcut, dir) < score)
                    {
                        newPath = newPath.Take(j).Concat(shortcut)
                            .Concat(newPath.Skip(i + 1)).ToList();

                        shortcutsFound++;
                        goAgain = true;
                        goto Loop;
                    }
                }
            }

            finalPaths.Add(newPath);
        }

        if (goAgain)
        {
            pathToUse = finalPaths.ToList();
            goto Short;
        }

        long GetScore(List<Vector2> path, Vector2 dir)
        {
            long score = 0;
            score = path.Count - 1;
            for (int i = 0; i < path.Count - 1; i += 2)
            {
                Vector2 current = path[i];
                Vector2 next = path[i + 1];
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

                dir = pathDir;
            }
            
            return score;
        }

        totals[0] = finalPaths.Select(x => GetScore(x, new (1, 0))).Order().First();
        Console.WriteLine($"Total moves: {totalMoves}");
        Console.WriteLine($"Total shortcuts: {shortcutsFound} / {shortcutsExamined}");
    }
}
