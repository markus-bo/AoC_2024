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
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var height = input.Length;
        var width = input.First().Length;

        Dictionary<char, List<(int x, int y)>> antennas = GetAntennas(input, height, width);

        var antinodes = new Dictionary<char, List<(int x, int y)>>();

        foreach (var antennaKey in antennas.Keys)
        {
            antinodes.Add(antennaKey, new List<(int x, int y)>());

            for (int i = 0; i < antennas[antennaKey].Count; i++)
            {
                for (int j = i + 1; j < antennas[antennaKey].Count; j++)
                {
                    var diffX = Math.Abs(antennas[antennaKey][i].x - antennas[antennaKey][j].x);
                    var diffY = Math.Abs(antennas[antennaKey][i].y - antennas[antennaKey][j].y);

                    var antinodeAX = -1;
                    var antinodeAY = -1;
                    var antinodeBX = -1;
                    var antinodeBY = -1;

                    if (antennas[antennaKey][i].x <= antennas[antennaKey][j].x)
                    {
                        antinodeAX = antennas[antennaKey][i].x - diffX;
                        antinodeBX = antennas[antennaKey][j].x + diffX;
                    }
                    else
                    {
                        antinodeAX = antennas[antennaKey][i].x + diffX;
                        antinodeBX = antennas[antennaKey][j].x - diffX;
                    }

                    if (antennas[antennaKey][i].y <= antennas[antennaKey][j].y)
                    {
                        antinodeAY = antennas[antennaKey][i].y - diffY;
                        antinodeBY = antennas[antennaKey][j].y + diffY;
                    }
                    else
                    {
                        antinodeAY = antennas[antennaKey][i].y + diffY;
                        antinodeBY = antennas[antennaKey][j].y - diffY;
                    }

                    antinodes[antennaKey].Add((x: antinodeAX, y: antinodeAY));
                    antinodes[antennaKey].Add((x: antinodeBX, y: antinodeBY));
                }
            }
        }

        var result = antinodes.SelectMany(x => x.Value)
                              .Distinct()
                              .Where(a => a.x >= 0 && a.x < width && a.y >= 0 && a.y < height)
                              .Count();

        return result;
    }

    private static Dictionary<char, List<(int x, int y)>> GetAntennas(string[] input, int height, int width)
    {
        var antennas = new Dictionary<char, List<(int x, int y)>>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (input[y][x] == '.')
                {
                    continue;
                }

                if (antennas.ContainsKey(input[y][x]) == false)
                {
                    antennas.Add(input[y][x], new List<(int x, int y)>());
                }

                antennas[input[y][x]].Add((x: x, y: y));
            }
        }

        return antennas;
    }

    static object? solutionPart2(string[] input)
    {
        var height = input.Length;
        var width = input.First().Length;

        Dictionary<char, List<(int x, int y)>> antennas = GetAntennas(input, height, width);

        var antinodes = new Dictionary<char, List<(int xOrigin, int yOrigin, int xStep, int yStep)>>();

        foreach(var antennaKey in antennas.Keys)
        {
            antinodes.Add(antennaKey, new List<(int xOrigin, int yOrigin, int xStep, int yStep)>());

            for (int i = 0; i < antennas[antennaKey].Count; i++)
            {
                for (int j = i + 1; j < antennas[antennaKey].Count; j++)
                {
                    var diffX = Math.Abs(antennas[antennaKey][i].x - antennas[antennaKey][j].x);
                    var diffY = Math.Abs(antennas[antennaKey][i].y - antennas[antennaKey][j].y);

                    var antinodeAXOrigin = -1;
                    var antinodeAYOrigin = -1;
                    var antinodeBXOrigin = -1;
                    var antinodeBYOrigin = -1;
                    var antinodeAXStep = -1;
                    var antinodeAYStep = -1;
                    var antinodeBXStep = -1;
                    var antinodeBYStep = -1;

                    if (antennas[antennaKey][i].x <= antennas[antennaKey][j].x)
                    {
                        antinodeAXOrigin = antennas[antennaKey][i].x - diffX;
                        antinodeAXStep = -diffX;
                        antinodeBXOrigin = antennas[antennaKey][j].x + diffX;
                        antinodeBXStep = diffX;
                    }
                    else
                    {
                        antinodeAXOrigin = antennas[antennaKey][i].x + diffX;
                        antinodeAXStep = diffX;
                        antinodeBXOrigin = antennas[antennaKey][j].x - diffX;
                        antinodeBXStep = -diffX;
                    }

                    if (antennas[antennaKey][i].y <= antennas[antennaKey][j].y)
                    {
                        antinodeAYOrigin = antennas[antennaKey][i].y - diffY;
                        antinodeAYStep = -diffY;
                        antinodeBYOrigin = antennas[antennaKey][j].y + diffY;
                        antinodeBYStep = diffY;
                    }
                    else
                    {
                        antinodeAYOrigin = antennas[antennaKey][i].y + diffY;
                        antinodeAYStep = diffY;
                        antinodeBYOrigin = antennas[antennaKey][j].y - diffY;
                        antinodeBYStep = -diffY;
                    }

                    antinodes[antennaKey].Add((xOrigin: antinodeAXOrigin, yOrigin: antinodeAYOrigin, xStep: antinodeAXStep, yStep: antinodeAYStep));
                    antinodes[antennaKey].Add((xOrigin: antinodeBXOrigin, yOrigin: antinodeBYOrigin, xStep: antinodeBXStep, yStep: antinodeBYStep));
                }
            }
        }

        var expandedAntinodes = new List<(int x, int y)>();

        foreach(var antinodePosition in antinodes.SelectMany(x => x.Value))
        {
            var index = 0;

            while(true)
            {
                var point = (x: antinodePosition.xOrigin + antinodePosition.xStep * index, y: antinodePosition.yOrigin + antinodePosition.yStep * index);

                if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height)
                {
                    break;
                }

                expandedAntinodes.Add((x: point.x, y: point.y));

                index++;
            }
        }

        var result = expandedAntinodes.Concat(antennas.SelectMany(x => x.Value))
                                  .Distinct()
                                  .Count();

        return result;
    }
}