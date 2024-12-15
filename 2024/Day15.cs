using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/15"/>
/// </summary>
[Day(2024, 15)]
internal class Day15 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        int p = 0;
        string[] lines = input.Split(Environment.NewLine);
        Dictionary<char, Vector2> directions = new()
        {
            {'^', new(0, -1) },  {'>', new(1, 0) },
            {'v', new(0, 1) },  {'<', new(-1,0) }
        };
        Start:
        Vector2 robot = Vector2.Zero;
        Dictionary<Vector2, char> objects = [];
        
        // Parse map
        int y = 0;
        for (; y < lines.Length; y++)
        {
            if (string.IsNullOrEmpty(lines[y])) break;
            string line = p == 1 ? "" : lines[y];
            if (p == 1) // Part 2
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    char c = lines[y][x];
                    if (c == '#') line += "##";
                    if (c == 'O') line += "[]";
                    if (c == '.') line += "..";
                    if (c == '@') line += "@.";
                }
            }
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '.') continue;
                if (line[x] == '@')
                {
                    robot = new(x, y);
                    continue;
                }
                objects[new Vector2(x, y)] = line[x];
            }
        }

        // ParseInstructions
        for (++y; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                //Console.WriteLine($"Parsing robot direction {line[x]}");
                var dir = directions[line[x]];
                var newTile = robot + dir;

                if (!objects.ContainsKey(newTile))
                {
                    robot = newTile;
                    //Console.WriteLine("Moving robot");
                    continue;
                }

                var obj = objects[newTile];

                if (obj == '#')
                {
                    //Console.WriteLine("Hit wall");
                    continue;
                }

                // Newtile is O
                var nextObj = newTile;
                List<Vector2> boxes = [];

                while (true)
                {
                    boxes.Add(nextObj);
                    nextObj += dir;
                    if (!objects.ContainsKey(nextObj))
                    {
                        // Move all boxes over in that direction
                        robot = newTile;

                        foreach (var box in boxes)
                            objects.Remove(box);

                        boxes = boxes.Select(x => x + dir).ToList();

                        for (int i = 0; i < boxes.Count; i++)
                            objects[boxes[i]] = 'O';

                        //Console.WriteLine("Moving box(es)");

                        break;
                    }
                    else if (objects[nextObj] == '#') // wall
                    {
                        //Console.WriteLine("Hit wall");
                        break;
                    }
                }
            }
        }

        totals[p++] = (long)objects.Where(x => x.Value == 'O').Select(x => x.Key.X + (100 * x.Key.Y)).Sum();
        
        goto Start;
    }
}
