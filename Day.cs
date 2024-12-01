using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal abstract class Day
    {
        public abstract int DayNumber { get; }
        public abstract string Solve(string input);
    }
}
