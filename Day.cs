namespace AdventOfCode;

internal abstract class Day
{
    public virtual string? Synopsis { get; }
    public abstract string Input { get; }
    public abstract Task<string> Solve(string input);
}