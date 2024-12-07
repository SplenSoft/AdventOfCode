using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

internal class Day6 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 6;

    public override string Synopsis => throw new NotImplementedException();

    public override string Input => Resources._2024_6_Input;

    public override string Solve(string input)
    {
        StringBuilder result = new("Part 1 solution: ");
        List<Vector2> successPath = [];
        List<Vector2> attemptedObstacles = [];
        int loops = 0;
        Dictionary<char, Vector2> dirs = new()
            {{'^', new (0, -1)}, {'>', new (1, 0)}, {'v', new (0, 1)},
            {'<', new (-1, 0)}};
        Start:
        List<Vector2> path = [];
        string[] lines = input.Split(Environment.NewLine);
        Vector2 pos = Vector2.Zero;
        char dir = '^';
        
        void AddObstacle(Vector2 obs)
        {
            attemptedObstacles.Add(obs);
            int y = (int)obs.Y;
            if (lines[y][(int)obs.X] == '^') return;
            var e = lines[y][^(lines[y].Length - (int)obs.X - 1)..];
            lines[y] = lines[y][..(int)obs.X] + '#' + e;
        }

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

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                if (lines[y][x] == dir) pos = new Vector2(x, y);

        if (successPath.Count == 0)
        {
            Patrol();
            successPath = path;
            result.Append(path.Distinct().Count());
            goto Start;
        }

        if (attemptedObstacles.Count == successPath.Count)
            return result.ToString() + $"\nPart 2 solution: {loops}";

        var obs = successPath[attemptedObstacles.Count];
        bool exists = attemptedObstacles.Contains(obs);
        AddObstacle(obs);
        if (!exists && !Patrol()) loops++;
        goto Start;
    }
}
