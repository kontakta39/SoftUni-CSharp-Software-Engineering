//7 Exercise - Parking Lot
string input = Console.ReadLine();
HashSet<string> carNumbers = new();

while (input != "END")
{
    string[] carInfo = input.Split(", ");
    string operation = carInfo[0];
    string currentCarNumber = carInfo[1];

    if (operation == "IN")
    {
        carNumbers.Add(currentCarNumber);
    }

    else if (operation == "OUT")
    {
        carNumbers.Remove(currentCarNumber);
    }

    input = Console.ReadLine();
}

if (carNumbers.Count == 0)
{
    Console.WriteLine("Parking Lot is Empty");
}

else
{
    foreach (var item in carNumbers)
    {
        Console.WriteLine(item);
    }
}
