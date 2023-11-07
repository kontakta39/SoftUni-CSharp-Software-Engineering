using System.Text;

namespace GenericBox;

public class Box<T>
{
    private T number;
    private List<T> numbers = new();

    public void AddName(T addNumber)
    {
        number = addNumber;
        numbers.Add(number);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (T item in numbers)
        {
            sb.AppendLine($"{typeof(T)}: {item}");
        }

        return sb.ToString().TrimEnd();
    }
}
