//8 Exercise - Threeuple
using Threeuple;

public class StartUp
{
    static void Main()
    {
        string[] addressInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        addressInfo[0] += " " + addressInfo[1];

        if (addressInfo.Length > 4)
        {
            addressInfo[3] += " " + addressInfo[4];
        }

        string[] beerInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] bankBalanceInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);

        TupleTypes<string, string, string> personAddress = new(addressInfo[0], addressInfo[2], addressInfo[3]);
        TupleTypes<string, int, bool> personBeer = new(beerInfo[0], int.Parse(beerInfo[1]), beerInfo[2] == "drunk");
        TupleTypes<string, double, string> bankBalance = new(bankBalanceInfo[0], double.Parse(bankBalanceInfo[1]), bankBalanceInfo[2]);

        Console.WriteLine(personAddress.ToString());
        Console.WriteLine(personBeer.ToString());
        Console.WriteLine(bankBalance.ToString());
    }
}
