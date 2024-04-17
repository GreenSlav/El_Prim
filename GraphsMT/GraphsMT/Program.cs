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

        
        var solver = new GraphSolver(adjacencyMatrix3);

        solver.Solve(adjacencyMatrix3);
    }
}
/*
 * Общие заметки:
 * в целом не совсем понимаю, зачем я вообще делал несколько методов Solve с разными аргументами
 * потому что на вход подаваться данные будут в одном формате
 * поэтому, пожалуй, обойдусь матрицой
*/