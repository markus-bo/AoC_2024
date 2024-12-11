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
using Microsoft.VisualBasic;
using System.ComponentModel;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var stones = input.First()
                          .Split()
                          .ToList();
        
        for (int i = 0; i < 25; i++)
        {
            for (int j = 0; j < stones.Count; j++)
            {
                if (stones[j] == "0" || stones[j] == "")
                {
                    stones[j] = "1";
                    continue;
                }

                if (stones[j].Length % 2 == 0)
                {
                    stones.Insert(j, stones[j][..(int)(stones[j].Length / 2)]);
                    stones[j + 1] = stones[j + 1][(int)(stones[j + 1].Length / 2)..].TrimStart('0');

                    j++;
                    continue;
                }

                stones[j] = (long.Parse(stones[j]) * 2024L).ToString();
            }
        }

        return stones.Count;
    }

    static object? solutionPart2(string[] input)
    {
        var stones = input.First()
                          .Split()
                          .ToList();

        long stoneCount = stones.Sum(x => GetStoneCountAfterBlinks(75, x));

        return stoneCount;
    }

    private static Dictionary<(string, int), long> cache = new();

    private static long GetStoneCountAfterBlinks(int blinks, string value)
    {
        if (blinks == 0)
        {
            return 1;
        }

        if (cache.ContainsKey((value, blinks)))
        {
            return cache[(value, blinks)];
        }

        long result;

        if (value == "0" || value == "")
        {
            result = GetStoneCountAfterBlinks(blinks - 1, "1");
        }
        else if (value.Length % 2 == 0)
        {
            var leftResult = GetStoneCountAfterBlinks(blinks - 1, value[..(value.Length / 2)]);
            var rightResult = GetStoneCountAfterBlinks(blinks - 1, value[(value.Length / 2)..].TrimStart('0'));

            result = leftResult + rightResult;
        }
        else
        {
            result = GetStoneCountAfterBlinks(blinks - 1, (long.Parse(value) * 2024L).ToString());
        }

        cache.Add((value, blinks), result);

        return result;
    } 
}