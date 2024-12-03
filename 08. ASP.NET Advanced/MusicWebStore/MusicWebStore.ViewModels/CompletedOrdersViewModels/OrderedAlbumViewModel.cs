namespace MusicWebStore.ViewModels;

public class OrderedAlbumViewModel
{
    public Guid AlbumId { get; set; }
    public string AlbumImageUrl { get; set; } = null!;
    public string AlbumTitle { get; set; } = null!;
    public int AlbumQuantity { get; set; }
    public decimal AlbumPrice { get; set; }
    public bool isReturned { get; set; }
}