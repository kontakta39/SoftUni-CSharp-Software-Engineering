using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebStore.ViewModels;

public class OrderCartViewModel
{
    public Guid OrderId { get; set; } 

    public Guid BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string? ImageUrl { get; set; } 

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ItemTotal => (UnitPrice ?? 0) * Quantity;

    public bool IsCompleted { get; set; }
}