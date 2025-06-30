namespace BookWebStore.ViewModels;

public class CompletedOrderViewModel
{
    public Guid OrderId { get; set; }

    public Guid BookId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateOnly OrderDate { get; set; }

    public string Title { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? TotalPrice { get; set; }

    public bool IsReturned { get; set; }
}