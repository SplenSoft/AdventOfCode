using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/18"/>
/// </summary>
[Day(2024, 18)]
internal class Day18 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        int dims = Settings.IsTest ? 6 : 70;
        HashSet<Vector2> bytes = [];
        Vector2 exit = new(dims, dims);
        string[] lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var g = new Regex(@"(\d+),(\d+)").Match(line).Groups;
            bytes.Add(new(float.Parse(g[1].Value), float.Parse(g[2].Value)));
        }

        int max = Math.Min(bytes.Count, Settings.IsTest ? 12 : 1024);
        bytes = bytes.Index().Where(x => x.Index < max).Select(x => x.Item).ToHashSet();
        List<Vector2> dirs = [new(0, 1), new(1, 0), new(-1, 0), new(0, -1)];
        List<HashSet<Vector2>> paths = [];
        Dictionary<Vector2, int> tileScore = [];
        double Angle(Vector2 v, Vector2 u) => Math.Acos(Vector2.Dot(v, u)
            / (v.Length() * u.Length()));

        IEnumerable<Vector2> GetDirs(Vector2 pos) => dirs.OrderBy(x => Math.Abs(Angle(exit - pos, x - pos)));

        long lowest = long.MaxValue;
        HashSet<Vector2> used = [];
        long running = 0;
        Path(new(0, 0), [new(0, 0)]);
        
        
        void Path(Vector2 pos, HashSet<Vector2> path)
        {
            running++;
            if (running % 1000000 == 0)
            {
                Draw();
            }
            used.Add(pos);
            if (pos == exit)
            {
                paths.Add(path);
                if (path.Count - 1 < lowest)
                {
                    lowest = path.Count - 1;
                    //Console.Write($"\rLowest: {lowest}                                           ");
                }
                return;
            }

            if (path.Count - 1 >= lowest) return;

            if (!tileScore.TryGetValue(pos, out var score))
                score = int.MaxValue;

            if (path.Count > score) return;
            tileScore[pos] = path.Count;

            foreach (var dir in GetDirs(pos))
            {
                Vector2 next = pos + dir;
                if (next.X < 0 || next.Y < 0 || next.X > dims || next.Y > dims) continue; // Stay on map
                if (bytes.Contains(next)) continue;
                if (path.Contains(next)) continue;
                Path(next, [.. path, next]);
            }
        }

        void Draw()
        {
            Console.Clear();
            for (int x = 0; x <= dims; x++)
            {
                string line = "";
                for (int y = 0; y <= dims; y++)
                {
                    if (bytes.Contains(new Vector2(x, y))) line += '#';
                    else if (used.Contains(new Vector2(x, y))) line += 'O';
                    else line += ' ';
                }
                Console.WriteLine(line);
            }
            Console.ReadLine();
        }

        totals[0] = lowest;
    }
}
