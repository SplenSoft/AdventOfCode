using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Day1 : Day
    {
        public override int DayNumber { get; } = 1;

        public override string Solve(string input)
        {
            var regex = new Regex(@"(\d{5})\s{3}(\d{5})(?:\n|\r\n|$)");
            List<int> list1 = [];
            List<int> list2 = [];

            foreach (Match match in regex.Matches(input))
            {
                list1.Add(int.Parse(match.Groups[1].Value));
                list2.Add(int.Parse(match.Groups[2].Value));
            }

            list1 = [.. list1.OrderBy(x => x)];
            list2 = [.. list2.OrderBy(x => x)];
            int sum = 0;

            for (int i = 0; i < list1.Count; i++)
                sum += Math.Abs(list1[i] - list2[i]);

            return sum.ToString();
        }
    }
}
