//2 Exercise - Collection
using ListyIteratorType;

List<string> list = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Skip(1)
    .ToList();

ListyIterator<string> listyIterator = new(list);

string command = Console.ReadLine();

while (command != "END")
{
    string[] commandInfo = command
        .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string operation = commandInfo[0];

    if (operation == "Move")
    {
        Console.WriteLine(listyIterator.Move());
    }

    else if (operation == "HasNext")
    {
        Console.WriteLine(listyIterator.HasNext());
    }

    else if (operation == "Print")
    {
        try
        {
            listyIterator.Print();
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    else if (operation == "PrintAll")
    {
        //listyIterator = new(list);

        foreach (var item in listyIterator)
        {
            Console.Write(item + " ");
        }

        //Console.WriteLine(string.Join(" ", listyIterator));

        Console.WriteLine();
    }

    command = Console.ReadLine();
}
