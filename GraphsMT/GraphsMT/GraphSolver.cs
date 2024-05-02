using System.Runtime.InteropServices.ComTypes;

namespace GraphsMT;

public class GraphSolver : IGraphSolver
{
    private static List<int> pickedVertexes = new List<int>();
    private static ThreadSafeList<Edge> currentLocalMinValues = new ThreadSafeList<Edge>();
    
    public bool ValidateMatrix(int[,]? matrix)
    {
        if (matrix is null || matrix.GetLength(0) != matrix.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] < 0 || matrix[i, j] != matrix[j, i])
                {
                    int a = 0;
                    return false;
                }
            }
        }

        return true;
    }

    
    // !!!
    // протестировать бы надо метод (протестировал: робит норм :) )
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


    // именно этот метод мы и будем запускать в нескольких потоках
    // в качестве имеющихся вершин будем передавать вершины, которые мы уже имеем
    // они будут находиться в статическом массиве в классе
    // после того, как все потоки отработают, найдем минимальное значение, среди тех, что 
    // они нам предоставили после работы
    // ----------
    // минимальные текущие значение после выполнения будут добавлять в статический список
    // а уже потом хэш-сет выберет там минимальное расстояние и возьмет себе
    // осталось понять, как узнать от какой вершину пришло это значение
    private static void FindTheClosestVertex(int[,] matrix, int vertex) // отсчет вершин начинается с нуля (v=0,1,2...)
    {
        int minValue = int.MaxValue;
        int destinationVertex = 0;
        
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            if (pickedVertexes.Contains(i) || matrix[vertex, i] == 0)
                continue;

            if (matrix[vertex, i] < minValue)
            {
                minValue = matrix[vertex, i];
                destinationVertex = i;
            }
        }

        currentLocalMinValues.Add(new Edge(vertex, destinationVertex, minValue));
    }
    
    
    // нужен метод для нахождения вершин, что пока отсутствуют в pickedVertexes

    public int[,] SolvePrl(int[,] matrix)
    {
        int[,] resultMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
        
        pickedVertexes.Add(0); // добавляем самую первую вершину 

        while (pickedVertexes.Count != matrix.GetLength(0))
        {
            Task[] taskArray = new Task[pickedVertexes.Count];
            
            for (int i = 0; i < pickedVertexes.Count; i++)
            {
                int j = i; // избегаем замыкания
                taskArray[i] = new Task(() => FindTheClosestVertex(matrix, pickedVertexes[j]));
                taskArray[i].Start();
            }

            // ждем окончания всех тасок
            Task.WaitAll(taskArray);
            
            
            // все, на этом этапе мы знаем вершину с минимальным расстоянием до нее

            var minPath = currentLocalMinValues.Min();
            currentLocalMinValues.Clear();
            pickedVertexes.Add(minPath.DestinationVertex);
            resultMatrix[minPath.SourceVertex, minPath.DestinationVertex] = minPath.Distance;
            resultMatrix[minPath.DestinationVertex, minPath.SourceVertex] = minPath.Distance;
        }
        
        for (int i = 0; i < resultMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < resultMatrix.GetLength(1); j++)
            {
                Console.Write(resultMatrix[i, j] + " ");
            }

            Console.WriteLine();
        }
        
        // очищаем коллекции после работы
        pickedVertexes.Clear();
        currentLocalMinValues.Clear();

        return resultMatrix;
    }
}