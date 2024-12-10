using System.Numerics;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/10"/>
/// </summary>
[Day(2024, 10)]
internal class Day10 : Day
{
    public override string Input => Resources._2024_10_Input;
    //public override string Input => Resources._2024_10_Test_Input;
    //51
    public override async Task Solve(string input, long[] tot)
    {
        string[] lines = input.Split(Environment.NewLine);
        int[,] map = new int[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                map[x, y] = int.Parse(lines[y][x].ToString());

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                if (map[x, y] == 0)
                {
                    List<Vector2> tops = [];
                    Path([new Vector2(x, y)], tops);
                    tot[0] += tops.Distinct().Count();
                    tot[1] += tops.Count;
                }

        void Path(List<Vector2> pathSoFar, List<Vector2> tops)
        {
            Vector2 pos = pathSoFar.Last();
            
            if (map[(int)pos.X, (int)pos.Y] == 9) tops.Add(pos);
            else for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    {
                        Vector2 newPos = new(pos.X + x, pos.Y + y);
                        if ((x == 0 || y == 0) && newPos.X >= 0
                            && newPos.X < lines[0].Length
                            && newPos.Y < lines.Length && newPos.Y >= 0
                            && map[(int)newPos.X, (int)newPos.Y]
                            - map[(int)pos.X, (int)pos.Y] == 1
                            && !pathSoFar.Contains(newPos))
                                Path([.. pathSoFar, newPos], tops);
                    }
        }
    }
}
