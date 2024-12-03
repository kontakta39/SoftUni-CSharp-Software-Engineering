namespace MusicWebStore.ViewModels;

public class CompletedOrderViewModel
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateOnly OrderDate { get; set; }
    public decimal TotalPrice { get; set; }

    public List<OrderedAlbumViewModel> OrderedAlbums { get; set; } = new List<OrderedAlbumViewModel>();
}