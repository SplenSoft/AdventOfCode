using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class)]
internal class DayAttribute(int year, int day) 
    : Attribute
{
    public int Year { get; } = year;
    public int Day { get; } = day;
}