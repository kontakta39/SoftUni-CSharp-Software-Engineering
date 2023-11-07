//4 Exercise - Even Times
int numbersCount = int.Parse(Console.ReadLine());
Dictionary<int, int> numbers = new();

for (int i = 0; i < numbersCount; i++)
{
    int currentNumber = int.Parse(Console.ReadLine());

    if (!numbers.ContainsKey(currentNumber))
    {
        numbers.Add(currentNumber, 1);
    }

    else
    {
        numbers[currentNumber]++;
    }

}

foreach (var item in numbers)
{
    if (item.Value % 2 == 0)
    {
        Console.WriteLine(item.Key);
    }
}

