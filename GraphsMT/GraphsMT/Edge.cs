namespace GraphsMT;

public class Edge : IComparable<Edge>
{
    public int SourceVertex { get; }
    public int DestinationVertex { get; }
    public int Distance { get; }
    
    public Edge(int sourceVertex, int destinationVertex, int distance)
    {
        SourceVertex = sourceVertex;
        DestinationVertex = destinationVertex;
        Distance = distance;
    }

    

    public int CompareTo(Edge? other)
    {
        if (other == null)
        {
            throw new ArgumentException("Невозможно сравнить null объект");
        }

        return this.Distance.CompareTo(other.Distance);
    }
}