using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/9"/>
/// </summary>
[Day(2024, 9)]
internal class Day9 : Day
{
    public override string Input => Resources._2024_9_Input;
    //public override string Input => Resources._2024_9_Test_Input;

    public override async Task<string> Solve(string input)
    {
        List<List<int>> list = [];

        void DoDebug()
        {
            string debug = "";

            for (int x = 0; x < list.Count; x++)
                for (int y = 0; y < list[x].Count; y++)
                    debug += list[x][y].ToString();

            Console.WriteLine(debug);
        }

        for (int i = 0; i < input.Length; i++)
        {
            int howMany = int.Parse(input[i].ToString());

            if (i % 2 == 0) // Is block
            {
                list.Add([]);
                for (int j = 0; j < howMany; j++)
                {
                    list[i / 2].Add(i / 2);
                }
            }
            else
            {
                for (int j = 0; j < howMany; j++)
                {
                    list[i / 2].Add(-1);
                }
            }
        }

        DoDebug();

        //while (true)
        //{
        //    //Get x y of first -1
        //    Vector2 firstNegIndex = new Vector2(-1, -1);
        //    for (int x = 0; x < list.Count; x++)
        //    {
        //        if (firstNegIndex.X > -1) break;
        //        for (int y = 0; y < list[x].Count; y++)
        //        {
        //            if (list[x][y] == -1)
        //            {
        //                firstNegIndex = new Vector2(x, y);
        //                break;
        //            }
        //        }
        //    }

        //    if (firstNegIndex.X == -1) break; // We're done

        //    // Get x y of last number
        //    Vector2 lastNumIndex = new Vector2(-1, -1);
        //    for (int x = list.Count - 1; x >= 0; x--)
        //    {
        //        if (lastNumIndex.X > -1) break;
        //        for (int y = list[x].Count - 1; y >= 0; y--)
        //        {
        //            if (list[x][y] > -1)
        //            {
        //                lastNumIndex = new Vector2(x, y);
        //                break;
        //            }

        //        }
        //    }

        //    if (firstNegIndex.X > lastNumIndex.X 
        //        || (firstNegIndex.X == lastNumIndex.X && firstNegIndex.Y > lastNumIndex.Y))
        //        break; // We're done

        //    int number = list[(int)lastNumIndex.X][(int)lastNumIndex.Y];
        //    list[(int)lastNumIndex.X].RemoveAt((int)lastNumIndex.Y);
        //    list[(int)firstNegIndex.X].RemoveAt((int)firstNegIndex.Y);
        //    list[(int)firstNegIndex.X].Insert((int)firstNegIndex.Y, number);
        //}

        void DoOperation()
        {
            int x = list.Count - 1;
            Loop:
            if (x == -1) return;
            for (int y = list[x].Count - 1; y >= 0; y--)
            {
                if (list[x][y] > -1)
                {
                    // Get all adjacent numbers
                    int count = list[x].Where(a => a == list[x][y]).Count();
                    Vector3 num = new Vector3(x, y, count);

                    // Now find a -1 block that will fit this
                    for (int x1 = 0; x1 < list.Count; x1++)
                        for (int y1 = 0; y1 < list[x1].Count; y1++)
                        {
                            if (x1 >= x) 
                                break;

                            if (list[x1][y1] == -1)
                            {
                                // get count
                                count = list[x1].Where(a => a == -1).Count();
                                if (count >= num.Z)
                                {
                                    int number = list[x][y];
                                    int numberIndex = list[x].IndexOf(number);
                                    list[x].RemoveRange(numberIndex, (int)num.Z);
                                    list[x1].RemoveRange(y1, (int)num.Z);
                                    for (int i = 0; i < num.Z; i++)
                                    {
                                        list[x1].Insert(y1, number);
                                        list[x].Insert(numberIndex, -1);
                                    }
                                    DoDebug();
                                    x--;
                                    goto Loop;
                                }
                            }
                        }
                }
            }
            x--;
            goto Loop;
        }

        DoOperation();

        int pos = -1;
        long total = 0;
        
        for (int x = 0; x < list.Count; x++)
            for (int y = 0; y < list[x].Count; y++)
            {
                pos++;
                if (list[x][y] == -1) continue;
                total += ((long)pos * list[x][y]);
            }

        //DoDebug();
        return total.ToString();
    }
}
