using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class ArtistIndexViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    public string Genre { get; set; } = null!;
}