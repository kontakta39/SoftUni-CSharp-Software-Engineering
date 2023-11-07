//2 Exercise - Basic Queue Operations
int[] operationInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int[] numbersInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();

Queue<int> numbers = new();

int enqueueCount = operationInfo[0];
int dequeueCount = operationInfo[1];
int elementCheck = operationInfo[2];

for (int i = 0; i < enqueueCount; i++)
{
    numbers.Enqueue(numbersInfo[i]);
}

for (int i = 0; i < dequeueCount; i++)
{
    numbers.Dequeue();
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
