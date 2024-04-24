namespace Graphs.Models;

public class Vertex
{
    public List<Vertex> ComesTo = new();
    public int Number { get; }
    public int X { get; set; }
    public int Y { get; set; }
}