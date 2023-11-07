//1 Exercise - Rubber Duck Debuggers
int[] timeInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();

int[] tasksInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();

Queue<int> time = new(timeInfo);
Stack<int> tasks = new(tasksInfo);
Dictionary<string, int> ducks = new()
{
   {"Darth Vader Ducky", 0},
   {"Thor Ducky", 0},
   {"Big Blue Rubber Ducky", 0},
   {"Small Yellow Rubber Ducky", 0}
};

while (time.Count > 0 && tasks.Count > 0)
{
    int currentTime = time.Dequeue();
    int currentTask = tasks.Pop();

    int result = currentTime * currentTask;

    if (result >= 0 && result <= 60)
    {
        ducks["Darth Vader Ducky"]++;
    }

    else if (result >= 61 && result <= 120)
    {
        ducks["Thor Ducky"]++;
    }

    else if (result >= 121 && result <= 180)
    {
        ducks["Big Blue Rubber Ducky"]++;
    }

    else if (result >= 181 && result <= 240)
    {
        ducks["Small Yellow Rubber Ducky"]++;
    }

    else
    {
        currentTask -= 2;
        time.Enqueue(currentTime);
        tasks.Push(currentTask);
    }
}

Console.WriteLine("Congratulations, all tasks have been completed! Rubber ducks rewarded:");
Console.WriteLine(string.Join(Environment.NewLine, ducks.Select(x => $"{x.Key}: {x.Value}")));