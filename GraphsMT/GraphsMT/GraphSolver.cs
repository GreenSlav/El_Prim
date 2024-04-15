namespace GraphsMT;

public class GraphSolver
{
    private int[,]? _matrix;
    private int[,] _resultMatrix;
    private int[][]? _arrayOfArrays;
    private int[][] _resultArrayOfArrays;
    
    public GraphSolver(int[,]? matrix)
    {
        if (matrix is null || matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new Exception("Inccorect matrix in constructor!");
        }
        
        _matrix = matrix;
    }

    public GraphSolver(int[][]? arrayOfArrays)
    {
        if (arrayOfArrays is null || arrayOfArrays.Length != arrayOfArrays[0].Length)
        {
            throw new Exception("Inccorect matrix in constructor!");
        }
        
        _arrayOfArrays = arrayOfArrays;
    }

    public void SolveWithPrim()
    {
        if (_matrix is null)
        {
            if (_arrayOfArrays is not null)
                Solve(_arrayOfArrays!);
        }
        else if (_arrayOfArrays is null)
        {
            if (_matrix is not null)
                Solve(_matrix);
            // еще нужно проверить, что все строки этой матрицы будут одинаковой длины
        }

        throw new Exception("Both sources are null!");
    }

    private void Solve(int[,] matrix)
    {
        
    }

    private void Solve(int[][] array)
    {
        
    }
}