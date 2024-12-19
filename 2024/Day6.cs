using System.Numerics;
using System.Text;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/6"/>
/// </summary>
[Day(2024, 6)]
internal class Day6 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        List<List<Vector2>> vecs = [[/*path*/], [/*obstacles*/], [/*count*/]]; 
        Dictionary<char, Vector2> dirs = new()
            {{'^', new (0, -1)}, {'>', new (1, 0)},
            {'v', new (0, 1)}, {'<', new (-1, 0)}};
        Start:
        List<Vector2> path = [];
        string[] lines = input.Split(Environment.NewLine);
        Vector2 pos = Vector2.Zero;
        char dir = '^';
        
        bool Patrol()
        {
            int i = 0;
            Patrol:
            if (++i > lines[0].Length * lines.Length * 2) return false; //loop
            path.Add(pos);
            Vector2 nxt = pos + dirs[dir];

            if (nxt.X == lines[0].Length || nxt.Y == lines.Length 
                || nxt.X < 0 || nxt.Y < 0)
                return true; // We're off the map, no loop

            if (lines[(int)nxt.Y][(int)nxt.X] == '#')
            {
                int n = dirs.Keys.ToList().IndexOf(dir) + 1;
                dir = dirs.Keys.ElementAt(n == dirs.Keys.Count ? 0 : n);
            }
            else pos = nxt;
            goto Patrol;
        }

        for (int y1 = 0; y1 < lines.Length; y1++) // Find start pos
            for (int x = 0; x < lines[y1].Length; x++)
                if (lines[y1][x] == dir) pos = new Vector2(x, y1);

        if (vecs[0].Count == 0 && Patrol()) // Solve part 1
        {
            vecs[0] = path;
            totals[0] = path.Distinct().Count();
            goto Start;
        }

        if (vecs[1].Count == vecs[0].Count) // We've tried all obstacles
        {
            totals[1] = vecs[2].Count;
            return;
        }

        var obs = vecs[0][vecs[1].Count]; // Part 2, adding obstacles
        bool exists = vecs[1].Contains(obs);
        vecs[1].Add(obs);
        int y = (int)obs.Y;
        if (lines[y][(int)obs.X] == '^') goto Start;
        lines[y] = lines[y][..(int)obs.X] + '#' 
            + lines[y][^(lines[y].Length - (int)obs.X - 1)..];
        if (!exists && !Patrol()) vecs[2].Add(Vector2.Zero);
        goto Start;
    }
}
