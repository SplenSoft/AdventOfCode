using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/14"/>
/// </summary>
[Day(2024, 14)]
internal class Day14 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        int w /*width of map*/ = 101, h /*height of map*/ = 103;
        var regex = new Regex(@"p=(\d+),(\d+)\sv=(-?\d+),(-?\d+)");
        List<(Vector2 p/*position*/, Vector2 v/*velocity*/)> bots = [];
        string[] lines = input.Split(Environment.NewLine);
        List<float> distances = []; // Used for part 2 only
        bool foundTree = false;

        for (int i = 0; i < lines.Length; i++)
        {   // Parse bot starts and velocity
            var g = regex.Match(lines[i]).Groups;
            bots.Add((new(float.Parse(g[1].Value), float.Parse(g[2].Value)), 
                new(float.Parse(g[3].Value), float.Parse(g[4].Value))));
        }
       
        for (int i = 0; i > -1; i++) 
        {   // Loop forever until we meet our conditions
            if (foundTree && totals[0] > 0) break; // Our conditions

            distances.Clear(); // Get average distance between bots (part 2)
            for (int j = 0; j < bots.Count; j++)
                for (int k = 0; k < bots.Count; k++)
                    if (j != k)
                        distances.Add(Vector2.Distance(bots[j].p, bots[k].p));

            if (!foundTree && distances.Average() < 32) // Solve part 2
            {// From testing, lower than 32 avg means they're in tree form
                totals[1] = i; // This is dumb and I hate it.
                foundTree = true;
            }

            for (int j = 0; j < bots.Count; j++) 
            {   // Patrol the bots (parts 1 and 2)
                var r = bots[j];
                Vector2 p = r.p;
                p = new(p.X + r.v.X, p.Y + r.v.Y);
                if (p.X < 0) p.X += w;
                else if (p.X >= w) p.X -= w;
                if (p.Y < 0) p.Y += h;
                else if (p.Y >= h) p.Y -= h;
                bots[j] = (p, r.v);
            }

            if (i != 99) continue;
            // At 100 seconds get the quadrant totals and mult them for part 1
            totals[0] = bots.Where(x => x.p.X < w / 2 && x.p.Y < h / 2).Count()
                * bots.Where(x => x.p.X < w / 2 && x.p.Y > h / 2).Count()
                * bots.Where(x => x.p.X > w / 2 && x.p.Y > h / 2).Count()
                * bots.Where(x => x.p.X > w / 2 && x.p.Y < h / 2).Count();
        }
    }
}