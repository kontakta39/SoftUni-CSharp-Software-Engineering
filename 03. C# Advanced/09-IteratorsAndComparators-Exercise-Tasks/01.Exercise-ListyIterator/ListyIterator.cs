namespace ListyIteratorType;

public class ListyIterator<T>
{
    private List<T> list;
    private int index;

    public ListyIterator(List<T> list)
    {
        this.list = list;
    }

    public List<T> Create(List<T> list, T[] commandInfo)
    {
        list = commandInfo.Skip(1).ToList();
        return list;
    }

    public bool Move(List<T> list)
    {
        if (index < list.Count - 1)
        {
            index++;
            return true;
        }

        return false;
    }

    public bool HasNext(List<T> list)
    {
        return index < list.Count - 1;
    }

    public void Print(List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Invalid Operation!");
        }

        Console.WriteLine(list[index]);
    }
}