namespace GraphsMT;

public class GraphSolver
{
    private int[,]? _matrix;
    //private int[,] _resultMatrix;
    private int[][]? _arrayOfArrays;
    //private int[][] _resultArrayOfArrays;
    
    public GraphSolver(int[,]? matrix)
    {
        if (matrix is null || matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new Exception("Inccorect matrix in constructor!");
        }
        
        _matrix = matrix;
        //_resultMatrix = new int[matrix.GetLength(0),matrix.GetLength(1)];
    }

    public GraphSolver(int[][]? arrayOfArrays)
    {
        if (arrayOfArrays is null || arrayOfArrays.Length != arrayOfArrays[0].Length)
        {
            throw new Exception("Inccorect matrix in constructor!");
        }

        int lastLength = arrayOfArrays[0].Length;
        for (int i = 1; i < arrayOfArrays.Length; i++)
        {
            if (arrayOfArrays[i].Length != lastLength)
            {
                throw new Exception("Length of subarrays is different!");
            }
        }
        
        _arrayOfArrays = arrayOfArrays;
    }

    // public void SolveWithPrim()
    // {
    //     if (_matrix is null)
    //     {
    //         if (_arrayOfArrays is not null)
    //             Solve(_arrayOfArrays!);
    //     }
    //     else if (_arrayOfArrays is null)
    //     {
    //         if (_matrix is not null)
    //             Solve(_matrix!);
    //     }
    //
    //     throw new Exception("Both sources are null!");
    // }
    
    
    

    
    // !!!
    // протестировать бы надо метод
    public int[,] Solve(int[,] matrix)
    {
        var resultMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
        
        // заметка: на самом деле как будто бы в качестве первого элемента можно взять любую вершину
        // все равно же рано или поздно до нее доберемся
        HashSet<int> pickedVertexes = new HashSet<int>() {0};
        //bool firstVertexIsPicked = false;
        int minAvailablePathlength = int.MaxValue;
        int fromVertex = 0;
        int toVertex = 0;
        
        // находим самый первый кратчайший путь из нулевой вершины
        for (int i = 1; i < matrix.GetLength(0); i++)
        {
            if (matrix[0, i] != 0 && matrix[0, i] < minAvailablePathlength)
            {
                minAvailablePathlength = matrix[0, i];
                toVertex = i;
            }
        }

        resultMatrix[0, toVertex] = minAvailablePathlength;
        resultMatrix[toVertex, 0] = minAvailablePathlength;
        pickedVertexes.Add(toVertex);
        toVertex = 0;
        minAvailablePathlength = int.MaxValue;
        
        //bool minAvailablePathlengthIsPicked = false; // true будет означать, что мы дейстивтельно выбрали это значение
        // это сделано с целью проверки в графе реально был путь int.MaxValue или нет
        // с другой стороны по сути мы на каждом этапе будем выбирать по одному значению
        // не может быть ситуции, что за один обход мы по итогу так ничего и не включим в список вершин
        
        while (pickedVertexes.Count != matrix.GetLength(0))
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i == j || matrix[i, j] == 0 || resultMatrix[i, j] != 0 || pickedVertexes.Contains(j))
                        continue;

                    if (matrix[i, j] < minAvailablePathlength && pickedVertexes.Contains(i))
                    {
                        minAvailablePathlength = matrix[i, j];
                        fromVertex = i;
                        toVertex = j;
                    }
                }
            }

            resultMatrix[fromVertex, toVertex] = minAvailablePathlength;
            resultMatrix[toVertex, fromVertex] = minAvailablePathlength;
            pickedVertexes.Add(toVertex);
            toVertex = 0;
            minAvailablePathlength = int.MaxValue;
        }


        for (int i = 0; i < resultMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < resultMatrix.GetLength(1); j++)
            {
                Console.Write(resultMatrix[i, j] + " ");
            }

            Console.WriteLine();
        }
        
        return resultMatrix;
    }

    // private int[][] Solve(int[][] array)
    // {
    //     // инициализация
    //     var resultArray = new int[array.Length][];
    //     for (int i = 0; i < array.Length; i++)
    //     {
    //         resultArray[i] = new int[array.Length];
    //     }
    //     
    //     // заметка: на самом деле как будто бы в качестве первого элемента можно взять любую вершину
    //     // все равно же рано или поздно до нее доберемся
    //     HashSet<int> pickedVertexes = new HashSet<int>() {0};
    //     //bool firstVertexIsPicked = false;
    //     int minAvailablePathlength = int.MaxValue;
    //     int fromVertex = 0;
    //     int toVertex = 0;
    //     
    //     // находим самый первый кратчайший путь из нулевой вершины
    //     for (int i = 1; i < array.Length; i++)
    //     {
    //         if (array[0][i] != 0 && array[0][i] < minAvailablePathlength)
    //         {
    //             minAvailablePathlength = array[0][i];
    //             toVertex = i;
    //         }
    //     }
    //
    //     resultArray[0][toVertex] = minAvailablePathlength;
    //     resultArray[toVertex][0] = minAvailablePathlength;
    //     pickedVertexes.Add(toVertex);
    //     toVertex = 0;
    //     minAvailablePathlength = int.MaxValue;
    //     
    //     //bool minAvailablePathlengthIsPicked = false; // true будет означать, что мы дейстивтельно выбрали это значение
    //     // это сделано с целью проверки в графе реально был путь int.MaxValue или нет
    //     // с другой стороны по сути мы на каждом этапе будем выбирать по одному значению
    //     // не может быть ситуции, что за один обход мы по итогу так ничего и не включим в список вершин
    //     
    //     while (pickedVertexes.Count != array.Length)
    //     {
    //         for (int i = 0; i < array.Length; i++)
    //         {
    //             for (int j = 0; j < array.Length; j++)
    //             {
    //                 if (i == j || resultArray[i][j] != 0 || pickedVertexes.Contains(j))
    //                     continue;
    //
    //                 if (array[i][j] < minAvailablePathlength && pickedVertexes.Contains(i))
    //                 {
    //                     minAvailablePathlength = array[i][j];
    //                     fromVertex = i;
    //                     toVertex = j;
    //                 }
    //             }
    //         }
    //
    //         resultArray[fromVertex][toVertex] = minAvailablePathlength;
    //         resultArray[toVertex][fromVertex] = minAvailablePathlength;
    //         pickedVertexes.Add(toVertex);
    //         toVertex = 0;
    //         minAvailablePathlength = int.MaxValue;
    //     }
    //
    //
    //     for (int i = 0; i < resultArray.Length; i++)
    //     {
    //         for (int j = 0; j < resultArray.Length; j++)
    //         {
    //             Console.Write(resultArray[i][j] + ' ');
    //         }
    //
    //         Console.WriteLine();
    //     }
    //     
    //     return resultArray;
    // }
}