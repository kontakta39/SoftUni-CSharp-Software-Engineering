using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;

namespace EDriveRent.Repositories;

public class RouteRepository : IRepository<IRoute>
{
    private List<IRoute> routesRepository;

    public RouteRepository()
    {
        routesRepository = new();
    }

    public void AddModel(IRoute model)
    {
        routesRepository.Add(model);
    }

    public bool RemoveById(string identifier)
    {
        IRoute currentRoute = routesRepository.Find(x => x.RouteId == int.Parse(identifier));
        return routesRepository.Remove(currentRoute);
    }

    public IRoute FindById(string identifier)
    {
        IRoute currentRoute = routesRepository.Find(x => x.RouteId == int.Parse(identifier));
        return currentRoute;
    }

    public IReadOnlyCollection<IRoute> GetAll() => routesRepository.AsReadOnly();
}