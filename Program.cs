using AdventOfCode;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

Dictionary<int, List<Day>> days = [];
var assembly = Assembly.GetExecutingAssembly()
    ?? throw new Exception("Assembly was null");

foreach (var type in assembly.GetTypes())
{
    if (!type.IsSubclassOf(typeof(Day)))
        continue;

    Debug.WriteLine($"AdventofCode Day initialized: {type.Name}");

    Day instance = (Day?)Activator.CreateInstance(type)
        ?? throw new Exception("Instance was null");

    var dayAttr = (DayAttribute?)instance.GetType()
        .GetCustomAttribute(typeof(DayAttribute), true) 
        ?? throw new Exception($"{nameof(DayAttribute)} was null");

    Debug.WriteLine(
        $"Adding day {dayAttr.Day} to year {dayAttr.Year}");

    if (!days.TryGetValue(dayAttr.Year, out List<Day>? value))
    {
        value = [];
        days[dayAttr.Year] = value;
    }

    value.Insert(Math.Min(dayAttr.Day - 1, value.Count), instance);
}

int? year = null;
int? dayNumber = null;
var orderedYears = days.Keys.Order();
int oldestYear = orderedYears.First();
int newestYear = orderedYears.Last();

GetYear:
while (year == null)
{
    Console.WriteLine(
        $"Please enter a year between {oldestYear} and {newestYear}:");

    string? yearNumberInput = Console.ReadLine();

    if (int.TryParse(yearNumberInput, out int validInt)
        && validInt >= oldestYear && validInt <= newestYear)
    {
        year = validInt;
        break;
    }

    Console.WriteLine("Invalid integer detected");
}

if (!days.ContainsKey((int)year))
{
    Console.WriteLine($"No days available for year {(int)year}. Try again.");
    year = null;
    goto GetYear;
}

while (dayNumber == null)
{
    Console.WriteLine(
        $"Please enter a day number between 1 and {days[(int)year].Count}:");

    string? dayNumberInput = Console.ReadLine();

    if (int.TryParse(dayNumberInput, out int validInt) && validInt > 0
        && validInt <= days[(int)year].Count)
    {
        dayNumber = validInt - 1;
        break;
    }

    Console.WriteLine("Invalid integer detected");
}

Console.WriteLine($"Solving Advent of Code year {year}, day {dayNumber + 1}");
Day day = days[(int)year][(int)dayNumber];
long[] totals = [0, 0];
await day.Solve(day.Input, totals);
string result = $"Part 1 solution: {totals[0]}\nPart 2 solution: {totals[1]}";
Console.WriteLine(result);