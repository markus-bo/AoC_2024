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
using System.Formats.Asn1;

internal class Region
{
    public int Id { get; set; }
    public char Plant { get; set; }
    public int Area { get; set; }
    public int Perimeter { get; set; }
}

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var height = input.Length;
        var width = input[0].Length;

        var map = input.To2DCharArray();

        filledMap = new bool[width, height];

        var regions = new List<(int area, int perimeter)>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (filledMap[x, y] == true)
                    continue;

                var region = FillRegion(map, width, height, map[x, y], x, y);

                regions.Add(region);
            }
        }

        var result = regions.Select(x => x.area * x.perimeter)
                            .Sum();

        return result;
    }

    private static bool[,] filledMap;

    private static (int area, int perimeter) FillRegion(char[,] map, int width, int height, char scanCharacter, int currentX, int currentY)
    {   
        if (currentX < 0 || currentX >= width || currentY < 0 || currentY >= height)
            return (0, 1);

        if (map[currentX, currentY] != scanCharacter)
            return (0, 1);

        if (filledMap[currentX, currentY] == true)
            return (0, 0);

        filledMap[currentX, currentY] = true;

        var toLeft = FillRegion(map, width, height, scanCharacter, currentX - 1, currentY);
        var toRight = FillRegion(map, width, height, scanCharacter, currentX + 1, currentY);
        var toTop = FillRegion(map, width, height, scanCharacter, currentX, currentY - 1);
        var toBottom = FillRegion(map, width, height, scanCharacter, currentX, currentY + 1);

        var perimeter = toLeft.perimeter + toRight.perimeter + toTop.perimeter + toBottom.perimeter;

        return (1 + toLeft.area + toRight.area + toTop.area + toBottom.area, perimeter);
    }

    static object? solutionPart2(string[] lines)
    {
        var map = lines.ToList();

        var height = map.Count;
        var width = map[0].Length;

        for(int i = 0; i< map.Count; i++)
        {
            map[i] = $"-{map[i]}-";
        }

        map.Insert(0, new string('-', width + 2));
        map.Add(new string('-', width + 2));

        filledMap = new bool[height + 2, width + 2];

        var regions = new List<(int area, int perimeter)>();

        for (int y = 1; y < height+1; y++)
        {
            for (int x = 1; x < width+1; x++)
            {
                if (filledMap[y, x] == true)
                    continue;

                var region = FillRegion_Part2(map.ToArray(), width + 1, height + 1, map[y][x], x, y);

                regions.Add(region);
            }
        }

        var result = regions.Select(x => x.area * x.perimeter)
                            .Sum();

        return result;
    }

    private static int GetNewPerimeterEdgeCount(int x, int y, string[] map)
    {
        var result = 0;

        var current = map[y][x];
        var above = map[y-1][x];
        var below = map[y+1][x];
        var left = map[y][x-1];
        var right = map[y][x+1];
        var aboveLeft = map[y-1][x-1];
        var belowRight = map[y+1][x+1];

        result += left != current && !(above == current && aboveLeft != current) ? 1 : 0;
        result += right != map[y][x] && !(below == current && belowRight != current) ? 1 : 0;
        result += above != map[y][x] && !(left == current && aboveLeft != current) ? 1 : 0;
        result += below != map[y][x] && !(right == current && belowRight != current) ? 1 : 0;

        return result;
    }

    private static (int area, int perimeter) FillRegion_Part2(string[] map, int width, int height, char scanCharacter, int currentX, int currentY)
    {   
        if (currentX < 1 || currentX > width || currentY < 1 || currentY >= height)
            return (0, 0);

        if (map[currentY][currentX] != scanCharacter)
            return (0, 0);

        if (filledMap[currentY, currentX] == true)
            return (0, 0);

        filledMap[currentY, currentX] = true;

        var toLeft = FillRegion_Part2(map, width, height, scanCharacter, currentX - 1, currentY);
        var toRight = FillRegion_Part2(map, width, height, scanCharacter, currentX + 1, currentY);
        var toTop = FillRegion_Part2(map, width, height, scanCharacter, currentX, currentY - 1);
        var toBottom = FillRegion_Part2(map, width, height, scanCharacter, currentX, currentY + 1);

        var area = 1 + toLeft.area + toRight.area + toTop.area + toBottom.area;

        var perimeter = GetNewPerimeterEdgeCount(currentX, currentY, map)
                            + toLeft.perimeter 
                            + toRight.perimeter 
                            + toTop.perimeter 
                            + toBottom.perimeter;

        return (area, perimeter);
    }

}