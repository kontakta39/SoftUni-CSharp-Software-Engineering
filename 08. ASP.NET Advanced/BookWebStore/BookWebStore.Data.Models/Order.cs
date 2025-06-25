using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string OrderNumber { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(OrderMinPrice, double.MaxValue)]
    public decimal? TotalPrice { get; set; }

    [Required]
    public string BuyerId { get; set; } = null!;

    [ForeignKey(nameof(BuyerId))]
    public ApplicationUser Buyer { get; set; } = null!;

    public bool IsCompleted { get; set; } = false;

    public ICollection<OrderBook> OrdersBooks { get; set; } = new List<OrderBook>();
}