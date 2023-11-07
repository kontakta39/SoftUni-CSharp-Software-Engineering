//6 Exercise - Record Unique Names
int namesCount = int.Parse(Console.ReadLine());
HashSet<string> names = new();

for (int i = 0; i < namesCount; i++)
{
    string currentName = Console.ReadLine();
    names.Add(currentName);
}

foreach (var item in names)
{
    Console.WriteLine(item);
}

