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

internal class Solution
{
    enum Heading
    {
        North,
        East,
        South,
        West
    }

    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var (map, height, width, goalPositionX, goalPositionY, startPositionX, startPositionY) = ReadInput(input);

        var minimumCost = FindMinimumCost(
            map, height, width,
            goalPositionX, goalPositionY,
            startPositionX, startPositionY, Heading.East,
            0);

        return minimumCost;
    }

    static object? solutionPart2(string[] input)
    {
        var (map, height, width, goalPositionX, goalPositionY, startPositionX, startPositionY) = ReadInput(input);

        costmap = new Dictionary<(int y, int x, Heading h), int>();

        foundPaths = new List<((int posX, int posY)[] path, int cost)>();

        var minimumCost = FindMinimumCost(
            map, height, width,
            goalPositionX, goalPositionY,
            startPositionX, startPositionY, Heading.East,
            0);

        var allMinimumPathsLength = FindAllMinimumPathLengths(
            map, height, width,
            goalPositionX, goalPositionY,
            startPositionX, startPositionY, Heading.East,
            0,
            [(startPositionX, startPositionY)],
            minimumCost);

        return foundPaths.SelectMany(x => x.path)
                         .Distinct()
                         .Count();
    }

    static Dictionary<(int y, int x, Heading h), int> costmap;

    static Dictionary<(int posX, int posY, Heading heading), long> _exploredStates;

    static long FindMinimumCost(char[][] map, int height, int width,
                               int goalPositionX, int goalPositionY, 
                               int initialPositionX, int initialPositionY, Heading initialHeading,
                               int currentCost)
    {

        var queue = new Queue<(int posX, int posY, Heading heading, long cost)>();

        queue.Enqueue((posX: initialPositionX, posY: initialPositionY, heading: initialHeading, cost: currentCost));

        var minCost = long.MaxValue;

        _exploredStates = new Dictionary<(int posX, int posY, Heading heading), long>();

        while (queue.TryDequeue(out (int posX, int posY, Heading heading, long cost) current))
        {
            if (current.posX == goalPositionX && current.posY == goalPositionY)
            {
                minCost = Math.Min(minCost, current.cost);
                continue;
            }

            if (map[current.posY][current.posX] == '#')
            {
                continue;
            }

            if (current.cost >= minCost)
            {
                continue;
            }

            if (_exploredStates.ContainsKey((posX: current.posX, posY: current.posY, heading: current.heading)))
            {
                if (_exploredStates[(posX: current.posX, posY: current.posY, heading: current.heading)] <= current.cost)
                {
                    continue;
                }

                _exploredStates[(posX: current.posX, posY: current.posY, heading: current.heading)] = current.cost;
            }
            else
            {
                _exploredStates.Add((posX: current.posX, posY: current.posY, heading: current.heading), current.cost);
            }

            var nextForwardPosition = current.heading switch
            {
                Heading.North => (posX: current.posX, posY: current.posY - 1),
                Heading.South => (posX: current.posX, posY: current.posY + 1),
                Heading.West => (posX: current.posX - 1, posY: current.posY),
                Heading.East => (posX: current.posX + 1, posY: current.posY),
                _ => throw new Exception()
            };

            var nextMoveForwardHeadingCW = current.heading switch 
            {
                Heading.North => Heading.East,
                Heading.East => Heading.South,
                Heading.South => Heading.West,
                Heading.West => Heading.North,
                _ => throw new Exception()
            };

            var nextMoveForwardHeadingCCW = current.heading switch 
            {
                Heading.North => Heading.West,
                Heading.West => Heading.South,
                Heading.South => Heading.East,
                Heading.East => Heading.North,
                _ => throw new Exception()
            };

            queue.Enqueue((nextForwardPosition.posX, nextForwardPosition.posY, current.heading, current.cost + 1));
            queue.Enqueue((current.posX, current.posY, nextMoveForwardHeadingCW, current.cost + 1000));
            queue.Enqueue((current.posX, current.posY, nextMoveForwardHeadingCCW, current.cost + 1000));
        }

        return minCost;
    }

    static (char[][] map, int height, int width, int goalPosX, int goalPosY, int startPosX, int StartPosY) ReadInput(string[] input)
    {
        var map = new char[input.Length][];

        var height = input.Length;
        var width = input.First().Length;

        var goalPositionX = 0;
        var goalPositionY = 0;
        var startPositionX = 0;
        var startPositionY = 0;

        for(int y = 0; y < height; y++)
        {
            map[y] = new char[width];

            for(int x = 0; x < width; x++)
            {
                if (input[y][x] == 'S')
                {
                    startPositionX = x;
                    startPositionY = y;

                    map[y][x] = '.';

                    continue;
                }

                if (input[y][x] == 'E')
                {
                    goalPositionX = x;
                    goalPositionY = y;

                    map[y][x] = '.';

                    continue;
                }

                map[y][x] = input[y][x];
            }
        }

        return (map, height, width, goalPositionX, goalPositionY, startPositionX, startPositionY);
    }
   
    static List<((int posX, int posY)[] path, int cost)> foundPaths;

    static int FindAllMinimumPathLengths(char[][] map, int height, int width,
                               int goalPositionX, int goalPositionY, 
                               int currentPositionX, int currentPositionY, Heading heading,
                               int currentCost,
                               (int posX, int posY)[] path,
                               long minimumCost)
    {
        if (currentPositionX < 0 || currentPositionX >= width || currentPositionY < 0 || currentPositionY >= height)
        {
            return int.MaxValue;
        }

        if (currentCost > minimumCost)
        {
            return int.MaxValue;
        }

        if (currentPositionX == goalPositionX && currentPositionY == goalPositionY)
        {
            foundPaths.Add((path, currentCost));
            
            return currentCost;
        }

        if (map[currentPositionY][currentPositionX] == '#')
        {
            return int.MaxValue;
        }

        if (costmap.ContainsKey((currentPositionY, currentPositionX, heading)))
        {
            if (costmap[(currentPositionY, currentPositionX, heading)] < currentCost)
            {
                return int.MaxValue;
            }

            costmap[(currentPositionY, currentPositionX, heading)] = currentCost;
        }
        else
        {
            costmap.Add((currentPositionY, currentPositionX, heading), currentCost);
        }
        
        var minimumPath = int.MaxValue;

        var nextMoveForwardPositionX = currentPositionX;
        var nextMoveForwardPositionY = currentPositionY;

        if (heading == Heading.North)
        {
            nextMoveForwardPositionY--;
        }

        if (heading == Heading.South)
        {
            nextMoveForwardPositionY++;
        }

        if (heading == Heading.West)
        {
            nextMoveForwardPositionX--;
        }

        if (heading == Heading.East)
        {
            nextMoveForwardPositionX++;
        }

        var nextMoveForwardHeadingCW = heading switch 
        {
            Heading.North => Heading.East,
            Heading.East => Heading.South,
            Heading.South => Heading.West,
            Heading.West => Heading.North,
            _ => throw new Exception()
        };

        var nextMoveForwardHeadingCCW = heading switch 
        {
            Heading.North => Heading.West,
            Heading.West => Heading.South,
            Heading.South => Heading.East,
            Heading.East => Heading.North,
            _ => throw new Exception()
        };

        var forwardArray = new (int posX, int posY)[path.Length + 1];

        path.CopyTo(forwardArray, 0);

        forwardArray[path.Length] = (nextMoveForwardPositionX, nextMoveForwardPositionY);

        minimumPath = Math.Min(minimumPath, FindAllMinimumPathLengths(
                map, height, width,
                goalPositionX, goalPositionY,
                nextMoveForwardPositionX, nextMoveForwardPositionY, heading,
                currentCost + 1,
                forwardArray,
                minimumCost));

        minimumPath = Math.Min(minimumPath, FindAllMinimumPathLengths(
                map, height, width,
                goalPositionX, goalPositionY,
                currentPositionX, currentPositionY, nextMoveForwardHeadingCW,
                currentCost + 1000,
                path,
                minimumCost));

        minimumPath = Math.Min(minimumPath, FindAllMinimumPathLengths(
                map, height, width,
                goalPositionX, goalPositionY,
                currentPositionX, currentPositionY, nextMoveForwardHeadingCCW,
                currentCost + 1000,
                path,
                minimumCost));

        return minimumPath;
    }
}