using System.Numerics;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/15"/>
/// </summary>
[Day(2024, 15)]
internal class Day15 : Day
{   
    public override async Task Solve(string input, dynamic[] totals)
    {
        int p = 0;
        string[] lines = input.Split(Environment.NewLine);
        Dictionary<char, Vector2> dirs = new() { {'^', new(0, -1) },  
            {'>', new(1, 0) }, {'v', new(0, 1) },  {'<', new(-1,0) }};
        Start:
        Vector2 bot = Vector2.Zero;
        Dictionary<List<Vector2>, char> obs = []; // Boxes and walls
        int y = 0; // Separated so we can identify/skip the empty line

        for (; y < lines.Length; y++)
        {
            if (string.IsNullOrEmpty(lines[y])) break;
            string line = p == 1 ? "" : lines[y];
            for (int x = 0; x < lines[y].Length; x++) // Part 2 only
                if (p > 0 && lines[y][x] == '#') line += "##";
                else if (p > 0 && lines[y][x] == 'O') line += "[]";
                else if (p > 0 && lines[y][x] == '.') line += "..";
                else if (p > 0 && lines[y][x] == '@') line += "@.";

            for (int x = 0; x < line.Length; x++)
                if (line[x] == '[') obs[[new(x, y), new(x + 1, y)]] = 'O';
                else if (line[x] == '@') bot = new(x, y);
                else if (line[x] == 'O') obs[[new(x, y)]] = 'O'; // Part 1
                else if (line[x] == '#') obs[[new(x, y)]] = '#';
        }

        bool GetObj(Vector2 tile, out List<Vector2>? key)
        {
            key = obs.Keys.FirstOrDefault(x => x.Contains(tile));
            return key != null;
        }

        bool Move(Vector2 dir, List<Vector2> obj, List<List<Vector2>> all)
        {
            all.Add(obj);
            foreach (var item in obj)
                if (GetObj(item + dir, out List<Vector2>? objs))
                    if (all.Contains(objs)) continue;
                    else if (obs[objs] == '#') return false;
                    else if (!Move(dir, objs, all)) return false;
            return true;
        }

        for (++y; y < lines.Length; y++) // Iterate robot directions
            foreach (var ch in lines[y])
            {
                List<List<Vector2>> all = [];
                if (!GetObj(bot + dirs[ch], out List<Vector2>? objs) 
                    || (obs[objs] != '#' && Move(dirs[ch], objs, all)))
                {   
                    bot += dirs[ch]; // Move the robot
                    obs = obs.Select(x => all.Contains(x.Key) ? KeyValuePair
                        .Create(x.Key.Select(y => y + dirs[ch])
                        .ToList(), x.Value) : x).ToDictionary(); // Move boxes
                }
            }

        totals[p++] = (long)obs.Where(x => x.Value == 'O') // Weird scoring
            .Select(x => x.Key[0].X + (100 * x.Key[0].Y)).Sum();
        if (p == 2) return; else goto Start; // Go do part 2 or end
    }
}