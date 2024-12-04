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

    public override string Solve(string input)
    {
        var houses = new Dictionary<int, Dictionary<int, int>>();
        (int, int) posSanta = (0, 0);
        (int, int) posRoboSanta = (0, 0);
        int totalHitHouses = 0;

        void AddPresent((int, int) santaPos)
        {
            int x = santaPos.Item1;
            int y = santaPos.Item2;

            if (!houses.TryGetValue(x, out Dictionary<int, int>? value))
            {
                value = ([]);
                houses[x] = value;
            }

            if (!value.ContainsKey(y)) 
            {
                value[y] = 0;
                totalHitHouses++;
            }

            houses[x][y] += 1;
        }

        void Move(ref (int, int) santaPos, char ch)
        {
            switch (ch)
            {
                case '^':
                    santaPos.Item2 += 1;
                    break;
                case 'v':
                    santaPos.Item2 -= 1;
                    break;
                case '<':
                    santaPos.Item1 -= 1;
                    break;
                case '>':
                    santaPos.Item1 += 1;
                    break;
            }

            AddPresent(santaPos);
        }

        AddPresent(posSanta);
        AddPresent(posRoboSanta);

        var sb = new StringBuilder();

        for (int j = 0; j < 2; j++)
        {
            totalHitHouses = 0;
            posSanta = (0, 0);
            posRoboSanta = (0, 0);
            houses = [];

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                if (i % 2 == 0 || j == 0) // Normal Santa
                {
                    Move(ref posSanta, ch);
                }
                else
                {
                    Move(ref posRoboSanta, ch);
                }
            }

            sb.AppendLine($"Part {j + 1} solution: {totalHitHouses}");
        }
        
        return sb.ToString();
    }
}