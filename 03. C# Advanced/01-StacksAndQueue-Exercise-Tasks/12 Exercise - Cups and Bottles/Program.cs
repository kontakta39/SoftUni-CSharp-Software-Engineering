//12 Exercise - Cups and Bottles
Queue<int> cups = new(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
Stack<int> bottles = new(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
int wastedWaterSum = 0;

while (cups.Count > 0 && bottles.Count > 0)
{
    int currentCupSize = cups.Peek();
    int currentBottleSize = bottles.Peek();

    if (currentCupSize <= currentBottleSize)
    {
        int remainingWater = currentBottleSize - currentCupSize;
        wastedWaterSum += remainingWater;
        cups.Dequeue();
        bottles.Pop();
    }

    else if (currentCupSize > currentBottleSize)
    {
        while (currentCupSize > currentBottleSize)
        {
            bottles.Pop();
            currentBottleSize += bottles.Peek();
        }

        int remainingWater = currentBottleSize - currentCupSize;
        wastedWaterSum += remainingWater;
        cups.Dequeue();
        bottles.Pop();
    }
}

if (cups.Count > 0)
{
    Console.WriteLine($"Cups: {string.Join(" ", cups)}");
    Console.WriteLine($"Wasted litters of water: {wastedWaterSum}");
}

else if (bottles.Count > 0)
{
    Console.WriteLine($"Bottles: {string.Join(" ", bottles)}");
    Console.WriteLine($"Wasted litters of water: {wastedWaterSum}");
}
