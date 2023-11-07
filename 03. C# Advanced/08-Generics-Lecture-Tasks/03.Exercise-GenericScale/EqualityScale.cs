namespace GenericScale;

public class EqualityScale<T> where T : IComparable<T>
{
    private T firstElement;
    private T secondElement;
    public EqualityScale(T left, T right)
    {
        firstElement = left;
        secondElement = right;
    }

    public bool AreEqual()
    {
        bool ifAreEqual = false;

        if (firstElement.CompareTo(secondElement) == 0)
        {
            ifAreEqual = true;
        }

        else
        {
            ifAreEqual = false;
        }

        return ifAreEqual;
    }
}
