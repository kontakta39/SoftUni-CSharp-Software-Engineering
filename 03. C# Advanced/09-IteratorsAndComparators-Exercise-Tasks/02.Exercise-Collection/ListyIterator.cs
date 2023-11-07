using System.Collections;

namespace ListyIteratorType;

public class ListyIterator<T> : IEnumerable<T>
{
    private List<T> list;
    private int index;

    public ListyIterator(List<T> list)
    {
        this.list = list;
    }

    public bool Move()
    {
        if (index < list.Count - 1)
        {
            index++;
            return true;
        }

        return false;
    }

    public bool HasNext()
    {
        return index < list.Count - 1;
    }

    public void Print()
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Invalid Operation!");
        }

        Console.WriteLine(list[index]);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in list)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}