using CarDealer.Models;

namespace CarDealer.DTOs.Export;

public class ExportCarWithParts
{
    public string Make { get; set; }
    public string Model { get; set; }
    public long TraveledDistance { get; set; }
    public ICollection<PartCar> Parts { get; set; } = new List<PartCar>();
}