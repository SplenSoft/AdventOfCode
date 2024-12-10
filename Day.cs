namespace AdventOfCode;

internal abstract class Day
{
    public virtual string? Synopsis { get; }
    public abstract string Input { get; }
    public abstract Task Solve(string input, long[] totals);
}