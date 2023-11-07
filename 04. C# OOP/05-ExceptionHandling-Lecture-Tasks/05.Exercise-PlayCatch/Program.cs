//5 Exercise - Play Catch
int[] numbers = Console.ReadLine()
    .Split()
    .Select(int.Parse)
    .ToArray();

int exceptonsCount = 3;

while (exceptonsCount != 0)
{
    string[] inputInfo = Console.ReadLine().Split();

    if (inputInfo[0] == "Replace")
    {
        try
        {
            int index = int.Parse(inputInfo[1]);
            int currentElement = int.Parse(inputInfo[2]);

            if (index < numbers[0] || index > numbers.Length - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            else
            {
                numbers[index] = currentElement;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            exceptonsCount--;
            Console.WriteLine("The index does not exist!");
        }
        catch (FormatException)
        {
            exceptonsCount--;
            Console.WriteLine("The variable is not in the correct format!");
        }
    }

    else if (inputInfo[0] == "Print")
    {
        try
        {
            int startIndex = int.Parse(inputInfo[1]);
            int endIndex = int.Parse(inputInfo[2]);

            if (startIndex < numbers[0] || startIndex > numbers.Length - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (endIndex < numbers[0] || endIndex > numbers.Length - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = startIndex; i < numbers.Length; i++)
            {
                if (i == endIndex)
                {
                    Console.Write($"{numbers[i]}");
                    Console.WriteLine();
                    break;
                }

                else
                {
                    Console.Write($"{numbers[i]}, ");
                }
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            exceptonsCount--;
            Console.WriteLine("The index does not exist!");
        }
        catch (FormatException)
        {
            exceptonsCount--;
            Console.WriteLine("The variable is not in the correct format!");
        }
    }

    else if (inputInfo[0] == "Show")
    {
        try
        {
            int index = int.Parse(inputInfo[1]);

            if (index < numbers[0] || index > numbers.Length - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            else
            {
                Console.WriteLine(numbers[index]);
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            exceptonsCount--;
            Console.WriteLine("The index does not exist!");
        }
        catch (FormatException)
        {
            exceptonsCount--;
            Console.WriteLine("The variable is not in the correct format!");
        }
    }
}

if (exceptonsCount == 0)
{
    Console.WriteLine(string.Join(", ", numbers));
}