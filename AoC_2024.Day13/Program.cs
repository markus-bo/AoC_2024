using AoC_Toolbox;
using AoC_Toolbox.Geometry;
using AoC_Toolbox.InputParsing;
using AoC_Toolbox.Pathfinding;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using AoC_Toolbox.Mathematic;

record ClawMachineConfig(Point ButtonA, Point ButtonB, Point Prize, int TokenACost, int TokenBCost);

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var clawMachines = new List<ClawMachineConfig>();

        for (int i = 0; i <= input.Length - 3; i += 4)
        {
            var line = input[i];

            var clawMachine = new ClawMachineConfig(
                ButtonA: ParseButtonInputLine(line),
                ButtonB: ParseButtonInputLine(input[i + 1]),
                Prize: ParsePrizeInputLine(input[i + 2]),
                TokenACost: 3,
                TokenBCost: 1
            );

            clawMachines.Add(clawMachine);
        }

        var totalTokens = 0L;

        foreach (var clawMachine in clawMachines)
        {
            var result = MinimizeTokens2(clawMachine, 0, 0, 0, 0);

            if (result == long.MaxValue)
            {
                continue;
            }

            totalTokens += result;
        }

        return totalTokens;
    }

    static long MinimizeTokens2(ClawMachineConfig clawMachineConfig, long currentPositionX, long currentPositionY, int cost, int recursionCount)
    {
        if (clawMachineConfig.Prize.X == 0 && clawMachineConfig.Prize.Y == 0)
        {
            return 0;
        }

        if (clawMachineConfig.Prize.X % clawMachineConfig.ButtonA.X == 0 &&
            clawMachineConfig.Prize.Y % clawMachineConfig.ButtonA.Y == 0)
        {
            var totalMovementsX = clawMachineConfig.Prize.X / clawMachineConfig.ButtonA.X;

            if (totalMovementsX * clawMachineConfig.ButtonA.Y == clawMachineConfig.Prize.Y)
            {
                return totalMovementsX * clawMachineConfig.TokenACost;
            }
        }

        if (clawMachineConfig.Prize.X % clawMachineConfig.ButtonB.X == 0 &&
            clawMachineConfig.Prize.Y % clawMachineConfig.ButtonB.Y == 0)
        {
            var totalMovementsX = clawMachineConfig.Prize.X / clawMachineConfig.ButtonB.X;

            if (totalMovementsX * clawMachineConfig.ButtonB.Y == clawMachineConfig.Prize.Y)
            {
                return totalMovementsX * clawMachineConfig.TokenBCost;
            }
        }

        var minimumTokenCost = long.MaxValue;

        var gcd = new long[] { clawMachineConfig.Prize.X, clawMachineConfig.Prize.Y}
                        .GetGCD();

        var requiredXDivisor = clawMachineConfig.Prize.X / (double)gcd;
        var requiredYDivisor = clawMachineConfig.Prize.Y / (double)gcd;

        for (int pressesButtonA = 1; pressesButtonA < 100000; pressesButtonA++)
        {
            for (int pressesButtonB = 1; pressesButtonB < 100000; pressesButtonB++)
            {
                var movementX = pressesButtonA * clawMachineConfig.ButtonA.X + pressesButtonB * clawMachineConfig.ButtonB.X;
                var movementY = pressesButtonA * clawMachineConfig.ButtonA.Y + pressesButtonB * clawMachineConfig.ButtonB.Y;


                if (movementX / requiredXDivisor == movementY / requiredYDivisor)
                {
                    var totalMovementsX = clawMachineConfig.Prize.X / (double)movementX;

                    var totalPressesButtonA = (long)(totalMovementsX * pressesButtonA);
                    var totalPressesButtonB = (long)(totalMovementsX * pressesButtonB);

                    minimumTokenCost = Math.Min(minimumTokenCost, totalPressesButtonA * clawMachineConfig.TokenACost + totalPressesButtonB * clawMachineConfig.TokenBCost);

                    return minimumTokenCost;
                }
            }
        }

        return minimumTokenCost;
    }

    static Point ParseButtonInputLine(string line)
    {
        var split = line.Split(": ");
        var split2 = split[1].Split(", ")
                             .Select(x => x.Split("+")[1])
                             .Select(int.Parse)
                             .ToList();

        return new Point(x: split2[0], y: split2[1]);
    }

    static Point ParsePrizeInputLine(string line)
    {
        var split = line.Split(": ");
        var split2 = split[1].Split(", ")
                             .Select(x => x.Split("=")[1])
                             .Select(int.Parse)
                             .ToList();

        return new Point(x: split2[0], y: split2[1]);
    }

    static Point ParsePrizeInputLineWithHighCost(string line)
    {
        var split = line.Split(": ");
        var split2 = split[1].Split(", ")
                             .Select(x => x.Split("=")[1])
                             .Select(int.Parse)
                             .ToList();

        return new Point(x: split2[0] + 10000000000000L, y: split2[1] + 10000000000000L);
    }

    static object? solutionPart2(string[] input)
    {
                var clawMachines = new List<ClawMachineConfig>();

        for (int i = 0; i <= input.Length - 3; i += 4)
        {
            var line = input[i];

            var clawMachine = new ClawMachineConfig(
                ButtonA: ParseButtonInputLine(line),
                ButtonB: ParseButtonInputLine(input[i + 1]),
                Prize: ParsePrizeInputLineWithHighCost(input[i + 2]),
                TokenACost: 3,
                TokenBCost: 1
            );

            clawMachines.Add(clawMachine);
        }

        var totalTokens = 0L;

        foreach (var clawMachine in clawMachines)
        {
            var result = MinimizeTokens2(clawMachine, 0, 0, 0, 0);

            if (result == long.MaxValue)
            {
                continue;
            }

            totalTokens += result;
        }

        return totalTokens;
    }
}