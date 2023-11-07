//1 Exercise - Basic Stack Operations
int[] operationInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int[] numbersInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();

Stack<int> numbers = new();

int pushCount = operationInfo[0];
int popCount = operationInfo[1];
int elementCheck = operationInfo[2];

for (int i = 0; i < pushCount; i++)
{
    numbers.Push(numbersInfo[i]);
}

for (int i = 0; i < popCount; i++)
{
    numbers.Pop();
}

if (numbers.Count == 0)
{
    Console.WriteLine(0);
}

else if (numbers.Contains(elementCheck))
{
    Console.WriteLine("true");
}

else
{
    Console.WriteLine(numbers.Min());
}
