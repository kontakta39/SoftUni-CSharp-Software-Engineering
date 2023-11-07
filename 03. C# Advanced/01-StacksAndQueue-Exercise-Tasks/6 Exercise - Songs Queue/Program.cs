//6 Exercise - Songs Queue
Queue<string> songs = new(Console.ReadLine()
    .Split(", ", StringSplitOptions.RemoveEmptyEntries));

while (songs.Any())
{
    string[] operationInfo = Console.ReadLine().Split(" ");

    if (operationInfo[0] == "Play")
    {
        songs.Dequeue();
    }

    else if (operationInfo[0] == "Add")
    {
        string songName = string.Join(" ", operationInfo.Skip(1).ToArray());

        if (songs.Contains(songName))
        {
            Console.WriteLine($"{songName} is already contained!");
        }

        else
        {
            songs.Enqueue(songName);
        }
    }

    else if (operationInfo[0] == "Show")
    {
        Console.WriteLine(string.Join(", ", songs));
    }
}

Console.WriteLine("No more songs!");
