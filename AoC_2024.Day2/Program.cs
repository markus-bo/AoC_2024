using AoC_Toolbox;
using AoC_Toolbox.Geometry;
using AoC_Toolbox.InputParsing;
using AoC_Toolbox.Pathfinding;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var safeRoutes = 0;

        foreach (var row in input)
        {
            var levels = row.Split().Select(int.Parse).ToList();
            
            var previousLevel = levels[0];
            var direction = 0;
            var safeLevel = true;

            foreach (var level in levels.Skip(1))
            {
                if (level < previousLevel)
                {
                    if (direction == 1)
                    {
                        safeLevel = false;
                        break;
                    }

                    direction = -1;
                }
                else if (level > previousLevel)
                {
                    if (direction == -1)
                    {
                        safeLevel = false;
                        break;
                    }

                    direction = 1;
                }

                var diff = Math.Abs(level - previousLevel);

                if (diff > 3 || diff < 1)
                {
                    safeLevel = false;
                    break;
                }

                previousLevel = level;
            }

            if (safeLevel)
            {
                safeRoutes++;
            }
        }

        return safeRoutes;
    }

    static object? solutionPart2(string[] input)
    {
        var safeRoutes = 0;

        foreach (var row in input)
        {
            var levels = row.Split().Select(int.Parse).ToList();

        
            for (int i = 0; i < levels.Count; i++)
            {
                var previousLevel = i == 0 ? levels[1] : levels[0];
                var scanStart = i == 0 ? 2 : 1;
                var direction = 0;
                var safeLevel = true;

                for (int j = scanStart; j < levels.Count; j++)
                {
                    if ( i == j)
                    {
                        continue;
                    }

                    var level = levels[j];

                    if (level < previousLevel)
                    {
                        if (direction == 1)
                        {
                            safeLevel = false;
                            break;
                        }

                        direction = -1;
                    }
                    else if (level > previousLevel)
                    {
                        if (direction == -1)
                        {
                            safeLevel = false;
                            break;
                        }

                        direction = 1;
                    }

                    var diff = Math.Abs(level - previousLevel);

                    if (diff > 3 || diff < 1)
                    {
                        safeLevel = false;
                        break;
                    }

                    previousLevel = level;
                }


                if (safeLevel)
                {
                    safeRoutes++;

                    break;
                }
            }

        }

        return safeRoutes;
    }
}