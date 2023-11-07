namespace BoxOfT;

public class Box<T>
{
    private Stack<T> stack = new();

    int Count { get; }

    public void Add(T number)
    {
        stack.Push(number);
    }

    public T Remove()
    {
        return stack.Pop();
    }
}
