//1 Exercise - Listy Iterator
using ListyIteratorType;

List<string> list = new();
ListyIterator<string> listyIterator = new(list);

string command  = Console.ReadLine();

while (command != "END")
{
    string[] commandInfo = command
        .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string operation = commandInfo[0];

    if (operation == "Create")
    {
        list = listyIterator.Create(list, commandInfo);
    }

    else if (operation == "Move")
    {
        Console.WriteLine(listyIterator.Move(list));
    }

    else if (operation == "HasNext")
    {
        Console.WriteLine(listyIterator.HasNext(list));
    }

    else if (operation == "Print")
    {
        try
        {
            listyIterator.Print(list);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    command = Console.ReadLine();
}