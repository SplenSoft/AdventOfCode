using System.Numerics;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/10"/>
/// </summary>
[Day(2024, 10)]
internal class Day10 : Day
{
    public override string Input => Resources._2024_10_Input;

    public override async Task Solve(string input, long[] tot)
    {
        List<List<int>> lines = [..input.Split(Environment.NewLine)
            .Select(y => y.Select(x => int.Parse(x.ToString())).ToList())];
        var lists = lines.SelectMany((a, y) => a // Organize and recurse
            .Select((b, x) => (y, x, b))).Where(c => c.b == 0)
            .Select(x => Path([new(x.x, x.y)], [], new(x.x, x.y))).ToList();
        tot[0] = lists.Sum(x => x.Distinct().Count()); // Part 1 solution
        tot[1] = lists.Sum(x => x.Length); // Part 2 solution

        Vector2[] Path(Vector2[] pathSoFar, List<Vector2> tops, Vector2 pos)
        {   // If we reach the top, add a top to our list
            if (lines[(int)pos.Y][(int)pos.X] > 8) tops.Add(pos);
            else // otherwise, ...
                for (int x = -1; x <= 1; x++) 
                    for (int y = -1; y <= 1; y++)
                    {   // ... recursive pathfinding with lots of conditions
                        Vector2 pos2 = new(pos.X + x, pos.Y + y);
                        if ((x == 0 || y == 0) // No diagonals!
                            && pos2.X >= 0 && pos2.X < lines[0].Count
                            && pos2.Y < lines.Count && pos2.Y >= 0 // On map
                            && lines[(int)pos2.Y][(int)pos2.X]
                            - lines[(int)pos.Y][(int)pos.X] == 1 // 1-height
                            && !pathSoFar.Contains(pos2)) // No backtracking!
                            Path([.. pathSoFar, pos2], tops, pos2); // Next 
                    }
            return [..tops]; // No more valid paths, return the result
        }
    }
}