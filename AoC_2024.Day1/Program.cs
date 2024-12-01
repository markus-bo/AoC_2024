using AoC_Toolbox;
using AoC_Toolbox.Geometry;
using AoC_Toolbox.InputParsing;
using AoC_Toolbox.Pathfinding;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.ExceptionServices;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var rows = input.Select(x => x.Split("   ")).Select(x => (first: int.Parse(x[0]), second: int.Parse(x[1])));

        var lefts = rows.Select(x => x.first).OrderBy(x => x);
        var rights = rows.Select(x => x.second).OrderBy(x => x);

        var result = lefts.Zip(rights).Select(x => Math.Abs(x.First - x.Second)).Sum();

        return result;
    }

    static object? solutionPart2(string[] input)
    {
        var rows = input.Select(x => x.Split("   ")).Select(x => (first: int.Parse(x[0]), second: int.Parse(x[1])));

        var lefts = rows.Select(x => x.first).OrderBy(x => x);
        var rights = rows.Select(x => x.second).OrderBy(x => x);

        var result = lefts.Sum(x => x * rights.Count(y => y == x));

        return result;
    }
}