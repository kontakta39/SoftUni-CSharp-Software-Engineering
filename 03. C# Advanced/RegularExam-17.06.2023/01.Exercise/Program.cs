//1 Exercise
Queue<int> tools = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

Stack<int> substances = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

List<int> challanges = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

while (tools.Count > 0 && substances.Count > 0)
{
    int currentTool = tools.Dequeue();
    int currentSubstance = substances.Pop();
    int sum = currentTool * currentSubstance;
    bool isRemoved = false;

    for (int i = 0; i < challanges.Count; i++)
    {
        if (challanges[i] == sum)
        {
            challanges.RemoveAt(i);
            isRemoved = true;
            break;
        }
    }

    if (isRemoved == false)
    {
        currentTool += 1;
        tools.Enqueue(currentTool);
        currentSubstance -= 1;

        if (currentSubstance > 0)
        {
            substances.Push(currentSubstance);
        }
    }
}

if (tools.Count > 0 && substances.Count == 0 && challanges.Count == 0)
{
    Console.WriteLine($"Harry found an ostracon, which is dated to the 6th century BCE.");
    Console.WriteLine($"Tools: {string.Join(", ", tools)}");
}

else if (tools.Count == 0 && substances.Count > 0 && challanges.Count == 0)
{
    Console.WriteLine($"Harry found an ostracon, which is dated to the 6th century BCE.");
    Console.WriteLine($"Substances: {string.Join(", ", substances)}");
}

else if (tools.Count > 0 && substances.Count > 0 && challanges.Count == 0)
{
    Console.WriteLine($"Harry found an ostracon, which is dated to the 6th century BCE.");
    Console.WriteLine($"Tools: {string.Join(", ", tools)}");
    Console.WriteLine($"Substances: {string.Join(", ", substances)}");
}

else if (tools.Count == 0 && substances.Count == 0 && challanges.Count == 0)
{
    Console.WriteLine($"Harry found an ostracon, which is dated to the 6th century BCE.");
}

else if (tools.Count > 0 && substances.Count == 0 && challanges.Count > 0)
{
    Console.WriteLine($"Harry is lost in the temple. Oblivion awaits him.");
    Console.WriteLine($"Tools: {string.Join(", ", tools)}");
    Console.WriteLine($"Challenges: {string.Join(", ", challanges)}");
}

else if (tools.Count == 0 && substances.Count > 0 && challanges.Count > 0)
{
    Console.WriteLine($"Harry is lost in the temple. Oblivion awaits him.");
    Console.WriteLine($"Substances: {string.Join(", ", substances)}");
    Console.WriteLine($"Challenges: {string.Join(", ", challanges)}");
}