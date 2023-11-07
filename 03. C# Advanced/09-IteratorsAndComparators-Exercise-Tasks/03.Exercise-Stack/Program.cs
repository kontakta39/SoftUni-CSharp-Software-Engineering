//3 Exercise - Stack
using StackType;

List<int> numbers = new();
StackStructure<int> stack = new(numbers);

string command = Console.ReadLine();

while (command != "END")
{
    string[] delimeters = { ", ", " " };
    string[] commandInfo = command
        .Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
    string operation = commandInfo[0];

    if (operation == "Push")
    {
        numbers = stack.Push(numbers, commandInfo);
    }

    else
    {
        try
        {
            numbers = stack.Pop(numbers);
        }

        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    command = Console.ReadLine();
}

stack = new(numbers);

foreach (var item in stack)
{
    Console.WriteLine(item);
}

foreach (var item in stack)
{
    Console.WriteLine(item);
}