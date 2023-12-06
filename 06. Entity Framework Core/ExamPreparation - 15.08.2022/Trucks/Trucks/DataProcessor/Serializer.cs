namespace Trucks.DataProcessor;

using Data;
using Newtonsoft.Json;
using Trucks.Data.Models;
using Trucks.DataProcessor.ExportDto;
using Trucks.Utilities;

public class Serializer
{
    public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
    {
        var xmlParser = new XmlParser();

        var despatchers = context.Despatchers
            .AsEnumerable()
            .Where(d => d.Trucks.Any())
            .Select(d => new ExportDespatcherDto()
            {
                TrucksCount = d.Trucks.Count(),
                DespatcherName = d.Name,
                Trucks = d.Trucks.Select(t => new TruckInfoDto()
                {
                    RegistrationNumber = t.RegistrationNumber,
                    Make = t.MakeType.ToString()
                })
                .OrderBy(d => d.RegistrationNumber)
                .ToArray()
            })
            .OrderByDescending(d => d.TrucksCount)
            .ThenBy(d => d.DespatcherName)
            .ToArray();

        var result = xmlParser.Serialize<ExportDespatcherDto>(despatchers, "Despatchers");
        return result;
    }

    public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
    {
        var clients = context.Clients
            .AsEnumerable()
            .Where(c => c.ClientsTrucks.Any(c => c.Truck.TankCapacity >= capacity))
            .Select(c => new ExportClientDto()
            {
                Name = c.Name,
                Trucks = c.ClientsTrucks
                .Where(c => c.Truck.TankCapacity >= capacity)
                .Select(ct => new TruckDto()
                { 
                TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                VinNumber = ct.Truck.VinNumber,
                TankCapacity = ct.Truck.TankCapacity,
                CargoCapacity = ct.Truck.CargoCapacity,
                CategoryType = ct.Truck.CategoryType.ToString(),
                MakeType = ct.Truck.MakeType.ToString()
                })
                .OrderBy(ct => ct.MakeType)
                .ThenByDescending(ct => ct.CargoCapacity)
                .ToArray()
            })
            .OrderByDescending(c => c.Trucks.Count())
            .ThenBy(c => c.Name)
            .Take(10)
            .ToArray();

        return JsonConvert.SerializeObject(clients, Formatting.Indented);
    }
}