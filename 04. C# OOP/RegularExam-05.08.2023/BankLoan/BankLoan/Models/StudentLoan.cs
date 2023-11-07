namespace BankLoan.Models;

public class StudentLoan : Loan
{
    public const int studentInterestRate = 1;
    public const double studentAmount = 10000;

    public StudentLoan() 
        : base(studentInterestRate, studentAmount)
    {
    }
}
