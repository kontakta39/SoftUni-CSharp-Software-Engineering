using BankLoan.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankLoan.Models;

public abstract class Bank : IBank
{
    private string name;
    private readonly List<ILoan> loans;
    private readonly List<IClient> clients;

    public Bank(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
        loans = new();
        clients = new();
    }

    public string Name
    {
        get => name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Bank name cannot be null or empty.");
            }

            name = value;
        }
    }

    public int Capacity { get; private set; }

    public IReadOnlyCollection<ILoan> Loans => loans.AsReadOnly();

    public IReadOnlyCollection<IClient> Clients => clients.AsReadOnly();

    public double SumRates()
    {
        double sumRates = 0;

        foreach (var loan in loans) 
        {
            sumRates += loan.InterestRate;
        }

        return sumRates;
    }

    public void AddClient(IClient Client)
    {
        if (clients.Count > Capacity)
        {
            throw new ArgumentException("Not enough capacity for this client.");
        }

        clients.Add(Client);
    }

    public void RemoveClient(IClient Client)
    {
        clients.Remove(Client);
    }

    public void AddLoan(ILoan loan)
    {
        loans.Add(loan);
    }

    public string GetStatistics()
    {
        StringBuilder sb = new();

        sb.AppendLine($"Name: {Name}, Type: {GetType().Name}");

        if (clients.Count == 0)
        {
            sb.AppendLine("Clients: none");
        }

        else
        {
            sb.Append("Clients: ");

            for (int i = 0; i <= clients.Count - 1; i++)
            {
                if (i == clients.Count - 1)
                {
                    sb.AppendLine($"{clients[i].Name}");
                }

                else
                {
                    sb.Append($"{clients[i].Name}, ");
                }
            }
        }

        sb.AppendLine($"Loans: {loans.Count}, Sum of Rates: {SumRates()}");

        return sb.ToString().TrimEnd();
    }
}
