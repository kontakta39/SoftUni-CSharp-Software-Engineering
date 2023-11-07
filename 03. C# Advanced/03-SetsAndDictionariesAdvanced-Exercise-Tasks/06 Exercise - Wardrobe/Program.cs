//6 Exercise - Wardrobe
int clothesCount = int.Parse(Console.ReadLine());
Dictionary<string, Dictionary<string, int>> colors = new();

for (int i = 0; i < clothesCount; i++)
{
    string[] delimiters = { " -> ", "," };
    string[] colorInfo = Console.ReadLine()
        .Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    string currentColor = colorInfo[0];

    for (int j = 1; j <= colorInfo.Length - 1; j++)
    {
        string currentWear = colorInfo[j];

        if (!colors.ContainsKey(currentColor))
        {
            colors.Add(currentColor, new Dictionary<string, int>());
            colors[currentColor].Add(currentWear, 1);
        }

        else if (!colors[currentColor].ContainsKey(currentWear))
        {
            colors[currentColor].Add(currentWear, 1);
        }

        else
        {
            colors[currentColor][currentWear]++;
        }
    }
}

string[] findWearInfo = Console.ReadLine().Split(" ");
string findColor = findWearInfo[0];
string findWear = findWearInfo[1];
bool ifExists = false;
string existingColor = string.Empty;

foreach (var (color, wear) in colors)
{
    if (colors.ContainsKey(findColor))
    {
      existingColor = findColor;
      ifExists = true;
    }

    Console.WriteLine($"{color} clothes:");

    foreach (var (currentWear, count) in wear)
    {
        if (ifExists)
        {
            if (existingColor == color && currentWear == findWear)
            {
                Console.WriteLine($"* {currentWear} - {count} (found!)");
            }

            else
            {
                Console.WriteLine($"* {currentWear} - {count}");
            }
        }

        else
        {
            Console.WriteLine($"* {currentWear} - {count}");
        }
    }
}

