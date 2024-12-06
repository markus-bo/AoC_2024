using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;
using System.Transactions;
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
        var orderingInput = new List<string>();
        var pageUpdatesInput = new List<string>();
        var readFirstBlock = true;
        var result = 0;

        foreach(var row in input)
        {
            if (row == "")
            {
                readFirstBlock = false;
                continue;
            }

            if (readFirstBlock)
            {
                orderingInput.Add(row);
            }
            else
            {
                pageUpdatesInput.Add(row);
            }
        }

        var ordering  = orderingInput.Select(x => x.Split("|"))
                                     .Select(x => (first: int.Parse(x[0]), second: int.Parse(x[1])))
                                     .ToList();

        var pageUpdates = pageUpdatesInput.Select(x => x.Split(","))
                                          .Select(x => x.Select(int.Parse).ToList())
                                          .ToList();

        foreach (var update in pageUpdates)
        {
            var validUpdate = true;

            //Console.Error.WriteLine($"Checking update: {string.Join(",", update)}");

            for (int i = 0; i < update.Count; i++)
            {
                var rulesForThisPage = ordering.Where(x => x.second == update[i])
                                               .Select(x => x.first)
                                               .ToList();

                for (int j = i + 1; j < update.Count; j++)
                {
                    if (rulesForThisPage.Contains(update[j]))   
                    {
                        validUpdate = false;
                        break;
                    }
                }

                if (validUpdate == false)
                {
                    break;
                }
            }

            if (validUpdate)
            {
                Console.Error.WriteLine($"Valid update: {string.Join(",", update)}");

                var middleNumber = update[update.Count / 2];
                result += middleNumber;
            }
        }

        return result;
    }

    static object? solutionPart2(string[] input)
    {
                var orderingInput = new List<string>();
        var pageUpdatesInput = new List<string>();
        var readFirstBlock = true;
        var result = 0;

        foreach(var row in input)
        {
            if (row == "")
            {
                readFirstBlock = false;
                continue;
            }

            if (readFirstBlock)
            {
                orderingInput.Add(row);
            }
            else
            {
                pageUpdatesInput.Add(row);
            }
        }

        var ordering  = orderingInput.Select(x => x.Split("|"))
                                     .Select(x => (first: int.Parse(x[0]), second: int.Parse(x[1])))
                                     .ToList();

        var pageUpdates = pageUpdatesInput.Select(x => x.Split(","))
                                          .Select(x => x.Select(int.Parse).ToList())
                                          .ToList();

        var invalidUpdates = new List<List<int>>();

        foreach (var update in pageUpdates)
        {
            var validUpdate = true;

            for (int i = 0; i < update.Count; i++)
            {
                var rulesForThisPage = ordering.Where(x => x.second == update[i])
                                               .Select(x => x.first)
                                               .ToList();

                for (int j = i + 1; j < update.Count; j++)
                {
                    if (rulesForThisPage.Contains(update[j]))   
                    {
                        validUpdate = false;
                        break;
                    }
                }

                if (validUpdate == false)
                {
                    break;
                }
            }

            if (validUpdate == false)
            {
                invalidUpdates.Add(update);
            }
        }

        for (int k = 0; k < invalidUpdates.Count; k++)
        {
            Console.Error.WriteLine($"Invalid update: {string.Join(",", invalidUpdates[k])}");

            var correctPageOrder = new List<int>();

            var validPage = true;

            for (int i = 0; i < invalidUpdates[k].Count; i++)
            {
                var rulesForThisPage = ordering.Where(x => x.second == invalidUpdates[k][i])
                                               .Select(x => x.first)
                                               .ToList();

                for (int j = i + 1; j < invalidUpdates[k].Count; j++)
                {
                    if (rulesForThisPage.Contains(invalidUpdates[k][j]))   
                    {
                        var pageToSwap = invalidUpdates[k][i];
                        var pageToSwapWith = invalidUpdates[k][j];
                        invalidUpdates[k][i] = pageToSwapWith;
                        invalidUpdates[k][j] = pageToSwap;

                        i--;
                        validPage = false;
                        break;
                    }
                }

                if (validPage == false)
                {
                   

                }
            }

            Console.Error.WriteLine($"  Corrected update: {string.Join(",", invalidUpdates[k])}");
            
            var middleNumber = invalidUpdates[k][invalidUpdates[k].Count / 2];
            result += middleNumber;

            Console.Error.WriteLine($"  Middle number: {middleNumber}");
        }

        return result;
    }
}