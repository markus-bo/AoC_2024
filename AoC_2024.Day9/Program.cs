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
using System.Data.SqlTypes;

internal class Solution
{
    static void Main(string[] args)
    {
        PuzzleSetup.Solve(solutionPart1, solutionPart2);
    }

    static object? solutionPart1(string[] input)
    {
        var disc = input.First()
                        .ToCharArray()
                        .Select(x => x-'0')
                        .Select((x,i) => (isSpace: i%2==1, id: i%2==0 ? (int)(i/2) : -1, length: x))
                        .ToList();

        var blockLeftIndex = 0;
        var blockRightIndex = disc.Count - 1;

        var compactedDisc = new List<(int id, int length)>();
        
        var remainingSpace = 0;
        var movedSpace = 0;

        while(blockLeftIndex < blockRightIndex)
        {
            if (remainingSpace > 0)
            {
                var moveableSpace = Math.Min(remainingSpace, disc[blockRightIndex].length - movedSpace);

                compactedDisc.Add((disc[blockRightIndex].id, moveableSpace));

                remainingSpace -= moveableSpace;

                if (moveableSpace >= (disc[blockRightIndex].length - movedSpace))
                {
                    blockRightIndex -= 2;
                    movedSpace = 0;
                }
                else
                {
                    movedSpace += moveableSpace;
                }

                continue;
            }


            if (disc[blockLeftIndex].isSpace == false)
            {
                compactedDisc.Add((disc[blockLeftIndex].id, disc[blockLeftIndex].length));
                
                blockLeftIndex += 1;
                remainingSpace = disc[blockLeftIndex].length;
                
                continue;
            }

            if (remainingSpace == 0)
            {
                blockLeftIndex += 1;
            }
        }

        if (movedSpace > 0)
        {
            compactedDisc.Add((disc[blockRightIndex].id, disc[blockRightIndex].length - movedSpace));
        }

        var hashSum = 0L;
        var index = 0L;

        foreach (var (id, length) in compactedDisc)
        {
            for (var i = index; i < index + length; i++)
            {
                hashSum += id * i;
            }
            
            index += length;
        }

        return hashSum;
    }

    static object? solutionPart2(string[] input)
    {
        var disc = input.First()
                        .ToCharArray()
                        .Select(x => x-'0')
                        .Select((x,i) => (isSpace: i%2==1, id: i%2==0 ? (int)(i/2) : -1, length: x))
                        .ToList();

        var blockLeftIndex = 0;
        var blockRightIndex = disc.Count - 1;

        while(blockLeftIndex <= blockRightIndex)
        {
            if (disc[blockLeftIndex].isSpace == false)
            {
                blockLeftIndex += 1;
                continue;
            }

            if (disc[blockRightIndex].isSpace == true)
            {
                blockRightIndex -= 1;
                continue;
            }

            var neededSpace = disc[blockRightIndex].length;

            for (int i = 0; i < blockRightIndex; i++)
            {
                if (disc[i].isSpace == false)
                {
                    continue;
                }

                if (disc[i].isSpace && disc[i].length == neededSpace)
                {
                    disc[i] = (isSpace: false, id: disc[blockRightIndex].id, length: disc[blockRightIndex].length);

                    disc[blockRightIndex] = (isSpace: true, id: -1, length: neededSpace);

                    break;
                }
                else if (disc[i].isSpace && disc[i].length > neededSpace)
                {
                    disc[i] = (isSpace: true, id: disc[i].id, length: disc[i].length - neededSpace);

                    disc.Insert(i, (isSpace: false, id: disc[blockRightIndex].id, length: neededSpace));
                    
                    disc[blockRightIndex + 1] = (isSpace: true, id: -1, length: neededSpace);

                    break;
                }
            }

            blockRightIndex -= 1;
        }

      
        var hashSum = 0L;
        var index = 0L;

        foreach (var (isSpace, id, length) in disc)
        {
            if (isSpace == false)
            {
                for (var i = index; i < index + length; i++)
                {
                    hashSum += id * i;
                }
            }
            
            index += length;
        }       
        
        return hashSum;
    }
}