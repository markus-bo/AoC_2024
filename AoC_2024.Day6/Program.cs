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
    enum DirectionEnum
    {
        Up,
        Down,
        Left,
        Right
    }

    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var map = input.To2DCharArray();

        var width = map.GetLength(0);
        var height = map.GetLength(1);

        var guardPosition = new Point(0, 0);
        var guardDirection = DirectionEnum.Up;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[y,x] == '^')
                {
                    guardPosition = new Point(x, y);
                }
            }
        }

        var visitedMap = new bool[height, width];

        while(true)
        {
            visitedMap[guardPosition.Y, guardPosition.X] = true;

            if (guardDirection == DirectionEnum.Up)
            {
                if (guardPosition.Y == 0)
                {
                    break;
                }
                else if (map[guardPosition.Y - 1, guardPosition.X] == '#')
                {
                    guardDirection = DirectionEnum.Right;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X, guardPosition.Y - 1);
                }
            }

            if (guardDirection == DirectionEnum.Down)
            {
                if (guardPosition.Y == height - 1)
                {
                    break;
                }
                else if (map[guardPosition.Y + 1, guardPosition.X] == '#')
                {
                    guardDirection = DirectionEnum.Left;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X, guardPosition.Y + 1);
                }
            }

            if (guardDirection == DirectionEnum.Left)
            {
                if (guardPosition.X == 0)
                {
                    break;
                }
                else if (map[guardPosition.Y, guardPosition.X - 1] == '#')
                {
                    guardDirection = DirectionEnum.Up;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X - 1, guardPosition.Y);
                }
            }

            if (guardDirection == DirectionEnum.Right)
            {
                if (guardPosition.X == width - 1)
                {
                    break;
                }
                else if (map[guardPosition.Y, guardPosition.X + 1] == '#')
                {
                    guardDirection = DirectionEnum.Down;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X + 1, guardPosition.Y);
                }
            }
        }

        var visitedArea = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (visitedMap[y,x])
                {
                    visitedArea++;
                }
            }
        }

        return visitedArea;
    }

    static object? solutionPart2(string[] input)
    {
        var map = input.To2DCharArray();

        var width = map.GetLength(0);
        var height = map.GetLength(1);

        var guardPosition = new Point(0, 0);
        var guardDirection = DirectionEnum.Up;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[y,x] == '^')
                {
                    guardPosition = new Point(x, y);
                }
            }
        }

        var track = GetTrack(map, height, width, guardPosition, guardDirection);

        var result = 0;

        var checkedCandidatePositions = new HashSet<Point>();

        foreach(var cell in track.track)
        {
            Point? candidate = null;

            if (cell.guardDirection == DirectionEnum.Up && cell.point.Y > 0)
            {
                candidate = new Point(cell.point.X, cell.point.Y - 1);
            }
            else if (cell.guardDirection == DirectionEnum.Down && cell.point.Y < height - 1)
            {
                candidate = new Point(cell.point.X, cell.point.Y + 1);
            }
            else if (cell.guardDirection == DirectionEnum.Left && cell.point.X > 0)
            {
                candidate = new Point(cell.point.X - 1, cell.point.Y);
            }
            else if (cell.guardDirection == DirectionEnum.Right && cell.point.X < width - 1)
            {
                candidate = new Point(cell.point.X + 1, cell.point.Y);
            }
            else
            {
                continue;
            }

            if (map[candidate.Y, candidate.X] == '#' || map[candidate.Y, candidate.X] == '^')
            {
                continue;
            }

            if (checkedCandidatePositions.Contains(candidate))
            {
                continue;
            }

            checkedCandidatePositions.Add(candidate);

            map[candidate.Y, candidate.X] = '#';

            var modifiedTrack = GetTrack(map, height, width, cell.point, cell.guardDirection);

            if (modifiedTrack.isLooping)
            {
                result++;
            }

            map[candidate.Y, candidate.X] = '.';
        }

        return result;
    }


    private static (bool isLooping, List<(Point point, DirectionEnum guardDirection)> track) GetTrack(char[,] map, int height, int width, Point guardPosition, DirectionEnum guardDirection)
    {
        var visitedCells = new HashSet<(Point point, DirectionEnum guardDirection)>();

        var isLooping = false;

        while(true)
        {
            if (visitedCells.Contains((guardPosition, guardDirection)))
            {
                isLooping = true;
                break;
            }

            visitedCells.Add((point: guardPosition, guardDirection: guardDirection));

            if (guardDirection == DirectionEnum.Up)
            {
                if (guardPosition.Y == 0)
                {
                    break;
                }
                else if (map[guardPosition.Y - 1, guardPosition.X] == '#')
                {
                    guardDirection = DirectionEnum.Right;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X, guardPosition.Y - 1);
                }
            }
            else if (guardDirection == DirectionEnum.Down)
            {
                if (guardPosition.Y == height - 1)
                {
                    break;
                }
                else if (map[guardPosition.Y + 1, guardPosition.X] == '#')
                {
                    guardDirection = DirectionEnum.Left;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X, guardPosition.Y + 1);
                }
            }
            else if (guardDirection == DirectionEnum.Left)
            {
                if (guardPosition.X == 0)
                {
                    break;
                }
                else if (map[guardPosition.Y, guardPosition.X - 1] == '#')
                {
                    guardDirection = DirectionEnum.Up;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X - 1, guardPosition.Y);
                }
            }
            else if (guardDirection == DirectionEnum.Right)
            {
                if (guardPosition.X == width - 1)
                {
                    break;
                }
                else if (map[guardPosition.Y, guardPosition.X + 1] == '#')
                {
                    guardDirection = DirectionEnum.Down;
                }
                else 
                {
                    guardPosition = new Point(guardPosition.X + 1, guardPosition.Y);
                }
            }
        }

        return (isLooping, visitedCells.ToList());
    }
}