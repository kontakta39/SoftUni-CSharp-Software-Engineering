namespace BankLoan.Models;

public class BranchBank : Bank
{
    public const int branchBankCapacity = 25;

    public BranchBank(string name) 
        : base(name, branchBankCapacity)
    {
    }
}
