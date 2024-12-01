using AdventOfCode2024;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

List<Day> days = [];

var assembly = Assembly.GetExecutingAssembly()
    ?? throw new Exception("Assembly was null");

foreach (var type in assembly.GetTypes())
{
    Debug.WriteLine($"AdventofCode Day initialized: {type.Name}");

    if (type.IsSubclassOf(typeof(Day)))
    {
        object instance = Activator.CreateInstance(type)
            ?? throw new Exception("Instance was null");

        days.Add((Day)instance);
    }
}

days = [.. days.OrderBy(d => d.DayNumber)];

int? dayNumber = null;

while (dayNumber == null)
{
    Console.WriteLine($"Please enter a day number (1 - {days.Count}):");
    string? dayNumberInput = Console.ReadLine();

    if (int.TryParse(dayNumberInput, out int validInt) && validInt > 0 
        && validInt <= days.Count)
    {
        dayNumber = validInt - 1;
        break;
    }

    Console.WriteLine("Invalid integer detected");
}

//string? input = null;

//while (input == null)
//{
//    Console.WriteLine($"Please paste your puzzle input and press enter:");
//    input = Console.ReadLine();
//}

Day day = days[(int)dayNumber];
Console.WriteLine(day.Solve(day.SampleInput));