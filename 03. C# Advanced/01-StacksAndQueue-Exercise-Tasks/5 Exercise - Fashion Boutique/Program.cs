//5 Exercise - Fashion Boutique
Stack<int> clothes = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));
int rackCapacity = int.Parse(Console.ReadLine());
int currentClothesCount = 0, clothesCount = 0;
int racksCount = 0;

while (clothes.Any())
{
    currentClothesCount = clothes.Peek();
    clothesCount += currentClothesCount;

    if (clothesCount < rackCapacity)
    {
        if (clothes.Count == 1)
        {
            if (clothesCount <= rackCapacity)
            {
                racksCount++;
            }
        }

        clothes.Pop();
    }

    else if (clothesCount == rackCapacity)
    {
        racksCount++;
        clothes.Pop();
        clothesCount = 0;
    }

    else if (clothesCount > rackCapacity)
    {
        racksCount++;
        clothesCount = 0;
    }
}

Console.WriteLine(racksCount);
