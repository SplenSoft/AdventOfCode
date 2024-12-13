using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/13"/>
/// </summary>
[Day(2024, 13)]
internal class Day13 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        string[] lines = input.Split(Environment.NewLine)
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var regexButton = new Regex(@"Button (?:A|B): X\+(\d+), Y\+(\d+)");

        Vector2 Match(Regex regex, string line)
        {
            var match = regex.Match(line).Groups;
            return new (int.Parse(match[1].Value), int.Parse(match[2].Value));
        }

        for (long j = 0; j <= 10000000000000L; j += 10000000000000)
        {
            for (int i = 0; i < lines.Length; i += 3)
            {
                Vector2 a = Match(regexButton, lines[i]);
                Vector2 b = Match(regexButton, lines[i + 1]);
                Vector2 p = Match(new (@"Prize: X=(\d+), Y=(\d+)"), lines[i + 2]);
                Console.WriteLine($"\nStarting machine {i + 1} ...");
                Console.WriteLine($"A: X = {a.X}, Y = {a.Y}");
                Console.WriteLine($"B: X = {b.X}, Y = {b.Y}");
                Console.WriteLine($"P: X = {p.X}, Y = {p.Y}");
                List<List<Vector2>> validCounts = [[], []];

                for (int x = 0; x < (j == 0 ? 101 : j); x++)
                {
                    for (int y = 0; y < (j == 0 ? 101 : j); y++)
                    {
                        Vector2 a1 = new Vector2(a.X * x, a.Y * x);
                        Vector2 b1 = new Vector2(b.X * y, b.Y * y);
                        Vector2 solution = a1 + b1;
                        if (solution.Length() > p.X + j + p.Y + j)
                        {
                            break;
                        }

                        if (solution.X == p.X + j && solution.Y == p.Y + j)
                        {
                            validCounts[j == 0 ? 0 : 1].Add(new Vector2(x, y));
                        }
                    }
                    Console.Write($"\rProcessed: {(x + 1) * j} - Matches: {validCounts[j == 0 ? 0 : 1].Count}                    ");
                }

                Console.WriteLine("");

                if (validCounts[j == 0 ? 0 : 1].Count == 0)
                {
                    Console.WriteLine("No combination possible!");
                    continue;
                }
                long lowest = (long)validCounts[j == 0 ? 0 : 1].Select(x => x.X * 3 + x.Y).Order().First();
                Console.WriteLine($"Lowest cost = {lowest}");
                totals[j == 0 ? 0 : 1] += lowest;
                
            }
            Console.WriteLine($"Part 1 solution = {totals[0]}");
            Console.WriteLine($"Press enter key to continue");
            Console.ReadLine();
        }
    }
}
