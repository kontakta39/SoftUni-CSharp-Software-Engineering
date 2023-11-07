//1 Exercise - Generic Box of String
using GenericBox;

public class StartUp
{
    static void Main()
    {
        Box<string> currentName = new();
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            string addName = Console.ReadLine();
            currentName.AddName(addName);
        }

        Console.WriteLine(currentName.ToString());
    }
}
