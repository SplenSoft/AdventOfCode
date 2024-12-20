using static System.Math;
namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/17"/>
/// </summary>
[Day(2024, 17)]
internal class Day17 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        List<ulong> Run(ulong a /*Register A*/, List<ulong> prog)
        {
            List<ulong> outs = [];
            ulong b = 0; // Register B
            ulong c = 0; // Register C
            ulong GetAdv(ulong o) => (ulong)Floor(a / Pow(2, Combo(o)));
            ulong Combo(ulong o) => o == 4 ? a : o == 5 ? b : o == 6 ? c : o;

            for (int i = 0; i < prog.Count;)
            {   // Giant list of instructions from AoC, pretty much verbatim
                int oc = (int)prog[i]; // Opcode
                if (oc == 0) a = GetAdv(prog[i + 1]);
                else if (oc == 1) b ^= prog[i + 1];
                else if (oc == 2) b = Combo(prog[i + 1]) % 8;
                else if (oc == 3 && a != 0)
                {   // JUMP AROUND!
                    i = (int)prog[i + 1];
                    continue;
                }
                else if (oc == 4) b ^= c;
                else if (oc == 5) outs.Add(Combo(prog[i + 1]) % 8); // 3 bits!
                else if (oc == 6) b = GetAdv(prog[i + 1]);
                else if (oc == 7) c = GetAdv(prog[i + 1]);
                i += 2;
            }

            return outs;
        }

        string[] lines = input.Split(Environment.NewLine);
        var prog1 = lines[4][9..].Split(',').Select(ulong.Parse).ToList();
        List<ulong> validValsA = [0]; // Part 2, reverse engineering ...
        var reversed = prog1.ToArray().Reverse().ToList(); // ... literally.
        for (int i = 0; i < reversed.Count; i++) // For each program digit
        {   // We need to hand-build a valid Register 'A', 3 bits at a time
            int oc = (int)prog1[i];
            List<ulong> newAs = []; // Valid A registers get stored here
            foreach (var a in validValsA)
                for (uint j = 0; j < 8; j++) // 8 == 3 bits
                {   // Find valid bit replacements and build 'A' Registers
                    ulong newA = a << 3; // Bitshift 3 to make room for bits
                    newA += j; // Add bits to the larger number
                    var res = Run(newA, prog1); // Run through program
                    if (res[0] == reversed[i]) newAs.Add(newA); // Save valid
                }

            validValsA = [.. newAs]; // Use valid A's for next program digit
        }

        totals[0] = string.Join(',', Run(ulong.Parse(lines[0][12..]), prog1));
        totals[1] = validValsA.Order().First(); // Lowest valid A register
    }
}