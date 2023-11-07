//4 Exercise - Sum of Integers
string[] numbersInput = Console.ReadLine()
    .Split();
int sum = 0;

foreach (var item in numbersInput)
{
    try
    {
        int currentNumber = int.Parse(item);
        sum += currentNumber;
    }
    catch (FormatException)
    {
        Console.WriteLine($"The element '{item}' is in wrong format!");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"The element '{item}' is out of range!");
    }
    finally
    {
        Console.WriteLine($"Element '{item}' processed - current sum: {sum}");
    }
}

Console.WriteLine($"The total sum of all integers is: {sum}");