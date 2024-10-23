namespace DeskMarket.Models;

public class IndexViewModel
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public bool IsSeller { get; set; } = false;
    public bool HasBought { get; set; } = false;
}