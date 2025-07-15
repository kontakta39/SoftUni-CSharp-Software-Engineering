namespace BookWebStore.ViewModels;

public class BlogIndexViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } 

    public string Publisher { get; set; } = null!;
}