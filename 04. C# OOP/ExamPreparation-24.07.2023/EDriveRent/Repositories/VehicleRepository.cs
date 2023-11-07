using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System.Collections.Generic;

namespace EDriveRent.Repositories;

public class VehicleRepository : IRepository<IVehicle>
{
    private List<IVehicle> vehiclesRepository;

    public VehicleRepository()
    {
        vehiclesRepository = new();
    }

    public void AddModel(IVehicle vehicle)
    {
        vehiclesRepository.Add(vehicle);
    }

    public bool RemoveById(string identifier)
    {
        IVehicle currentVehicle = vehiclesRepository.Find(x => x.LicensePlateNumber == identifier);
        return vehiclesRepository.Remove(currentVehicle);
    }

    public IVehicle FindById(string identifier)
    { 
        IVehicle currentVehicle = vehiclesRepository.Find(x => x.LicensePlateNumber == identifier);
        return currentVehicle;
    }

    public IReadOnlyCollection<IVehicle> GetAll() => vehiclesRepository.AsReadOnly();
}