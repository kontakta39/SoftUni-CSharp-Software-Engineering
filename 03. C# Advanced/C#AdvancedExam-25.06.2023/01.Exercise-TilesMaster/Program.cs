//1 Exercise - Tiles Master
Stack<int> whiteTiles = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

Queue<int> greyTiles = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

Dictionary<string, int> locations = new()
{
    { "Countertop", 0 },
    { "Floor", 0 },
    { "Oven", 0 },
    { "Sink", 0 },
    { "Wall", 0 },
};

while (whiteTiles.Count > 0 && greyTiles.Count > 0)
{
    int currentWhiteTile = whiteTiles.Pop();
    int currentGreyTile = greyTiles.Dequeue();

    if (currentWhiteTile == currentGreyTile)
    {
        int sum = currentWhiteTile + currentGreyTile;

        if (sum == 40)
        {
            locations["Sink"]++;
        }

        else if (sum == 50)
        {
            locations["Oven"]++;
        }

        else if (sum == 60)
        {
            locations["Countertop"]++;
        }

        else if (sum == 70)
        {
            locations["Wall"]++;
        }

        else
        {
            locations["Floor"]++;
        }
    }

    else
    {
        currentWhiteTile /= 2;
        whiteTiles.Push(currentWhiteTile);
        greyTiles.Enqueue(currentGreyTile);
    }
}

if (whiteTiles.Any())
{
    Console.WriteLine($"White tiles left: {string.Join(", ", whiteTiles)}");
}

else
{
    Console.WriteLine($"White tiles left: none");
}

if (greyTiles.Any())
{
    Console.WriteLine($"Grey tiles left: {string.Join(", ", greyTiles)}");
}

else
{
    Console.WriteLine($"Grey tiles left: none");
}

foreach (var item in locations.OrderByDescending(x => x.Value)
    .ThenBy(x => x.Key))
{
    if (item.Value > 0)
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    } 
}