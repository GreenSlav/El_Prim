namespace GraphsMT;

public class Path : IComparable<Path>
{
    public int SourceVertex { get; }
    public int DestinationVertex { get; }
    public int Distance { get; }
    
    public Path(int sourceVertex, int destinationVertex, int distance)
    {
        SourceVertex = sourceVertex;
        DestinationVertex = destinationVertex;
        Distance = distance;
    }

    

    public int CompareTo(Path? other)
    {
        if (other == null)
        {
            throw new ArgumentException("Невозможно сравнить null объект");
        }

        return this.Distance.CompareTo(other.Distance);
    }
}