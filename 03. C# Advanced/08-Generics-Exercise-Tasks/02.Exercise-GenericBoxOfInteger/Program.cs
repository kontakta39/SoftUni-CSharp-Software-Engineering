//2 Exercise - Generic Box of Integer
using GenericBox;

public class StartUp
{
    static void Main()
    {
        Box<int> currentNumber = new();
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            int addNumber = int.Parse(Console.ReadLine());
            currentNumber.AddName(addNumber);
        }

        Console.WriteLine(currentNumber.ToString());
    }
}
