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
                map[x][y] = int.Parse(lines[y][x]);

        bool IsOffMap(Vector2 pos)
        {
            return pos.X < 0 || pos.Y < 0 || pos.X >= lines[0].Length || pos.Y >= lines.Length;
        }

        bool IsTopHeight(Vector2 pos)
        {
            
        }

        void Path(List<Vector2> pathSoFar)
        {
            Vector2 currentTile = pathSoFar.Last();
            if (IsOffMap(currentTile)) return;
            if (
            int currentHeight = 
            for (x
        }
    }
}
