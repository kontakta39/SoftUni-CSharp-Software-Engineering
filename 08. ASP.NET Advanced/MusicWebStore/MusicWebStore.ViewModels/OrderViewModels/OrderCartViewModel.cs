using MusicWebStore.Data;
using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class OrderCartViewModel
{
    public Guid Id { get; set; }
    public string AlbumTitle { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public virtual ICollection<BuyerAlbum> BuyerAlbums { get; set; } = new HashSet<BuyerAlbum>();
}