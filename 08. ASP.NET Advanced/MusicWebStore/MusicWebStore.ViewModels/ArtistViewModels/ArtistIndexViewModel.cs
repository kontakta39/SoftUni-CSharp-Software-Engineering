using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class ArtistIndexViewModel
{
    public required Guid Id { get; set; }

    [StringLength(ArtistNameMaxLength, MinimumLength = ArtistNameMinLength)]
    public required string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string Genre { get; set; } = null!;
}