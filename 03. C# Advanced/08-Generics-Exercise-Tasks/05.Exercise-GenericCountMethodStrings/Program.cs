//5 Exercise - Generic Count Method Strings
using GenericCount;

public class StartUp
{
    static void Main()
    {
        Box<string> currentElement = new();
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            string addElement = Console.ReadLine();
            currentElement.AddName(addElement);
        }

        string elementToCompare = Console.ReadLine();

        Console.WriteLine(currentElement.Compare(elementToCompare));
    }
}

