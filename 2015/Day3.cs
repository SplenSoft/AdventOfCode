using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    //62
    public override string Solve(string input)
    {
        Dictionary<int, List<int>> houses;
        (int, int)[] santas = new (int, int)[2];
        string result = "";
        Dictionary<char, (int, int)> moves = new()
        {
            { '^', (0, 1) }, { 'v', (0, -1) }, 
            { '<', (-1, 0) }, { '>', (1, 0) }
        };

        void Move(ref (int, int) pos, char ch)
        {
            pos.Item1 += moves[ch].Item1;
            pos.Item2 += moves[ch].Item2;
            if (!houses.ContainsKey(pos.Item1)) houses[pos.Item1] = [];
            houses[pos.Item1].Add(pos.Item2);
        }

        for (int j = 0; j < 2; j++)
        {
            santas = new (int, int)[2];
            houses = [];

            for (int i = 0; i < input.Length; i++)
                if (i % 2 != 0 || j == 1) Move(ref santas[0], input[i]);
                else Move(ref santas[1], input[i]);

            int res = houses
                .SelectMany(x => houses[x.Key].Distinct()).Count() + j;

            result = $"Part {(j == 0 ? 2 : 1)} solution: {res}\n" + result;
        }

        return result;
    }
}