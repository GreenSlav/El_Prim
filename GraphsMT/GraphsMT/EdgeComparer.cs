namespace GraphsMT;

class EdgeComparer : IComparer<Edge>
{
    public int Compare(Edge x, Edge y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Невозможно сравнить null объекты");
        }

        return x.Distance.CompareTo(y.Distance);
    }
}