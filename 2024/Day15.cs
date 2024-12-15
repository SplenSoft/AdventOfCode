using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/15"/>
/// </summary>
[Day(2024, 15)]
internal class Day15 : Day
{   //89
    public override async Task Solve(string input, long[] totals)
    {
        int p = 0;
        string[] lines = input.Split(Environment.NewLine);
        Dictionary<char, Vector2> directions = new() { {'^', new(0, -1) },  
            {'>', new(1, 0) }, {'v', new(0, 1) },  {'<', new(-1,0) }};
        Start:
        Vector2 robot = Vector2.Zero;
        Dictionary<List<Vector2>, char> obs = [];
        int y = 0;

        for (; y < lines.Length; y++)
        {
            if (string.IsNullOrEmpty(lines[y])) break;
            string line = p == 1 ? "" : lines[y];
            if (p > 0) // Part 2
                for (int x = 0; x < lines[y].Length; x++)
                    if (lines[y][x] == '#') line += "##";
                    else if (lines[y][x] == 'O') line += "[]";
                    else if (lines[y][x] == '.') line += "..";
                    else if (lines[y][x] == '@') line += "@.";

            for (int x = 0; x < line.Length; x++)
                if (line[x] == '[') obs[[new(x, y), new(x + 1, y)]] = 'O';
                else if (line[x] == '@') robot = new(x, y);
                else if (line[x] == 'O') obs[[new(x, y)]] = 'O';
                else if (line[x] == '#') obs[[new(x, y)]] = '#';
        }

        bool TryGetOccupied(Vector2 tile, out List<Vector2>? key)
        {
            key = obs.Keys.FirstOrDefault(x => x.Contains(tile));
            return key != null;
        }

        bool TryMoveAllObjects(Vector2 dir, List<Vector2> obj, 
            List<List<Vector2>> all)
        {
            all.Add(obj);
            foreach (var item in obj)
                if (TryGetOccupied(item + dir, out var objs))
                    if (all.Contains(objs)) continue;
                    else if (obs[objs] == '#') return false;
                    else if (!TryMoveAllObjects(dir, objs, all)) return false;
            return true;
        }

        for (++y; y < lines.Length; y++) // Iterate robot directions
            for (int x = 0; x < lines[y].Length; x++)
            {
                var dir = directions[lines[y][x]];

                if (!TryGetOccupied(robot + dir, out List<Vector2>? key))
                {   // Nothing is in the way, move the robot
                    robot += dir;
                    continue;
                }

                if (obs[key] == '#') continue;
                List<List<Vector2>> allObjs = [];
                if (TryMoveAllObjects(dir, key, allObjs))
                {
                    robot += dir;
                    foreach (var item in allObjs) obs.Remove(item);
                    allObjs = [.. allObjs.Select(x => x
                    .Select(y => y + dir).ToList())];
                    foreach (var item in allObjs) obs[item] = 'O';
                }
            }

        totals[p++] = (long)obs.Where(x => x.Value == 'O')
            .Select(x => x.Key[0].X + (100 * x.Key[0].Y)).Sum();
        if (p == 2) return; else goto Start;
    }
}
