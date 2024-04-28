using System.Collections;

namespace GraphsMT;

public interface IThreadSafeList<T>
{
    public void Sort(IComparer<T> comparer);

    public void Sort(Comparison<T> comparison);

    public T Min();

    public T Max();

    public void Add(T item);

    public void Clear();

    public bool Contains(T item);

    public void CopyTo(T[] array, int arrayIndex);

    public bool Remove(T item);
    
    public bool IsReadOnly { get; }

    public int IndexOf(T item);

    public void Insert(int index, T item);

    public void RemoveAt(int index);
}