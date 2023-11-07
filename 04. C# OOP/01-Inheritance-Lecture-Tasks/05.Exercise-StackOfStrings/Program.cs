//5 Exercise - Stack of Strings
using CustomStack;

public class StartUp
{
    static void Main()
    {
        StackOfStrings strings = new();

        Console.WriteLine(strings.IsEmpty());
        strings.AddRange(new string[] { "1", "2", "3", "4", "5", "6" });
        Console.WriteLine(strings.Pop());
        Console.WriteLine(strings.Pop());
        Console.WriteLine(strings.IsEmpty());
    }
}