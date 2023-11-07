namespace CustomRandomList;

public class RandomList : List<string>
{
    private Random random = new Random();

    public string RandomString()
    {
        int index = random.Next(0, Count);
        string removedElement = this[index];
        this.RemoveAt(index);
        return removedElement;
    }
}