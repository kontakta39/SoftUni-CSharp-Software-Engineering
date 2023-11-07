namespace CustomStack;

public class StackOfStrings : Stack<string>
{
    public bool IsEmpty()
    {
        return this.Count == 0;
    }

    public Stack<string> AddRange(IEnumerable<string> elements) 
    {
        foreach (var item in elements)
        {
            this.Push(item);
        }

        return this;
    }
}