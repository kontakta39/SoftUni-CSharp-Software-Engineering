using System.Collections;

namespace StackType;

public class StackStructure<T> : IEnumerable<T>
{
    private List<T> numbers;

    public StackStructure(List<T> numbers)
    {
        this.numbers = numbers;
    }

    public List<int> Push(List<int> numbers, string[] commandInfo)
    {
        if (numbers.Count == 0)
        {
            numbers = commandInfo.Skip(1).Select(int.Parse).ToList();
        }

        else
        {
            //numbers.AddRange(commandInfo.Skip(1).Select(int.Parse).ToList());
            numbers.Add(int.Parse(commandInfo[1]));

        }

        return numbers;
    }

    public List<int> Pop(List<int> numbers)
    {
        if (numbers.Count == 0)
        {
            throw new InvalidOperationException("No elements");
        }

        numbers.RemoveAt(numbers.Count - 1);
        return numbers;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = numbers.Count - 1; i >= 0; i--)
        {
            yield return numbers[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}