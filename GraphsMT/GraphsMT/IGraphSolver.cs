namespace GraphsMT;

public interface IGraphSolver
{
    public bool ValidateMatrix(int[,]? matrix);


    public int[,] Solve(int[,] matrix);


    public int[,] SolvePrl(int[,] matrix);
}