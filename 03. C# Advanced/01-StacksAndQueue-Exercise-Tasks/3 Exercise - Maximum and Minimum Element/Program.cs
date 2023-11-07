//3 Exercise - Maximum and Minimum Element
int queriesCount = int.Parse(Console.ReadLine());
Stack<int> numbers = new();

for (int i = 1; i <= queriesCount; i++)
{
    int[] operationInfo = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    if (operationInfo[0] == 1)
    {
        numbers.Push(operationInfo[1]);
    }

    else if (operationInfo[0] == 2)
    {
        numbers.Pop();
    }

    else if (operationInfo[0] == 3)
    {
        if (numbers.Count > 0)
        {
            Console.WriteLine(numbers.Max());
        }
    }

    else if (operationInfo[0] == 4)
    {
        if (numbers.Count > 0)
        {
            Console.WriteLine(numbers.Min());
        }
    }
}

Console.WriteLine(string.Join(", ", numbers));
