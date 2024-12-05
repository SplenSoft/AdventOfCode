namespace AdventOfCode;

internal abstract class Day
{
    public abstract int Year { get; }
    public abstract int DayNumber { get; }
    public abstract string Synopsis { get; }
    public abstract string Input { get; }
    public abstract string Solve(string input);
}