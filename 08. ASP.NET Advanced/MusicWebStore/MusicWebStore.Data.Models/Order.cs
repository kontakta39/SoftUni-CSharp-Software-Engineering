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
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Required]
    [Range(OrderQuantityMinLength, OrderQuantityMaxLength)]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; } = null!;

    public virtual ICollection<OrderAlbum> OrderAlbums { get; set; } = new HashSet<OrderAlbum>();
}