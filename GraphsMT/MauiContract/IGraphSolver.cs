namespace MauiContract;

public interface IGraphSolver
{
    /// <summary>
    /// Проверяет валидность матрцицы
    /// </summary>
    /// <param name="matrix">Матрица смежности графа</param>
    /// <returns>Возвращает валидность графа</returns>
    public bool ValidateMatrix(int[,]? matrix);


    /// <summary>
    /// Решает задачу в однопоточной манере
    /// </summary>
    /// <param name="matrix">Матрица смежности графа</param>
    /// <returns>Возвращает матрицу смежности в виде ответа</returns>
    public int[,] Solve(int[,] matrix);


    /// <summary>
    /// Решает задачу в параллельной манере
    /// </summary>
    /// <param name="matrix">Матрица смежности графа</param>
    /// <returns>Возвращает матрицу смежности в виде ответа</returns>
    public int[,] SolvePrl(int[,] matrix);
}