using MusicWebStore.Data;
using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class OrderCartViewModel
{
    public Guid Id { get; set; } // Order Id
    public Guid AlbumId { get; set; } // Add this property for the Album Id
    public string AlbumTitle { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}