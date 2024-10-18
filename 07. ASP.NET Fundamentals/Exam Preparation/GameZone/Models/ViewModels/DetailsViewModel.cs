namespace GameZone.Models.ViewModels;

public class DetailsViewModel
{
    public required int Id { get; set; }
    public string? ImageUrl { get; set; }
    public required string Description { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public required string ReleasedOn { get; set; }
    public required string Publisher { get; set; }
}