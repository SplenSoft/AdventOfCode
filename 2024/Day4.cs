﻿using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/4"/>
/// </summary>
[Day(2024, 4)]
internal class Day4 : Day
{
    /// <summary>
    /// Part 1: Flattens input into 4 lists of strings (horizontal, vertical, 
    /// diagonal left, diagonal right), then counts "XMAS" or "SAMX" 
    /// occurrences in each string in each list
    /// <para />
    /// Part 2: Handle X-Mas by validating 3x3 sections of the input grid
    /// </summary>
    public override async Task Solve(string input, dynamic[] totals)
    {
        int Count(string s, string v) => new Regex(v).Matches(s).Count;
        bool Valid(string s) => s is "SM" or "MS";
        string[] lines = input.Split(Environment.NewLine); // Horizontals
        List<List<string>> lists = [lines.Select(x => "").ToList(), [], []];

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
            {
                lists[0][x] += lines[y][x]; // Construct verticals

                // Handle X-Mas by validating 3x3 sections (solve part 2)
                if (y + 2 < lines.Length && x + 2 < lines[y].Length
                    && lines[y + 1][x + 1] == 'A'
                    && Valid($"{lines[y][x]}{lines[y + 2][x + 2]}")
                    && Valid($"{lines[y][x + 2]}{lines[y + 2][x]}"))
                    totals[1]++;

                if (y == 0 || x == 0 || x == lines[y].Length - 1)
                    for (int i = 0; i < 2; i++) // Construct diagonals
                    {
                        int a = x;
                        int b = y;
                        string newDiag = "";

                        while (b < lines.Length && a < lines[y].Length
                            && a >= 0)
                        {
                            newDiag += lines[b++][a];
                            a += i == 0 ? 1 : -1;
                        }

                        if (newDiag.Length > 3) lists[i + 1].Add(newDiag);
                    }
            }

        foreach (var line in lists.SelectMany(x => x).Concat(lines)) // Part 1
            totals[0] += Count(line, "XMAS") + Count(line, "SAMX");
    }
}