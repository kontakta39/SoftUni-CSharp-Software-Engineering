using MusicWebStore.Data.Models;
using MusicWebStore.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class AlbumDetailsViewModel
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; } = null!;

    public string? Label { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    [Required]
    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public string Artist { get; set; } = null!;

    [Required]
    public Guid ArtistId { get; set; }

    [Required]
    public string Genre { get; set; } = null!;
}
