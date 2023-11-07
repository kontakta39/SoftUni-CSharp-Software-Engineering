//1 Exercise - Apocalypse Preparation
Queue<int> textiles = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

Stack<int> medicaments = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

Dictionary<string, int> healingItems = new()
{
    { "Patch", 0},
    { "Bandage", 0},
    { "MedKit", 0}
};

while (textiles.Count > 0 && medicaments.Count > 0)
{
    int currentTextile = textiles.Dequeue();
    int currentMedicament = medicaments.Pop();
    int result = currentTextile + currentMedicament;

    if (result == 30)
    {
        healingItems["Patch"]++;
    }

    else if (result == 40)
    {
        healingItems["Bandage"]++;
    }

    else if (result >= 100)
    {
        if (result > 100)
        {
            healingItems["MedKit"]++;
            result -= 100;
            int currentElement = medicaments.Pop();
            currentElement += result;
            medicaments.Push(currentElement);
        }

        else
        {
            healingItems["MedKit"]++;
        }
    }

    else
    {
        currentMedicament += 10;
        medicaments.Push(currentMedicament);
    }
}

if (textiles.Count == 0 && medicaments.Count == 0)
{
    Console.WriteLine("Textiles and medicaments are both empty.");
}

else if (textiles.Count == 0)
{
    Console.WriteLine("Textiles are empty.");
}

else
{
    Console.WriteLine("Medicaments are empty.");
}

foreach (var item in healingItems.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
{
    if (item.Value > 0)
    {
        Console.WriteLine($"{item.Key} - {item.Value}");
    }
}

if (textiles.Any() || medicaments.Any())
{
    if (textiles.Count > 0)
    {
        Console.WriteLine($"Textiles left: {string.Join(", ", textiles)}");
    }

    else if (medicaments.Count > 0)
    {
        Console.WriteLine($"Medicaments left: {string.Join(", ", medicaments)}");
    }

    else
    {
        Console.WriteLine($"Textiles left: {string.Join(", ", textiles)}");
        Console.WriteLine($"Medicaments left: {string.Join(", ", medicaments)}");
    }
}