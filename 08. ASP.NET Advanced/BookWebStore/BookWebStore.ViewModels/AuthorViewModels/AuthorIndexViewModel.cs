namespace BookWebStore.ViewModels;

public class AuthorIndexViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string? ImageUrl { get; set; }
}
