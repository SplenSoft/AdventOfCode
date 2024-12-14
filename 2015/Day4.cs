using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015;

/// <summary>
/// <see href="https://adventofcode.com/2015/day/4"/>
/// </summary>
[Day(2015, 4)]
internal class Day4 : Day
{
    public override async Task Solve(string input, long[] totals)
    {
        int j = 0;
        List<List<long>> numbers = [[], []];
        Action[] actions = new Action[Environment.ProcessorCount];
        while (numbers[0].Count == 0 || numbers[1].Count == 0)
        {
            for (int l = 0; l < Environment.ProcessorCount; l++)
            {
                long amt = j * l * 100000;
                actions[l] = () => 
                {
                    for (long i = amt; i < amt + 100000; i++)
                    {
                        var res = Convert.ToHexString(MD5.HashData(
                            Encoding.ASCII.GetBytes(input + i.ToString())));
                        if (res[..6] == "000000") numbers[1].Add(i);
                        if (res[..5] == "00000") numbers[0].Add(i);
                    }
                };
            }
            Parallel.Invoke(actions);
            j++;
        }
        totals[0] = numbers[0].Order().First();
        totals[1] = numbers[1].Order().First();
    }
}