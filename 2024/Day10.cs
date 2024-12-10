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

    public override async Task Solve(string input, long[] tot)
    {
        string[] lines = input.Split(Environment.NewLine);

        int[,] map = new int[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                map[x, y] = int.Parse(lines[y][x].ToString());

        bool IsOffMap(Vector2 pos)
        {
            return pos.X < 0 || pos.Y < 0 || pos.X >= lines[0].Length || pos.Y >= lines.Length;
        }

        bool IsTopHeight(Vector2 pos)
        {
            return map[(int)pos.X, (int)pos.Y] == 9;
        }

        void Path(List<Vector2> pathSoFar)
        {
            Vector2 pos = pathSoFar.Last();
            
            if (IsTopHeight(pos))
            {
                tot[0]++;
                return;
            }
            int height = map[(int)pos.X, (int)pos.Y];
            for (int x = -1; x <= 1; x += 2)
                for (int y = -1; y <= 1; y += 2)
                {
                    Vector2 newPos = new(pos.X + x, pos.Y + y);
                    int newHeight = map[(int)newPos.X, (int)newPos.Y];
                    if (newHeight - height != 1) continue;
                    if (pathSoFar.Contains(newPos)) continue;
                    if (IsOffMap(newPos)) continue;
                    List<Vector2> newPath = [..pathSoFar, newPos];
                    Path(newPath);
                }
        }
    }
}
