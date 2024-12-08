using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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
    //public override string Input => Resources._2024_7_Test_Input;

    private void WriteToFile(Dictionary<int, long> solutions)
    {
        List<string> list = [];
        foreach (var item in solutions) 
        {
            list.Add($"{item.Key}: {item.Value}");
        }
        File.WriteAllLines("solutions2.txt", list);
    }

    public override async Task<string> Solve(string input)
    {
        long total = 0;
        string[] lines = input.Split(Environment.NewLine);
        var regex = new Regex(@"(\d+?):\s(.+)");
        bool GetCombos(int max, ushort str, long res, int[] ints, int k)
        {
            if (k == max)
            {
                long amt = ints[0];

                for (int i = 0; i < max; i++)
                    amt = (str & (1 << (max - i - 1))) != 0 
                        ? amt + ints[i + 1] : amt * ints[i + 1];

                return amt == res;
            }

            for (int j = k; j < max; j++)
                if (GetCombos(max, (ushort)(str ^ (1 << j)), res, ints, k + 1) 
                    || GetCombos(max, str, res, ints, k + 1))
                    return true;

            return false;
        }
        Dictionary<int, long> solutions = [];
        //if (File.Exists("solutions2.txt"))
        //{
        //    string[] solutionsStrings = File.ReadAllLines("solutions2.txt");

        //    foreach (string line in solutionsStrings)
        //    {
        //        var grp = regex.Match(line).Groups;
        //        solutions[int.Parse(grp[1].Value)] = long.Parse(grp[2].Value);
        //    }
        //}
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        //for (int i = 0; i < lines.Length; i++)
        Parallel.For(0, lines.Length, i =>
        {
            //float percent = solutions.Keys.Count / (float)lines.Length * 100;
            //Console.WriteLine($"Line {i + 1} of {lines.Length} ({percent:0.0}%)");
            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
            //if (solutions.ContainsKey(i))
            //{
            //    Console.WriteLine("Skipped line " + (i + 1));
            //}
            //else
            //{
                string line = lines[i];
                Console.WriteLine(line);
                var grp = regex.Match(line).Groups;
                long res = long.Parse(grp[1].Value);
                var ints = grp[2].Value.Split(' ').Select(int.Parse).ToArray();
                bool success = GetCombos(ints.Length - 1, 0, res, ints, 0);
                lock (regex)
                {
                    if (success)
                    {
                        Interlocked.Add(ref total, res);
                        solutions[i] = res;

                    }
                    else
                    {
                        solutions[i] = 0;
                    }
                    //WriteToFile(solutions);
                }
            //}

        });
        stopwatch.Stop();
        return solutions.Values.Sum().ToString();
    }
}