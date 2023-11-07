using System.Text;

namespace GenericCount;

public class Box<T> where T : IComparable<T>
{
    private T name;
    private List<T> names = new();

    public void AddName(T addName)
    {
        name = addName;
        names.Add(name);
    }

    public string Compare(T elementToCompare)
    {
        int count = 0;

        foreach (T item in names)
        {
            int result = item.CompareTo(elementToCompare);

            if (result > 0)
            {
                count++;
            }
        }

        return count.ToString();
    }
}
