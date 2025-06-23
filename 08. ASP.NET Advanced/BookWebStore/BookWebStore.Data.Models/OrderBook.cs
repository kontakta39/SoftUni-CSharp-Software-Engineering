using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

[PrimaryKey(nameof(OrderId), nameof(BookId))]
public class OrderBook
{
    public Guid OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;

    public Guid BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; } = null!;

    [Required]
    [Range(OrderMinQuantity, OrderMaxQuantity)]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(OrderMinPrice, OrderMaxPrice)]
    public decimal UnitPrice { get; set; }
}