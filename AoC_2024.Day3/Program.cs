using AoC_Toolbox;
using AoC_Toolbox.Geometry;
using AoC_Toolbox.InputParsing;
using AoC_Toolbox.Pathfinding;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Schema;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var pattern = new Regex(@"mul\([\d]*,[\d]*\)");

        var result = 0;

        foreach (var line in input)
        {
            var matches = pattern.Matches(line);

            foreach (Match match in matches)
            {
                var valuePair = match.Value
                                 .Replace("mul(", "")
                                 .Replace(")", "")
                                 .Split(',')
                                 .Select(int.Parse)
                                 .ToList();               

                result += valuePair[0] * valuePair[1];
            }
        }
        return result;
    }

    static object? solutionPart2(string[] input)
    {
        var pattern = new Regex(@"((mul\([\d]*,[\d]*\))|do\(\)|don't\(\))");
        
        var result = 0L;

        var captures = input.SelectMany(x => pattern.Matches(x))
             .Select(x => x.Value)
             .ToList();;

        var enableMultiply = true;

        foreach (var capture in captures)
        {
            if (capture == "do()")
            {
                enableMultiply = true;
                continue;
            }
            else if (capture == "don't()")
            {
                enableMultiply = false;
                continue;
            }

            if (enableMultiply)
            {
                var valuePair = capture
                                 .Replace("mul(", "")
                                 .Replace(")", "")
                                 .Split(',')
                                 .Select(int.Parse)
                                 .ToList();               

                result += valuePair[0] * valuePair[1];
            }
        }
        return result;
    }
}