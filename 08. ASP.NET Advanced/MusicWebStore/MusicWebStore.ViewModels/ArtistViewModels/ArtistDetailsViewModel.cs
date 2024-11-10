using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class ArtistDetailsViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Biography { get; set; } = null!;

    public string? Nationality { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string? Label { get; set; }

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public string Genre { get; set; } = null!;
}