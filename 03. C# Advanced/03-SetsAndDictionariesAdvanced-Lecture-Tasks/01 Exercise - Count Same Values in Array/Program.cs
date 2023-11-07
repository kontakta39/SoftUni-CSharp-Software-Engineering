//1 Exercise - Count Same Values in Array
double[] input = Console.ReadLine()
.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(double.Parse)
.ToArray();

Dictionary<double, int> numbers = new();

for (int i = 0; i < input.Length; i++)
{
    if (!numbers.ContainsKey(input[i]))
    {
        numbers.Add(input[i], 1);
    }

    else
    {
        numbers[input[i]]++;
    }
}

foreach (var item in numbers)
{
    Console.WriteLine($"{item.Key} - {item.Value} times");
}