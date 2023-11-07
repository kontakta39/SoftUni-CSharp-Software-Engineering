//2 Exercise - Enter Numbers
List<int> numbers = new();
int lastCurrentNumber = 1;

while (numbers.Count != 10)
{
	try
	{
		int currentNumber = int.Parse(Console.ReadLine());

		if (currentNumber <= lastCurrentNumber || currentNumber >= 100)
		{
			throw new ArgumentOutOfRangeException();
		}

		lastCurrentNumber = currentNumber;
		numbers.Add(currentNumber);
	}
	catch (FormatException)
	{
		Console.WriteLine("Invalid Number!");
	}
	catch (ArgumentOutOfRangeException)
	{
		Console.WriteLine($"Your number is not in range {lastCurrentNumber} - 100!");
	}
}

Console.WriteLine(string.Join(", ", numbers));