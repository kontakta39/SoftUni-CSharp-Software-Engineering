using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;

namespace EDriveRent.Repositories;

public class UserRepository : IRepository<IUser>
{
    private List<IUser> usersRepository;

    public UserRepository()
    {
        usersRepository = new();
    }

    public void AddModel(IUser model)
    {
        usersRepository.Add(model);
    }

    public bool RemoveById(string identifier)
    {
        IUser currentUser = usersRepository.Find(x => x.DrivingLicenseNumber == identifier);
        return usersRepository.Remove(currentUser);
    }

    public IUser FindById(string identifier)
    {
        IUser currentUser = usersRepository.Find(x => x.DrivingLicenseNumber == identifier);
        return currentUser;
    }

    public IReadOnlyCollection<IUser> GetAll() => usersRepository.AsReadOnly();
}