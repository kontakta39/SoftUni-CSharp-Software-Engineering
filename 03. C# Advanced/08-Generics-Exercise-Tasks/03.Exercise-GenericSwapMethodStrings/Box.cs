using System.Text;

namespace GenericBox;

public class Box<T>
{
    private T name;
    private List<T> names = new();

    public void AddName(T addName)
    {
        name = addName;
        names.Add(name);
    }

    public void Swap(int firstName, int secondName)
    {
        (names[firstName], names[secondName]) = (names[secondName], names[firstName]);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (T item in names)
        {
            sb.AppendLine($"{typeof(T)}: {item}");
        }

        return sb.ToString().TrimEnd();
    }
}
