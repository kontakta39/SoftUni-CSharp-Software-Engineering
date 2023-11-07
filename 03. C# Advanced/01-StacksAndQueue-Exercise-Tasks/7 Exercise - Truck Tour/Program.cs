//7 Exercise - Truck Tour
int stopsNumber = int.Parse(Console.ReadLine());
Stack<int> petrolAmount = new();
Stack<int> distance = new();
int currentPetrolAmount = 0;
int currentDistance = 0;
int fuelLeft = 0;
int pumpNumber = 0;

for (int i = 0; i < stopsNumber; i++)
{
    int[] operationInfo = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    petrolAmount.Push(operationInfo[0]);
    distance.Push(operationInfo[1]);
    currentPetrolAmount = petrolAmount.Peek();
    currentDistance = distance.Peek();

    fuelLeft += currentPetrolAmount;

    if (fuelLeft >= currentDistance)
    {
        fuelLeft -= currentDistance;
    }

    else
    {
        pumpNumber = i + 1;
        fuelLeft = 0;
    }
}

Console.WriteLine(pumpNumber);