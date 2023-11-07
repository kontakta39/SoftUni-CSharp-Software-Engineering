//3 Exercise - Generic Scale
using GenericScale;

public class StartUp
{
    static void Main()
    {
        EqualityScale<int> elements = new(1, 2);
        Console.WriteLine(elements.AreEqual());
        //EqualityScale<string> elementsOne = new("aa", "aa");
        //Console.WriteLine(elementsOne.AreEqual());
    }
}

