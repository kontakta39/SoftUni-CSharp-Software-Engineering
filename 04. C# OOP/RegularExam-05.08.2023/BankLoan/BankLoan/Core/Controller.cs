using BankLoan.Core.Contracts;
using BankLoan.Models;
using BankLoan.Models.Contracts;
using BankLoan.Repositories;
using BankLoan.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BankLoan.Core;

public class Controller : IController
{
    private IRepository<ILoan> loans;
    private IRepository<IBank> banks;

    public Controller()
    {
        loans = new LoanRepository();
        banks = new BankRepository();
    }

    public string AddBank(string bankTypeName, string name)
    {
        //if (bankTypeName != nameof(BranchBank) && bankTypeName != nameof(CentralBank))
        //{
        //    throw new ArgumentException("Invalid bank type.");
        //}

        IBank bank = null;

        if (bankTypeName == nameof(BranchBank))
        {
            bank = new BranchBank(name);
        }

        else if (bankTypeName == nameof(CentralBank))
        {
            bank = new CentralBank(name);
        }

        else
        {
            throw new ArgumentException("Invalid bank type.");
        }

        banks.AddModel(bank);
        return $"{bankTypeName} is successfully added.";
    }

    public string AddLoan(string loanTypeName)
    {
        ILoan loan = null;

        if (loanTypeName == nameof(StudentLoan))
        {
            loan = new StudentLoan();
        }

        else if (loanTypeName == nameof(MortgageLoan))
        {
            loan = new MortgageLoan();
        }

        else
        {
            throw new ArgumentException("Invalid loan type.");
        }

        loans.AddModel(loan);
        return $"{loanTypeName} is successfully added.";
    }

    public string ReturnLoan(string bankName, string loanTypeName)
    {
        ILoan loan = loans.Models.FirstOrDefault(x => x.GetType().Name == loanTypeName);

        if (loan == null)
        {
            throw new ArgumentException($"Loan of type {loanTypeName} is missing.");
        }

        IBank bank = banks.Models.FirstOrDefault(x => x.Name == bankName);

        bank.AddLoan(loan);
        loans.RemoveModel(loan);
        return $"{loanTypeName} successfully added to {bankName}.";
    }

    public string AddClient(string bankName, string clientTypeName, string clientName, string id, double income)
    {
        IClient client = null;

        if (clientTypeName == nameof(Student))
        {
            client = new Student(clientName, id, income);
        }

        else if (clientTypeName == nameof(Adult))
        {
            client = new Adult(clientName, id, income);
        }

        else
        {
            throw new ArgumentException("Invalid client type.");
        }

        IBank bank = banks.FirstModel(bankName);

        if (client.GetType().Name == "Student" && bank.GetType().Name == "BranchBank")
        {
            bank.AddClient(client);
        }

        else if (client.GetType().Name == "Adult" && bank.GetType().Name == "CentralBank")
        {
            bank.AddClient(client);
        }

        else
        {
            return "Unsuitable bank.";
        }

        return $"{clientTypeName} successfully added to {bankName}.";
    }

    public string FinalCalculation(string bankName)
    {
        IBank bank = banks.FirstModel(bankName);

        double incomesCount = 0;
        double loansCount = 0;
        double sum = 0;

        foreach (var client in bank.Clients)
        {
            incomesCount += client.Income;
        }

        foreach (var loan in bank.Loans)
        {
            loansCount += loan.Amount;
        }

        sum = incomesCount + loansCount;
        return $"The funds of bank {bankName} are {sum:f2}.";
    }

    public string Statistics()
    {
        StringBuilder sb = new();

        foreach (var item in banks.Models)
        {
            sb.AppendLine(item.GetStatistics());
        }

        return sb.ToString().TrimEnd();
    }
}
