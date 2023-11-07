//3 Exercise - Generic Swap Method Strings
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

        int[] swapElementsNumbers = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        currentName.Swap(swapElementsNumbers[0], swapElementsNumbers[1]);

        Console.WriteLine(currentName.ToString());
    }
}

