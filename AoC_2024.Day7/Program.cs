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
using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
using System.Collections.Concurrent;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var result = 0L;

        var equations = input.Select(x => x.Split(": "))
             .Select(x => (testValue: long.Parse(x[0]),
                           values: x[1].Split().Select(long.Parse).ToArray()))
             .ToList();

        foreach(var equation in equations)
        {
            var check = IsValidEquationPart1(equation.testValue, equation.values[0], equation.values, 1, equation.values.Length);

            if (check)
            {
                result += equation.testValue;
            }
        }

        return result;
    }

    private static bool IsValidEquationPart1(long testValue, long interimResult, long[] values, int index, int length) 
    {
        if (testValue == interimResult && index == length)
        {
            return true;
        }

        if (index == length)
        {
            return false;
        }

        if (interimResult > testValue)
        {
            return false;
        }

        var resultA = IsValidEquationPart1(testValue, interimResult + values[index], values, index + 1, length);
        var resultB = IsValidEquationPart1(testValue, interimResult * values[index], values, index + 1, length);

        return resultA || resultB;
    }

    static object? solutionPart2(string[] input)
    {
        var result = 0L;

        var equations = input.Select(x => x.Split(": "))
             .Select(x => (testValue: long.Parse(x[0]),
                           values: x[1].Split().Select(long.Parse).ToArray()))
             .ToList();

        foreach(var equation in equations)
        {
            var check = IsValidEquationPart2(equation.testValue, equation.values[0], equation.values, 1, equation.values.Length);

            if (check)
            {
                result += equation.testValue;
            }
        }

        return result;
    }

    private static bool IsValidEquationPart2(long testValue, long interimResult, long[] values, int index, int length) 
    {
        if (testValue == interimResult && index == length)
        {
            return true;
        }

        if (index == length)
        {
            return false;
        }

        if (interimResult > testValue)
        {
            return false;
        }

        var resultA = IsValidEquationPart2(testValue, interimResult + values[index], values, index + 1, length);
        var resultB = IsValidEquationPart2(testValue, interimResult * values[index], values, index + 1, length);
        var resultC = IsValidEquationPart2(testValue, long.Parse(interimResult.ToString() + values[index].ToString()), values, index + 1, length);

        return resultA || resultB || resultC;
    }
}