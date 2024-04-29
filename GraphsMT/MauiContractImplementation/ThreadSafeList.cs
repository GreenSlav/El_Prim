using System.Collections;
using MauiContract;

namespace MauiContractImplementation;

public class ThreadSafeList<T> : IThreadSafeList<T>
{
    private readonly List<T> _list = new List<T>();
    private readonly object _lock = new object();
    
    
    public void Sort(IComparer<T> comparer)
    {
        lock (_lock)
        {
            _list.Sort(comparer);
        }
    }
    


    public T Min()
    {
        lock (_lock)
        {
            if (_list.Count == 0)
            {
                throw new InvalidOperationException("Список пуст");
            }

            return _list.Min();
        }
    }

    public T Max()
    {
        lock (_lock)
        {
            
            if (_list.Count == 0)
            {
                throw new InvalidOperationException("Список пуст");
            }

            return _list.Max();
        }
    }


    public IEnumerator<T> GetEnumerator()
    {
        lock (_lock)
        {
            return _list.GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            _list.Add(item);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _list.Clear();
        }
    }

    public bool Contains(T item)
    {
        lock (_lock)
        {
            return _list.Contains(item);
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (_lock)
        {
            _list.CopyTo(array, arrayIndex);
        }
    }

    public bool Remove(T item)
    {
        lock (_lock)
        {
            return _list.Remove(item);
        }
    }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _list.Count;
            }
        }
    }

    public bool IsReadOnly { get; } = false;
    
    public int IndexOf(T item)
    {
        lock (_lock)
        {
            return _list.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        lock (_lock)
        {
            _list.Insert(index, item);
        }
    }

    public void RemoveAt(int index)
    {
        lock (_lock)
        {
            _list.RemoveAt(index);
        }
    }

    public T this[int index]
    {
        get
        {
            lock (_lock)
            {
                return _list[index];
            }
        }
        set
        {
            lock (_lock)
            {
                _list[index] = value;
            }
        }
    }
}