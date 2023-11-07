using BankLoan.Models.Contracts;
using BankLoan.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BankLoan.Repositories;

public class LoanRepository : IRepository<ILoan>
{
    private List<ILoan> loansRepository;

    public LoanRepository()
    {
        loansRepository = new();
    }

    public IReadOnlyCollection<ILoan> Models => loansRepository.AsReadOnly();

    public void AddModel(ILoan model)
    {
        loansRepository.Add(model);
    }

    public bool RemoveModel(ILoan model)
    {
        return loansRepository.Remove(model);
    }

    public ILoan FirstModel(string name)
    {
        return loansRepository.FirstOrDefault(x => x.GetType().Name == name);
    }
}
