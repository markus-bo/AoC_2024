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
using System.Text;
using Microsoft.VisualBasic;

class Robot()
{
    public Point Position { get; set; }
    public int VelX { get; set; }
    public int VelY { get; set; }
}

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var width = 101;
        var height = 103;

        var robots = input.Select(x => x.Split())
                          .Select(x => (position: x[0].Split("=")[1]
                                                      .Split(",")
                                                      .Select(int.Parse)
                                                      .ToList(),
                                        velocity: x[1].Split("=")[1]
                                                      .Split(",")
                                                      .Select(int.Parse)
                                                      .ToList()))
                          .Select(x => new Robot() { 
                                                 Position = new Point(x.position[0], x.position[1]),
                                                 VelX = x.velocity[0],
                                                 VelY = x.velocity[1]})
                          .ToList();

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < robots.Count; j++)
            {
                robots[j].Position = new Point(robots[j].Position.X + robots[j].VelX, robots[j].Position.Y + robots[j].VelY);

                while(robots[j].Position.X >= width) robots[j].Position = robots[j].Position with { X = robots[j].Position.X - width };
                while(robots[j].Position.X < 0) robots[j].Position = robots[j].Position with { X = robots[j].Position.X + width };
                while(robots[j].Position.Y >= height) robots[j].Position = robots[j].Position with { Y = robots[j].Position.Y - height };
                while(robots[j].Position.Y < 0) robots[j].Position = robots[j].Position with { Y = robots[j].Position.Y + height };
            }
        }

        var resultTopLeftQuadrant = 0;
        var resultTopRightQuadrant = 0;
        var resultBottomLeftQuadrant = 0;
        var resultBottomRightQuadrant = 0;
        var resultOutOfBounds = 0;

        foreach(var robot in robots)
        {
            if (robot.Position.X < width / 2 && robot.Position.Y < height / 2)
            {
                resultTopLeftQuadrant++;
            }
            else if (robot.Position.X > width / 2 && robot.Position.Y < height / 2)
            {
                resultTopRightQuadrant++;
            }
            else if (robot.Position.X < width / 2 && robot.Position.Y > height / 2)
            {
                resultBottomLeftQuadrant++;
            }
            else if (robot.Position.X > width / 2 && robot.Position.Y > height / 2)
            {
                resultBottomRightQuadrant++;
            }
            else
            {
                resultOutOfBounds++;
            }
        }

        return resultTopLeftQuadrant * resultTopRightQuadrant * resultBottomLeftQuadrant * resultBottomRightQuadrant;
    }

    private static void DebugOutput(List<Robot> robots, int width, int height)
    {
        var map = new int[height, width];

        foreach(var robot in robots)
        {
            map[robot.Position.Y, robot.Position.X]++;
        }

        for (int y = 0; y < height; y++)
        {
            var outputLine = new StringBuilder();

            for (int x = 0; x < width; x++)
            {
                outputLine.Append(map[y, x] == 0 ? "." : map[y, x].ToString());
            }

            Console.Error.WriteLine(outputLine.ToString());
        }
    }
    static object? solutionPart2(string[] input)
    {
        if (input.Length <20) return 0;

        var width = 101;
        var height = 103;

        var robots = input.Select(x => x.Split())
                          .Select(x => (position: x[0].Split("=")[1]
                                                      .Split(",")
                                                      .Select(int.Parse)
                                                      .ToList(),
                                        velocity: x[1].Split("=")[1]
                                                      .Split(",")
                                                      .Select(int.Parse)
                                                      .ToList()))
                          .Select(x => new Robot() { 
                                                 Position = new Point(x.position[0], x.position[1]),
                                                 VelX = x.velocity[0],
                                                 VelY = x.velocity[1]})
                          .ToList();

        var lowestVariance = double.MaxValue;
        var lowestVarianceIteration = 0;

        var variances = new Dictionary<int, double>();

        for (int i = 0; i < 10000; i++)
        {
            for (int j = 0; j < robots.Count; j++)
            {
                robots[j].Position = new Point(robots[j].Position.X + robots[j].VelX, robots[j].Position.Y + robots[j].VelY);

                while(robots[j].Position.X >= width) robots[j].Position = robots[j].Position with { X = robots[j].Position.X - width };
                while(robots[j].Position.X < 0) robots[j].Position = robots[j].Position with { X = robots[j].Position.X + width };
                while(robots[j].Position.Y >= height) robots[j].Position = robots[j].Position with { Y = robots[j].Position.Y - height };
                while(robots[j].Position.Y < 0) robots[j].Position = robots[j].Position with { Y = robots[j].Position.Y + height };
            }
            
            var points = robots.Select(x => x.Position).ToList();

            var distances = GetDistancesBetweenPoints(points);

            var variance = GetVariance(distances);

            variances.Add(i, variance);
        }

        // output the state ids ordered by lowest variance, must be some of them and check manually later
        Console.Error.WriteLine(string.Join("\n", variances.OrderBy(x => x.Value).Select(x => $"{x.Key}: {x.Value}").Take(20)));

        return 0;
    }

    static List<double> GetDistancesBetweenPoints(List<Point> points)
    {
        List<double> distances = new List<double>();
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                distances.Add(points[i].GetManhattenDistance(points[j]));
            }
        }
        return distances;
    }

    static double GetVariance(List<double> distances)
    {
        double mean = distances.Average();
        double variance = distances.Select(d => Math.Pow(d - mean, 2)).Average();
        return variance;
    }
}