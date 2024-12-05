using AdventOfCode;
using System.Diagnostics;
using System.Reflection;

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

    Debug.WriteLine(
        $"Adding day {instance.DayNumber} to year {instance.Year}");

    if (!days.TryGetValue(instance.Year, out List<Day>? value))
    {
        value = [];
        days[instance.Year] = value;
    }

    value.Add(instance);
}

foreach (var key in days.Keys)
    days[key] = [.. days[key].OrderBy(d => d.DayNumber)];

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
Console.WriteLine(day.Solve(day.Input));