﻿using System.Diagnostics;
using System.Text;


namespace GraphsMT;

class Program
{
    static void Main(string[] args)
    {
        int[,] adjacencyMatrix3 = {
            {0, 2, 0, 1, 4},
            {2, 0, 3, 0, 4},
            {0, 3, 0, 0, 5},
            {1, 4, 0, 0, 1},
            {4, 0, 5, 1, 0}
        };
        
        int[,] adjacencyMatrix6 = {
            {0, 3, 0, 0, 2},
            {3, 0, 8, 0, 0},
            {0, 8, 0, 1, 4},
            {0, 0, 1, 0, 6},
            {2, 0, 4, 6, 0}
        };
        
        
        int[,] adjacencyMatrix7 = {
            {0, 3, 1, 2, 2},
            {3, 0, 8, 3, 1},
            {1, 8, 0, 1, 4},
            {1, 1, 1, 0, 6},
            {2, 1, 4, 6, 0}
        };
        
        int[,] adjacencyMatrix10x10 = {
            {0, 4, 0, 0, 2, 0, 0, 0, 5, 0},
            {4, 0, 6, 0, 0, 0, 0, 0, 3, 0},
            {0, 6, 0, 8, 0, 0, 0, 0, 0, 0},
            {0, 0, 8, 0, 0, 1, 0, 0, 0, 0},
            {2, 0, 0, 0, 0, 7, 0, 0, 0, 4},
            {0, 0, 0, 1, 7, 0, 9, 0, 0, 0},
            {0, 0, 0, 0, 0, 9, 0, 2, 0, 0},
            {0, 0, 0, 0, 0, 0, 2, 0, 3, 0},
            {5, 3, 0, 0, 0, 0, 0, 3, 0, 7},
            {0, 0, 0, 0, 4, 0, 0, 0, 7, 0}
        };
        
        int[,] adjacencyMatrix20x20 = {
            {0, 5, 8, 9, 4, 2, 3, 6, 7, 10, 11, 15, 16, 18, 20, 21, 22, 23, 25, 30},
            {5, 0, 12, 14, 17, 19, 22, 25, 27, 30, 31, 32, 33, 35, 38, 40, 41, 43, 44, 46},
            {8, 12, 0, 16, 18, 21, 23, 26, 28, 32, 33, 35, 37, 40, 42, 45, 46, 48, 50, 52},
            {9, 14, 16, 0, 19, 22, 25, 27, 30, 33, 36, 38, 40, 43, 45, 48, 50, 52, 55, 57},
            {4, 17, 18, 19, 0, 24, 26, 29, 32, 35, 38, 41, 44, 47, 50, 53, 56, 58, 60, 63},
            {2, 19, 21, 22, 24, 0, 27, 30, 33, 36, 39, 42, 45, 48, 51, 54, 57, 60, 62, 65},
            {3, 22, 23, 25, 26, 27, 0, 31, 34, 37, 40, 43, 46, 49, 52, 55, 58, 61, 64, 67},
            {6, 25, 26, 27, 29, 30, 31, 0, 36, 39, 42, 45, 48, 51, 54, 57, 60, 63, 66, 69},
            {7, 27, 28, 30, 32, 33, 34, 36, 0, 40, 43, 46, 49, 52, 55, 58, 61, 64, 67, 70},
            {10, 30, 32, 33, 35, 36, 37, 39, 40, 0, 45, 48, 51, 54, 57, 60, 63, 66, 69, 72},
            {11, 31, 33, 36, 38, 39, 40, 42, 43, 45, 0, 50, 53, 56, 59, 62, 65, 68, 71, 74},
            {15, 32, 35, 38, 41, 42, 43, 45, 46, 48, 50, 0, 55, 58, 61, 64, 67, 70, 73, 76},
            {16, 33, 37, 40, 44, 45, 46, 48, 49, 51, 53, 55, 0, 60, 63, 66, 69, 72, 75, 78},
            {18, 35, 40, 43, 47, 48, 49, 51, 52, 54, 56, 58, 60, 0, 65, 68, 71, 74, 77, 80},
            {20, 38, 42, 45, 50, 51, 52, 54, 55, 57, 59, 61, 63, 65, 0, 70, 73, 76, 79, 82},
            {21, 40, 45, 48, 53, 54, 55, 57, 58, 60, 62, 64, 66, 68, 70, 0, 75, 78, 81, 84},
            {22, 41, 46, 50, 56, 57, 58, 60, 61, 63, 65, 67, 69, 71, 73, 75, 0, 80, 83, 86},
            {23, 43, 48, 52, 58, 60, 61, 63, 64, 66, 68, 70, 72, 74, 76, 78, 80, 0, 85, 88},
            {25, 44, 50, 55, 60, 62, 64, 66, 67, 69, 71, 73, 75, 77, 79, 81, 83, 85, 0, 90},
            {30, 46, 52, 57, 63, 65, 67, 69, 70, 72, 74, 76, 78, 80, 82, 84, 86, 88, 90, 0}
        };

        
        int[,] adjacencyMatrixchatgpt = {
            {0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 1, 0, 0, 0, 0, 0},
            {1, 0, 0, 0, 1, 1, 0, 0, 0, 0},
            {0, 1, 0, 0, 0, 1, 1, 0, 0, 0},
            {0, 1, 1, 0, 0, 0, 1, 1, 0, 0},
            {0, 0, 1, 1, 0, 0, 0, 0, 1, 0},
            {0, 0, 0, 1, 1, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 1, 0, 0, 0, 1, 1},
            {0, 0, 0, 0, 0, 1, 0, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 1, 1, 1, 0}
        };

        int[,] input = adjacencyMatrixchatgpt;

        StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.GetLength(0); i++)
        {
            for (int j = 0; j < input.GetLength(1); j++)
            {
                result.Append(input[i, j] + " ");
            }
        }

        Console.WriteLine(result);
        

        var s = new GraphSolver();
        bool a = s.ValidateMatrix(adjacencyMatrixchatgpt);

        
        /*
         * Ошибки:
         * к моменту вызова Min() получаю пустой список
         */
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        //GraphSolver.SolvePrl(adjacencyMatrix10x10);
        Console.WriteLine(stopwatch.Elapsed);
        stopwatch.Reset();
        
        stopwatch.Start();
        //GraphSolver.Solve(adjacencyMatrix10x10);
        Console.WriteLine(stopwatch.Elapsed);
        

        // var stopWatch = new Stopwatch();
        // stopWatch.Start();
        // GraphSolver.Solve(adjacencyMatrix3);
        // Console.WriteLine(stopWatch.Elapsed);
        // stopWatch.Reset();
        //
        // stopWatch.Start();
        // GraphSolver.Solve(adjacencyMatrix6);
        // Console.WriteLine(stopWatch.Elapsed);
        // stopWatch.Reset();
        //
        // stopWatch.Start();
        // GraphSolver.Solve(adjacencyMatrix7);
        // Console.WriteLine(stopWatch.Elapsed);
        // stopWatch.Reset();
        //
        // stopWatch.Start();
        // GraphSolver.Solve(adjacencyMatrix10x10);
        // Console.WriteLine(stopWatch.Elapsed);
        // stopWatch.Reset();
        //
        // stopWatch.Start();
        // GraphSolver.Solve(adjacencyMatrix20x20);
        // Console.WriteLine(stopWatch.Elapsed);
        // stopWatch.Reset();
    }
}
/*
 * Общие заметки:
 * в целом не совсем понимаю, зачем я вообще делал несколько методов Solve с разными аргументами
 * потому что на вход подаваться данные будут в одном формате
 * поэтому, пожалуй, обойдусь матрицой
*/