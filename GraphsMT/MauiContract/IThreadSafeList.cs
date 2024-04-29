using System.Collections;

namespace MauiContract;

public interface IThreadSafeList<T> : IEnumerable<T>, IList<T>
{
    /// <summary>
    /// Сравниватель
    /// </summary>
    /// <param name="comparer">Сравниваемое</param>
    public void Sort(IComparer<T> comparer);

    /// <summary>
    /// Метод для нахождения элемента минимального значения
    /// </summary>
    /// <returns>Минимальное значение в коллекции</returns>
    public T Min();

    /// <summary>
    /// Метод для нахождения максимального элемента коллекции
    /// </summary>
    /// <returns>Максимальное значение в коллекции</returns>    
    public T Max();

    /// <summary>
    /// Метод для добавления элемента в коллекцию
    /// </summary>
    /// <param name="item">Значение к добавлению</param>
    /// <returns></returns>
    public void Add(T item);

    /// <summary>
    /// Метод для очистки коллекции
    /// </summary>
    /// <returns></returns>
    public void Clear();

    /// <summary>
    /// Метод для проверки содержания элемента в коллекции
    /// </summary>
    /// <returns>Возвращает true, если элемент найден</returns>
    public bool Contains(T item);

    /// <summary>
    /// Метод для копиравания массива начиная с какого-то индекса
    /// </summary>
    /// <param name="array">В какой массив копируем</param>
    /// <param name="arrayIndex">Начиная с какого индекса копируем</param>
    /// <returns></returns>
    public void CopyTo(T[] array, int arrayIndex);

    /// <summary>
    /// Метод для удаления элемента из коллекции по значению
    /// </summary>
    /// <param name="item">Значение к удалению</param>
    /// <returns>Успешность удаления</returns>
    public bool Remove(T item);
    
    /// <summary>
    /// Коллекция только для чтения
    /// </summary>
    /// <returns>true, если только для чтения</returns>
    public bool IsReadOnly { get; }

    /// <summary>
    /// Метод для поиска индекса элемента
    /// </summary>
    /// <param name="item">Значение к поиску</param>
    /// <returns>Индекс элемента</returns>
    public int IndexOf(T item);

    /// <summary>
    /// Метод для вставки элемента в коллекцию
    /// </summary>
    /// <param name="index">Индекс на вставку</param>
    /// <param name="item">Значение к вставке</param>
    /// <returns></returns>
    public void Insert(int index, T item);

    /// <summary>
    /// Метод для удаления элемента по индексу
    /// </summary>
    /// <param name="index">Значение к вставке</param>
    /// <returns></returns>
    public void RemoveAt(int index);
}