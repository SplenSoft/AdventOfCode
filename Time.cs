using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class Time
{
    private static Stopwatch _timer = new();

    public static void Start()
    {
        _timer.Start();
    }

    public static void Stop() 
    { 
        _timer.Stop();
    }

    public static string Elapsed => _timer.Elapsed.ToString();
}
