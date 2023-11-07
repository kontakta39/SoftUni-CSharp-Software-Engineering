namespace BankLoan.Models;

public class CentralBank : Bank
{
    public const int centralBankCapacity = 50;

    public CentralBank(string name) 
        : base(name, centralBankCapacity)
    {
    }
}
