namespace AdventOfCode._2024;

/// <summary>
/// <see href="https://adventofcode.com/2024/day/17"/>
/// </summary>
[Day(2024, 17)]
internal class Day17 : Day
{
    public override async Task Solve(string input, dynamic[] totals)
    {
        string[] lines = input.Split(Environment.NewLine);
        ulong part1A = ulong.Parse(lines[0][12..]);
        var prog1 = lines[4][9..].Split(',').Select(ulong.Parse).ToList();
        // Part 1, run the program with our input A register
        List<ulong> Run(ulong a /*Register A*/, List<ulong> prog)
        {
            List<ulong> outs = [];
            ulong b = 0; // Register B
            ulong c = 0; // Register C

            ulong Combo(ulong operand) => operand switch
            {
                4 => a,
                5 => b,
                6 => c,
                _ => operand,
            };

            ulong GetAdv(ulong op) => 
                (ulong)Math.Floor(a / Math.Pow(2, Combo(op)));

            for (int i = 0; i < prog.Count;)
            {
                int oc = (int)prog[i];
                if (oc == 0) a = GetAdv(prog[i + 1]);
                else if (oc == 1) b ^= prog[i + 1];
                else if (oc == 2) b = Combo(prog[i + 1]) % 8;
                else if (oc == 3 && a != 0)
                {
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
        
        var part1 = Run(part1A, prog1);
        totals[0] = string.Join(',', part1);

        // Part 2, reverse engineering by finding the bits we took out
        List<ulong> validValsA = [0]; // Start with nothing, slowly add bits
        var reversed = prog1.ToArray().Reverse().ToList();
        for (int i = 0; i < reversed.Count; i++)
        {
            int oc = (int)prog1[i];
            List<ulong> newAs = [];
            foreach (var a in validValsA)
                for (uint j = 0; j < 8; j++) // 8 == 3 bits
                {   // Find valid bit replacements and build 'A' Registers
                    ulong newA = a << 3; // Bitshift 3 to make room for bits
                    newA += j; // Add bits to the larger number
                    var res = Run(newA, prog1); // Run through program
                    if (res[0] == reversed[i]) newAs.Add(newA); // Save valid
                }

            validValsA = newAs.ToList();
        }

        totals[1] = validValsA.Order().First(); // Take the lowest A register
    }
}
