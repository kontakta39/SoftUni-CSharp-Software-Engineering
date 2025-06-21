namespace BookWebStore.ViewModels;

public class BookIndexViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } 

    public string Author { get; set; } = null!;

    public string Genre { get; set; } = null!;
}