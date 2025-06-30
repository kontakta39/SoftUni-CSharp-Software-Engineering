namespace BookWebStore.ViewModels;

public class OrderReturnBookViewModel
{
    public Guid OrderId { get; set; }

    public Guid BookId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateOnly OrderDate { get; set; }

    public string Title { get; set; } = null!;
}