//3 Exercise - Largest 3 Numbers
List<int> numbers = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse).ToList());

numbers = numbers.OrderByDescending(x => x).ToList();

for (int i = 0; i < numbers.Count; i++)
{
    Console.Write(numbers[i] + " ");

    if (i == 2)
    {
        break;
    }
}

