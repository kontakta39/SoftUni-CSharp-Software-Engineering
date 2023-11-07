//8 Exercise - SoftUni Party
string input = Console.ReadLine();
HashSet<string> vipGuests = new();
HashSet<string> regularGuests = new();

while (input != "PARTY")
{
    string currentGuest = input;

    for (int i = 0; i < currentGuest.Length; i++)
    {
        bool isDigit = Char.IsDigit(currentGuest[i]);

        if (isDigit)
        {
            vipGuests.Add(currentGuest);
        }

        else 
        {
            regularGuests.Add(currentGuest);
        }

        break;
    }

    input = Console.ReadLine();
}

while (input != "END")
{
    string currentGuest = input;

    for (int i = 0; i < currentGuest.Length; i++)
    {
        bool isDigit = Char.IsDigit(currentGuest[i]);

        if (isDigit)
        {
            vipGuests.Remove(currentGuest);
        }

        else
        {
            regularGuests.Remove(currentGuest);
        }

        break;
    }

    input = Console.ReadLine();
}

int count = vipGuests.Count + regularGuests.Count;
Console.WriteLine(count);

foreach (var item in vipGuests)
{
    Console.WriteLine(item);
}

foreach (var item in regularGuests)
{
    Console.WriteLine(item);
}
