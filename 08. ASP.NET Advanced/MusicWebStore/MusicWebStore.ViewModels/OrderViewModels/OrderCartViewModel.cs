namespace MusicWebStore.ViewModels;

public class OrderCartViewModel
{
    public Guid Id { get; set; } // Order Id
    public Guid AlbumId { get; set; } // Property for the Album Id
    public string AlbumTitle { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsCompleted { get; set; }
}