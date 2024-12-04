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
        var result = 0;

        foreach(var row in input)
        {
            result += CountXMAS(row.ToCharArray());
            result += CountXMAS(row.Reverse().ToArray());
        }

        var tArray = TransposeArray(input.To2DCharArray());
        var tInput = new char[tArray.GetLength(0)][];

        for (int i = 0; i < tArray.GetLength(0); i++)
        {
            tInput[i] = new char[tArray.GetLength(1)];

            for (int j = 0; j < tArray.GetLength(1); j++)
            {
                tInput[i][j] = tArray[i,j];
            }
        }

        foreach(var row in tInput)
        {
            result += CountXMAS(row);
            result += CountXMAS(row.Reverse().ToArray());
        }

        for (int i = 0; i < input.Length - 3; i++)
        {
            for (int j = 0; j < input[i].Length - 3; j++)
            {
                if (input[i][j] == 'X' && input[i+1][j+1] == 'M'  && input[i+2][j+2] == 'A' && input[i+3][j+3] == 'S' )
                {
                    result++;
                }
            }

            for (int j = 3; j < input[i].Length; j++)
            {
                if (input[i][j] == 'X' && input[i+1][j-1] == 'M'  && input[i+2][j-2] == 'A' && input[i+3][j-3] == 'S' )
                {
                    result++;
                }
            }
        }

        for (int i = input.Length - 1; i >= 3; i--)
        {
            for (int j = 0; j < input[i].Length - 3; j++)
            {
                if (input[i][j] == 'X' && input[i-1][j+1] == 'M'  && input[i-2][j+2] == 'A' && input[i-3][j+3] == 'S' )
                {
                    result++;
                }
            }

            for (int j = 3; j < input[i].Length; j++)
            {
                if (input[i][j] == 'X' && input[i-1][j-1] == 'M'  && input[i-2][j-2] == 'A' && input[i-3][j-3] == 'S' )
                {
                    result++;
                }
            }
        }

        return result;
    }

    static object? solutionPart2(string[] input)
    {
        var result = 0;

        for (int i = 0; i < input.Length - 2; i++)
        {
            for (int j = 0; j < input[i].Length - 2; j++)
            {
                if (input[i][j] == 'M'      &&      input[i][j+2] == 'S'  && 
                                   input[i+1][j+1] == 'A' && 
                    input[i+2][j] == 'M'    &&      input[i+2][j+2] == 'S')
                {
                    result++;
                }

                if (input[i][j] == 'S'      &&      input[i][j+2] == 'S'  && 
                                   input[i+1][j+1] == 'A' && 
                    input[i+2][j] == 'M'    &&      input[i+2][j+2] == 'M')
                {
                    result++;
                }

                if (input[i][j] == 'S'      &&      input[i][j+2] == 'M'  && 
                                   input[i+1][j+1] == 'A' && 
                    input[i+2][j] == 'S'    &&      input[i+2][j+2] == 'M')
                {
                    result++;
                }

                if (input[i][j] == 'M'      &&      input[i][j+2] == 'M'  && 
                                   input[i+1][j+1] == 'A' && 
                    input[i+2][j] == 'S'    &&      input[i+2][j+2] == 'S')
                {
                    result++;
                }
            }
        }

        return result;
    }

    static char[,] TransposeArray(char[,] array)
    {

        var height = array.GetLength(0);
        var width = array.GetLength(1);

        var tArray = new char[width,height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                tArray[j,i] = array[i,j];
            }
        }

        return tArray;
    }

    static int CountXMAS(char[] line)
    {
        var count = 0;

        for (int i = 0; i < line.Count() - 3; i++)
        {
            if (line[i] == 'X' && line[i+1] == 'M' && line[i+2] == 'A' && line[i+3] == 'S')
            {
                count++;
            }
        }

        return count;
    }
}