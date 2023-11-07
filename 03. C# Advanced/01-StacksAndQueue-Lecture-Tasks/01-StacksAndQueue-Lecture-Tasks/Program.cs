//1 Exercise - Reverse a String
//string input = Console.ReadLine();
//Stack<string> text = new Stack<string>();

//foreach (var item in input)
//{
//    text.Push(item.ToString());
//}

//while (text.Any())
//{
//    Console.Write(text.Pop());
//}

//2 Exercise - Stack Sum
//int[] inputNumbers = Console.ReadLine().Split().Select(int.Parse).ToArray();
//Stack<int> numbers = new Stack<int>(inputNumbers);

//string command = Console.ReadLine().ToLower();

//while (command != "end")
//{
//    string[] commandInfo = command.Split();
//    string operation = commandInfo[0];

//    if (operation == "add")
//    {
//        int numberOne = int.Parse(commandInfo[1]);
//        int numberTwo = int.Parse(commandInfo[2]);

//        numbers.Push(numberOne);
//        numbers.Push(numberTwo);
//    }

//    else if (operation == "remove")
//    { 
//    int numbersCount = int.Parse(commandInfo[1]);

//        if (numbersCount > numbers.Count)
//        {
//            command = Console.ReadLine().ToLower();
//            continue;
//        }

//        else 
//        {
//            while (numbersCount != 0)
//            {
//                numbers.Pop();
//                numbersCount--;
//            }
//        }
//    }

//    command = Console.ReadLine().ToLower();
//}

//int result = 0;

//foreach (var item in numbers)
//{
//    result += item;
//}

//Console.WriteLine($"Sum: {result}");

//3 Exercise - Simple Calculator
//string[] input = Console.ReadLine()
//    .Split(" ", StringSplitOptions.RemoveEmptyEntries);
//Array.Reverse(input);
//Stack<string> numbers = new(input);

//int result = int.Parse(numbers.Pop());
//string currentSymbol = numbers.Pop();

//while (true)
//{
//    if (currentSymbol == "+")
//    {
//        int currentNumber = int.Parse(numbers.Pop());
//        result += currentNumber;
//    }

//    else if (currentSymbol == "-")
//    {
//        int currentNumber = int.Parse(numbers.Pop());
//        result -= currentNumber;
//    }

//    if (numbers.Count == 0)
//    {
//        break;
//    }

//    currentSymbol = numbers.Pop();
//}

//Console.WriteLine(result);

//4 Exercise - Matching Brackets
//string input = Console.ReadLine();
//Stack<int> openingScopes = new();

//for (int i = 0; i < input.Length; i++)
//{
//    if (input[i] == '(')
//    {
//        openingScopes.Push(i);
//    }

//    else if (input[i] == ')')
//    {
//        int currentOpeningScopeIndex = openingScopes.Pop();
//        int currentClosingScopeIndex = i;
//        int length = currentClosingScopeIndex - currentOpeningScopeIndex + 1;
//        string result = input.Substring(currentOpeningScopeIndex, length);
//        Console.WriteLine(result);
//    }
//}

//5 Exercise - Print Even Numbers
//Queue<int> numbers = new(Console.ReadLine()
//    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
//Queue<int> evenNumbers = new();
//int currentNumber = 0;

//while (numbers.Any())
//{
//    currentNumber = numbers.Dequeue();

//    if (currentNumber % 2 == 0)
//    {
//        evenNumbers.Enqueue(currentNumber);
//    }
//}

//Console.WriteLine(string.Join(", ", evenNumbers));

//6 Exercise - Supermarket
//string input = Console.ReadLine();
//Queue<string> names = new();
//int namesCount = 0;

//while (input != "End")
//{
//    if (input != "Paid")
//    {
//        names.Enqueue(input);
//    }

//    else if (input == "Paid")
//    {
//        while (names.Count != 0)
//        {
//            Console.WriteLine(names.Dequeue());
//        }
//    }

//    input = Console.ReadLine();
//}

//foreach (var item in names)
//{
//    namesCount++;
//}

//Console.WriteLine($"{namesCount} people remaining.");

//7 Exercise - Hot Potato
//Queue<string> names = new(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries));
//int count = int.Parse(Console.ReadLine());

//while (names.Count != 1)
//{
//    for (int i = 1; i <= count; i++)
//    {
//        if (i == count)
//        {
//            Console.WriteLine($"Removed {names.Dequeue()}");
//        }

//        else
//        {
//            string currentName = names.Dequeue();
//            names.Enqueue(currentName);
//        }
//    }
//}

//Console.WriteLine($"Last is {string.Join(" ", names)}");

//8 Exercise - Traffic Jam
int carPassNumber = int.Parse(Console.ReadLine());
string input = Console.ReadLine();
Queue<string> cars = new();
int passedCarsCount = 0;

while (input != "end")
{
    if (input != "green")
    {
        cars.Enqueue(input);
    }

    else if (input == "green")
    {
        for (int i = 1; i <= carPassNumber; i++)
        {
            if (cars.Count == 0)
            {
                break;
            }

            Console.WriteLine($"{cars.Dequeue()} passed!");
            passedCarsCount++;
        }
    }

    input = Console.ReadLine();
}

Console.WriteLine($"{passedCarsCount} cars passed the crossroads.");