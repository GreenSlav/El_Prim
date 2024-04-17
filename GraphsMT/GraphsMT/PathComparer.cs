namespace GraphsMT;

class PathComparer : IComparer<Path>
{
    public int Compare(Path x, Path y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Невозможно сравнить null объекты");
        }

        return x.Distance.CompareTo(y.Distance);
    }
}