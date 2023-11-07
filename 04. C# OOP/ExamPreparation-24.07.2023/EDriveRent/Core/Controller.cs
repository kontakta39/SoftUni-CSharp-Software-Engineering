using EDriveRent.Core.Contracts;
using EDriveRent.Models;
using EDriveRent.Models.Contracts;
using EDriveRent.Repositories;
using EDriveRent.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDriveRent.Core;

public class Controller : IController
{
    private IRepository<IUser> users;
    private IRepository<IVehicle> vehicles;
    private IRepository<IRoute> routes;

    public Controller()
    {
        users = new UserRepository();
        vehicles = new VehicleRepository();
        routes = new RouteRepository();
    }

    public string RegisterUser(string firstName, string lastName, string drivingLicenseNumber)
    {
        IUser newUser = new User(firstName, lastName, drivingLicenseNumber);
        IUser checkUser = users.FindById(drivingLicenseNumber);

        if (checkUser == null)
        {
            users.AddModel(newUser);
            return $"{firstName} {lastName} is registered successfully with DLN-{drivingLicenseNumber}";
        }

        return $"{drivingLicenseNumber} is already registered in our platform.";
    }

    public string UploadVehicle(string vehicleType, string brand, string model, string licensePlateNumber)
    {
        if (vehicleType != nameof(PassengerCar) && vehicleType != nameof(CargoVan))
        {
            return $"{vehicleType} is not accessible in our platform.";
        }

        IVehicle vehicle = vehicles.FindById(licensePlateNumber);

        if (vehicle != null)
        {
            return $"{licensePlateNumber} belongs to another vehicle.";
        }
        else
        {
            if (vehicleType == nameof(PassengerCar))
            {
                vehicle = new PassengerCar(brand, model, licensePlateNumber);
            }
            else if (vehicleType == nameof(CargoVan))
            {
                vehicle = new CargoVan(brand, model, licensePlateNumber);
            }

            vehicles.AddModel(vehicle);
            return $"{brand} {model} is uploaded successfully with LPN-{licensePlateNumber}";
        }
    }

    public string AllowRoute(string startPoint, string endPoint, double length)
    {
        int routeId = routes.GetAll().Count + 1;

        IRoute existingRoute = routes
            .GetAll()
            .FirstOrDefault(r => r.StartPoint == startPoint && r.EndPoint == endPoint);

        if (existingRoute != null)
        {
            if (existingRoute.Length == length)
            {
                return $"{startPoint}/{endPoint} - {length} km is already added in our platform.";
            }
            else if (existingRoute.Length < length)
            {
                return $"{startPoint}/{endPoint} shorter route is already added in our platform.";
            }
            else if (existingRoute.Length > length)
            {
                existingRoute.LockRoute();
            }
        }

        IRoute newRoute = new Route(startPoint, endPoint, length, routeId);
        routes.AddModel(newRoute);

        return $"{startPoint}/{endPoint} - {length} km is unlocked in our platform.";
    }

    public string MakeTrip(string drivingLicenseNumber, string licensePlateNumber, string routeId, bool isAccidentHappened)
    {
        IUser user = users.FindById(drivingLicenseNumber);
        IVehicle vehicle = vehicles.FindById(licensePlateNumber);
        IRoute route = routes.FindById(routeId);

        if (user.IsBlocked == true)
        { 
        return $"User {drivingLicenseNumber} is blocked in the platform! Trip is not allowed.";
        }

        else if (vehicle.IsDamaged == true)
        {
            return $"Vehicle {licensePlateNumber} is damaged! Trip is not allowed.";
        }

        else if (route.IsLocked == true)
        {
            return $"Route {routeId} is locked! Trip is not allowed.";
        }

        vehicle.Drive(route.Length);

        if (isAccidentHappened)
        {
            vehicle.ChangeStatus();
            user.DecreaseRating();
        }

        else
        {
            user.IncreaseRating();
        }

        return vehicle.ToString();
    }

    public string RepairVehicles(int count)
    {
        IReadOnlyCollection<IVehicle> damagedVehicles = vehicles.GetAll().Where(x => x.IsDamaged).OrderBy(x => x.Brand).ThenBy(x => x.Model).Take(count).ToArray();

        foreach (var vehicle in damagedVehicles)
        {
            vehicle.ChangeStatus();
            vehicle.Recharge();
        }

        return $"{damagedVehicles.Count} vehicles are successfully repaired!";
    }

    public string UsersReport()
    {
        StringBuilder sb = new();
        IReadOnlyCollection<IUser> currentUsers = users.GetAll().OrderByDescending(x => x.Rating).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToArray();

        sb.AppendLine("*** E-Drive-Rent ***");

        foreach (var user in currentUsers)
        {
            sb.AppendLine(user.ToString());
        }

        return sb.ToString().TrimEnd();
    }
}