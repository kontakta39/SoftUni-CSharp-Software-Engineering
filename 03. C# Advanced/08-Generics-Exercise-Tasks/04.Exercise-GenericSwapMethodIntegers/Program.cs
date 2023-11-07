//4 Exercise - Generic Swap Method Integers
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

        int[] swapElementsNumbers = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        currentNumber.Swap(swapElementsNumbers[0], swapElementsNumbers[1]);

        Console.WriteLine(currentNumber.ToString());
    }
}
