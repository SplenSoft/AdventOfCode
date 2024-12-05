using System.Numerics;

namespace AdventOfCode._2015;

/// <summary>
/// <see href="https://adventofcode.com/2015/day/3"/>
/// </summary>
internal class Day3 : Day
{
    public override int Year => 2015;

    public override int DayNumber => 3;

    public override string Synopsis => @"";

    public override string Input => Resources._2015_3_Input; 

    public override string Solve(string input)
    {
        Dictionary<float, List<float>> houses;
        Vector2[] santas = new Vector2[2];
        string result = "";
        Dictionary<char, Vector2> moves = new()
            {{'^', new (0, 1)}, {'v', new (0, -1)},
            {'<', new (-1, 0)}, {'>', new (1, 0)}};

        void Move(ref Vector2 pos, char ch)
        {
            pos += moves[ch];
            if (!houses.ContainsKey(pos.X)) houses[pos.X] = [];
            houses[pos.X].Add(pos.Y);
        }

        for (int j = 0; j < 2; j++)
        {
            santas = new Vector2[2];
            houses = [];

            for (int i = 0; i < input.Length; i++)
                 Move(ref santas[j == 1 ? 0 : i % 2], input[i]);

            int t = houses.SelectMany(x => houses[x.Key].Distinct()).Count();
            result = $"Part {(j == 0 ? 2 : 1)} solution: {t + j}\n" + result;
        }

        return result;
    }
}