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
            .Select(x => x.Select(y => int.Parse(y.ToString())).ToList())];
        var lists = lines.SelectMany((a, y) => a
            .Select((b, x) => (y, x, b))).Where(c => c.b == 0)
            .Select(x => Path([new(x.x, x.y)], [], new(x.x, x.y))).ToList();
        tot[0] = lists.Sum(x => x.Distinct().Count());
        tot[1] = lists.Sum(x => x.Length);

        Vector2[] Path(Vector2[] pathSoFar, List<Vector2> tops, Vector2 pos)
        {
            if (lines[(int)pos.Y][(int)pos.X] > 8) tops.Add(pos);
            else for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    {
                        Vector2 newPos = new(pos.X + x, pos.Y + y);
                        if ((x == 0 || y == 0) && newPos.X >= 0
                            && newPos.X < lines[0].Count
                            && newPos.Y < lines.Count && newPos.Y >= 0
                            && lines[(int)newPos.Y][(int)newPos.X]
                            - lines[(int)pos.Y][(int)pos.X] == 1
                            && !pathSoFar.Contains(newPos))
                                Path([.. pathSoFar, newPos], tops, newPos);
                    }
            return [..tops];
        }
    }
}