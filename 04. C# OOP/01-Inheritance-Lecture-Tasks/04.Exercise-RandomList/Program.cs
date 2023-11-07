//4 Exercise - Random List
using CustomRandomList;

public class StartUp
{
    static void Main()
    {
        RandomList strings = new();

        strings.Add("1");
        strings.Add("2");
        strings.Add("3");
        strings.Add("4");
        strings.Add("5");
        strings.Add("6");

        Console.WriteLine(strings.RandomString());
        Console.WriteLine(strings.RandomString());
        Console.WriteLine(strings.RandomString());
    }
}