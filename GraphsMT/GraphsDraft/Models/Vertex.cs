namespace Graphs.Models;

public class Vertex
{
    public HashSet<Vertex> ComesTo = new();
    public int Number { get; }
    public int X { get; set; }
    public int Y { get; set; }

    public Vertex(int number, int x, int y)
    {
        Number = number;
        X = x;
        Y = y;
    }
}