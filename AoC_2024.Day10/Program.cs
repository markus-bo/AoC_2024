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
using System.ComponentModel;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var startPoints = new List<(int x, int y)>();
        var endPoints = new List<(int x, int y)>();

        var height = input.First().Length;
        var width = input.Length;

        var map = new char[width, height];

        for(var x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                map[x, y] = input[y][x];

                if(map[x, y] == '0')
                    startPoints.Add((x, y));
                else if(map[x, y] == '9')
                    endPoints.Add((x, y));
            }
        }

        var totalTrailheads = 0;

        foreach(var startPoint in startPoints)
        {
            var floodfill = new Floodfill<char>();
            var filledMap = floodfill.FillArray(map, 'X', startPoint, (a, b) => (b - '0') - (a - '0') == 1, false);

            var trailheads = 0;

            foreach(var endPoint in endPoints)
            {
                if(filledMap[endPoint.x, endPoint.y] == 'X')
                    trailheads++;
            }

            totalTrailheads += trailheads;
        }

        return totalTrailheads;
    }

    static object? solutionPart2(string[] input)
    {
        var startPoints = new List<(int x, int y)>();

        var height = input.First().Length;
        var width = input.Length;

        var map = new char[width, height];

        for(var x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                map[x, y] = input[y][x];

                if(map[x, y] == '0')
                    startPoints.Add((x, y));
            }
        }

        var totalTrailheads = 0;

        foreach(var startPoint in startPoints)
        {
            var distinctTrailheads = GetDistinctTrailheads(map, startPoint, (char)('0' - 1), height, width);

            totalTrailheads += distinctTrailheads;
        }

        return totalTrailheads;
    }

    static private int GetDistinctTrailheads(char[,] map, (int x, int y) currentPosition, char previousValue, int height, int width)
    {
        if (currentPosition.x < 0 || currentPosition.x >= width || currentPosition.y < 0 || currentPosition.y >= height)
        {
            return 0;
        } 

        if (map[currentPosition.x, currentPosition.y] - previousValue != 1)
        {
            return 0;
        }

        if (map[currentPosition.x, currentPosition.y] == '9')
        {
            return 1;
        }

        var result = 0;

        result += GetDistinctTrailheads(map, (currentPosition.x + 1, currentPosition.y), map[currentPosition.x, currentPosition.y], height, width);
        result += GetDistinctTrailheads(map, (currentPosition.x - 1, currentPosition.y), map[currentPosition.x, currentPosition.y], height, width);
        result += GetDistinctTrailheads(map, (currentPosition.x, currentPosition.y + 1), map[currentPosition.x, currentPosition.y], height, width);
        result += GetDistinctTrailheads(map, (currentPosition.x, currentPosition.y - 1), map[currentPosition.x, currentPosition.y], height, width);

        return result;
    }
}