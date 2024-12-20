using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/18"/>
/// </summary>
[Day(2024, 18)]
internal class Day18 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        int dims = Settings.IsTest ? 6 : 70;
        List<Vector2> bytes = [];
        Vector2 exit = new(dims, dims);
        string[] lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            var g = new Regex(@"(\d+),(\d+)").Match(line).Groups;
            bytes.Add(new(float.Parse(g[1].Value), float.Parse(g[2].Value)));
        }

        int max = Math.Min(bytes.Count, Settings.IsTest ? 12 : 1024);
        bytes = bytes.Index().Where(x => x.Index < max).Select(x => x.Item).ToList();

        List<HashSet<Vector2>> paths = [];

        void Path(Vector2 pos, HashSet<Vector2> path)
        {
            
        }
    }
}
