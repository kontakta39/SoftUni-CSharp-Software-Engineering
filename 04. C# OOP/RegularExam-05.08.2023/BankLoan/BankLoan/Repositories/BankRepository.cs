using BankLoan.Models.Contracts;
using BankLoan.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BankLoan.Repositories;

public class BankRepository : IRepository<IBank>
{
    private List<IBank> banksRepository;

    public BankRepository()
    {
        banksRepository = new();
    }

    public IReadOnlyCollection<IBank> Models => banksRepository.AsReadOnly();

    public void AddModel(IBank model)
    {
        banksRepository.Add(model);
    }

    public bool RemoveModel(IBank model)
    {
        return banksRepository.Remove(model);
    }

    public IBank FirstModel(string name)
    {
        return banksRepository.FirstOrDefault(x => x.Name == name);
    }
}
