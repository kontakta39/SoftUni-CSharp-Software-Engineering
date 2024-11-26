using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicWebStore.Data;

[PrimaryKey(nameof(BuyerId), nameof(AlbumId), nameof(OrderId))]
public class BuyerAlbum
{
    [Key]
    public string BuyerId { get; set; } = null!;

    [ForeignKey(nameof(BuyerId))]
    public IdentityUser Buyer { get; set; } = null!;

    [Key]
    public Guid AlbumId { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public Album Album { get; set; } = null!;

    [Key]
    public Guid OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
}