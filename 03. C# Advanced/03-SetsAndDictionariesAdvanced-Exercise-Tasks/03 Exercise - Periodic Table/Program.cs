//3 Exercise - Periodic Table
int elementsCount = int.Parse(Console.ReadLine());
HashSet<string> elements = new();

for (int i = 0; i < elementsCount; i++)
{
    string[] elementsInfo = Console.ReadLine().Split(" ");

    for (int j = 0; j < elementsInfo.Length; j++)
    {
        elements.Add(elementsInfo[j]);
    }
}

elements = elements.OrderBy(x => x).ToHashSet();
Console.WriteLine(string.Join(" ", elements));
