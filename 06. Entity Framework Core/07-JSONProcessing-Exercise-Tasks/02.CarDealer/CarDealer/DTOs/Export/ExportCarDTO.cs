namespace CarDealer.DTOs.Export;

public class ExportCarDTO
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public long TraveledDistance { get; set; }
}