using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

internal class Day7 : Day
{
    public override int Year => 2024;

    public override int DayNumber => 7;

    public override string Synopsis => throw new NotImplementedException();

    public override string Input => Resources._2024_7_Input;

    public override string Solve(string input)
    {
        var regex = new Regex(@"(\d+?):\s(.+)");
        string[] lines = input.Split(Environment.NewLine);
        char[] possibleOps = ['+', '*'];
        long total = 0;

        void GetCombos(int max, string current, List<string> list)
        {
            if (current.Length == max)
            {
                if (!list.Contains(current)) list.Add(current);
                return;
            }
            
            for (int j = current.Length; j < max; j++)
                for (int i = 0; i < possibleOps.Length; i++)
                    GetCombos(max, current + possibleOps[i], list);
        }

        foreach (var line in lines) 
        {
            var grp = regex.Match(line).Groups;
            long res = long.Parse(grp[1].Value);
            int[] ints = grp[2].Value.Split(' ').Select(int.Parse).ToArray();
            var list = new List<string>();
            GetCombos(ints.Length - 1, "", list);
            foreach (var str in list)
            {
                long subtotal = ints[0];

                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    if (c == '+')
                    {
                        subtotal += ints[i + 1];
                    }
                    else
                    {
                        subtotal *= ints[i + 1];
                    }
                }

                if (subtotal == res)
                {
                    total += subtotal;
                    break;
                }
            }
        }

        return total.ToString();
    }
}
