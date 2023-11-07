//10 Exercise - Crossroads
int greenLightTime = int.Parse(Console.ReadLine());
int freeWindow = int.Parse(Console.ReadLine());
Queue<string> cars = new();
string input = Console.ReadLine();
int passedCarsCount = 0;
int currentGreenLightTime = 0;

while (input != "END")
{
    if (input != "green")
    {
        cars.Enqueue(input);
    }

    else if (input == "green")
    {
        currentGreenLightTime = greenLightTime;

        while (cars.Any() && currentGreenLightTime > 0)
        {
            string currentCar = cars.Dequeue();

            if (currentGreenLightTime >= currentCar.Length)
            {
                currentGreenLightTime -= currentCar.Length;
                passedCarsCount++;
            }

            else if (currentGreenLightTime < currentCar.Length)
            {
                string currentCarCopy = currentCar.Remove(0, currentGreenLightTime);
                currentGreenLightTime = 0;

                if (freeWindow >= currentCarCopy.Length)
                {
                    freeWindow -= currentCarCopy.Length;
                    passedCarsCount++;
                }

                else if (freeWindow < currentCarCopy.Length)
                {
                    currentCarCopy = currentCarCopy.Remove(0, freeWindow);
                    string symbol = currentCarCopy.Substring(0, 1);
                    Console.WriteLine("A crash happened!");
                    Console.WriteLine($"{currentCar} was hit at {symbol}.");
                    Environment.Exit(0);
                }
            }
        }
    }

    input = Console.ReadLine();
}

Console.WriteLine("Everyone is safe.");
Console.WriteLine($"{passedCarsCount} total cars passed the crossroads.");