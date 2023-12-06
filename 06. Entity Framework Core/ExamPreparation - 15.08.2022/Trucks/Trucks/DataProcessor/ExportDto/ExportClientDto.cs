using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ImportDto;

namespace Trucks.DataProcessor.ExportDto;

public class ExportClientDto
{
    public string Name { get; set; }
    public TruckDto[] Trucks { get; set; }
}

public class TruckDto
{
    public string TruckRegistrationNumber { get; set; }

    public string VinNumber { get; set; }

    public int TankCapacity { get; set; }

    public int CargoCapacity { get; set; }

    public string CategoryType { get; set; }

    public string MakeType { get; set; }
}