using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/11"/>
/// </summary>
[Day(2024, 11)]
internal class Day11 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        Stack<(long, int)> stones = [];

        //int max = 10000;
        //int maxStones = max * Environment.ProcessorCount;
        //List<(long, int)[]> retrievedStones = new(Environment.ProcessorCount);
        //List<Action> tasks = [];
        //int iterations = 0;
        //int tasksRunning = 0;
        //for (int a = 0; a < Environment.ProcessorCount; a++)
        //    retrievedStones.Add(new (long, int)[max]);

        int tasksRunning = 1;

        var task = Task.Run(async () =>
        {
            while (tasksRunning > 0)
            {
                await Task.Delay(1000);
                Console.WriteLine($"Stones count: {stones.Count}. Totals: {string.Join(", ", totals)} in {Time.Elapsed}");
            }
        });

        input.Split(' ').Select(x => (long.Parse(x), 0)).ToList().ForEach(x => 
        { 
            
        });

        async void Process((long, int) stoneItem)
        {
            while (tasksRunning >= 16) await Task.Yield();
            tasksRunning++;
            if (stoneItem.Item2 == 25)
            {
                totals[0]++;
                tasksRunning--;
                return;
            }
            else if (stoneItem.Item2 == 75)
            {
                totals[1]++;
                tasksRunning--;
                return;
            }

            long num = stoneItem.Item1;
            int i = stoneItem.Item2 + 1; // Blink count
            if (num == 0) Process((1, i));
            else if (Math.Floor(Math.Log10(num) + 1) % 2 == 0)
            { // Number of digits is even, split stone
                int digits = (int)Math.Floor(Math.Log10(num) + 1);
                long tens = (long)Math.Pow(10, digits / 2);
                long right = num % tens;
                long left = (num - right) / tens;
                Process((left, i));
                Process((right, i));
            }
            else Process((num * 2024, i));
        }

        tasksRunning--;
        //Parallel.Invoke([.. tasks]);
        //Console.WriteLine($"Completed parallel process {++iterations}. " +
        //    $"Stones count: {stones.Count}. Totals: {string.Join(", ",totals)} in {Time.Elapsed}");
    }
}
