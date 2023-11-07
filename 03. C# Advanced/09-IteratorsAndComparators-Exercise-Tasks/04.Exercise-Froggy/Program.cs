//4 Exercise - Froggy
using Froggy;

Lake lake = new();

int[] numbers = Console.ReadLine()
    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray(); 

lake.Add(numbers);

Console.WriteLine(string.Join(", ", lake));