using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class AlbumIndexViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public int Stock { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Artist { get; set; } = null!;

    [Required]
    public string Genre { get; set; } = null!;
}