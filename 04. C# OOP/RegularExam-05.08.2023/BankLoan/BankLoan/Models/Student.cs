namespace BankLoan.Models;

public class Student : Client
{
    public static int studentInterest = 2;

    public Student(string name, string id, double income) 
        : base(name, id, studentInterest, income)
    {
    }

    public override void IncreaseInterest()
    {
        studentInterest += 1;
    }
}
