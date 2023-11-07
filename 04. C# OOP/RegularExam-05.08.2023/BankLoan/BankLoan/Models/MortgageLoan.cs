namespace BankLoan.Models;

public class MortgageLoan : Loan
{
    public const int mortgageInterestRate = 3;
    public const double mortgageAmount = 50000;

    public MortgageLoan() 
        : base(mortgageInterestRate, mortgageAmount)
    {
    }
}
