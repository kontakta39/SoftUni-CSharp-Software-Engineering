using System.Text;

namespace GenericBox;

public class Box<T>
{
    private T number;
    private List<T> numbers = new();

    public void AddName(T addName)
    {
        number = addName;
        numbers.Add(number);
    }

    public void Swap(int firstName, int secondName)
    {
        (numbers[firstName], numbers[secondName]) = (numbers[secondName], numbers[firstName]);
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
