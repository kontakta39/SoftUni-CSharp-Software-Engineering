using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data;

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
    [Range(OrderTotalQuantityMinLength, OrderTotalQuantityMaxLength)]
    public int TotalQuantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    [Required]
    public string BuyerId { get; set; } = null!;

    [ForeignKey(nameof(BuyerId))]
    public IdentityUser Buyer { get; set; } = null!;
	
	public bool IsCompleted { get; set; } = false;

    public ICollection<OrderAlbum> OrdersAlbums { get; set; } = new HashSet<OrderAlbum>();
}