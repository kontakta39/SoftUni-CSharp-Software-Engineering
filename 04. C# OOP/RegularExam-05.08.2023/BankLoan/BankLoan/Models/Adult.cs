namespace BankLoan.Models;

public class Adult : Client
{
    public static int adultInterest = 4;

    public Adult(string name, string id, double income) 
        : base(name, id, adultInterest, income)
    {
    }

    public override void IncreaseInterest()
    {
        adultInterest += 2;
    }
}
