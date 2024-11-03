using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicWebStore.Data;

[PrimaryKey(nameof(OrderId), nameof(AlbumId))]
public class OrderAlbum
{
    [Key]
    public Guid OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;

    [Key]
    public Guid AlbumId { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public Album Album { get; set; } = null!;
}