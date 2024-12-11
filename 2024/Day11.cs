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
        ConcurrentStack<(long, int)> stones = [];
        stones.PushRange([..input.Split(' ').Select(x => (long.Parse(x), 0))]);
        int max = 10000;
        List<(long, int)[]> retrievedStones = new(Environment.ProcessorCount);
        List<Action> tasks = [];
        int iterations = 0;
        for (int a = 0; a < Environment.ProcessorCount; a++)
            retrievedStones.Add(new (long, int)[max]);
        Console.Error.WriteLine("test");
        while (!stones.IsEmpty)
        {
            for (int k = 0; k < Environment.ProcessorCount; k++)
            {
                int p = k;
                tasks.Add(() =>
                {
                    try
                    {
                        long tot1 = 0, tot2 = 0;
                        int amt = stones.TryPopRange(retrievedStones[p], 0, max);
                        for (int j = 0; j < amt; j++)
                        {
                            var stoneItem = retrievedStones[p][j];
                            if (stoneItem.Item2 == 25) tot1++;
                            else if (stoneItem.Item2 == 75)
                            {
                                tot2++;
                                continue;
                            }

                            long num = stoneItem.Item1;
                            int i = stoneItem.Item2 + 1; // Blink count
                            if (num == 0) stones.Push((1, i));
                            else if (Math.Floor(Math.Log10(num) + 1) % 2 == 0)
                            { // Number of digits is even, split stone
                                int digits = (int)Math.Floor(Math.Log10(num) + 1);
                                long tens = (long)Math.Pow(10, digits / 2);
                                long right = num % tens;
                                long left = (num - right) / tens;
                                stones.Push((left, i));
                                stones.Push((right, i));
                            }
                            else stones.Push((num * 2024, i));
                        }
                        Interlocked.Add(ref totals[0], tot1);
                        Interlocked.Add(ref totals[1], tot2);
                    }
                    catch (Exception ex) 
                    {
                        Console.Error.WriteLine(ex);
                        Console.Error.WriteLine(ex.Message);
                    }
                });
            }
            Parallel.Invoke([.. tasks]);
            Console.WriteLine($"Completed parallel process {++iterations}. " +
                $"Stones count: {stones.Count}. Totals: {string.Join(", ",totals)} in {Time.Elapsed}");
        }
    }
}
