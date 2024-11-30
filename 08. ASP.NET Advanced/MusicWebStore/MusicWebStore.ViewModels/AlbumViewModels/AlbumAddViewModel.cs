using MusicWebStore.Data.Models;
using MusicWebStore.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class AlbumAddViewModel
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(AlbumTitleMaxLength, MinimumLength = AlbumTitleMinLength)]
    public string Title { get; set; } = null!;

    [StringLength(AlbumLabelMaxLength, MinimumLength = AlbumLabelMinLength)]
    public string? Label { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public string? ReleaseDate { get; set; } = null!;

    [Required]
    [StringLength(AlbumDescriptionMaxLength, MinimumLength = AlbumDescriptionMinLength)]
    public string Description { get; set; } = null!;

    public IFormFile? ImageFile { get; set; }

    public string? ImageUrl { get; set; } = null!;

    [Required]
    [Range(AlbumMinPrice, AlbumMaxPrice)]
    public decimal Price { get; set; }

    [Required]
    [Range(AlbumStockMinLength, AlbumStockMaxLength)]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Please select a genre.")]
    public Guid? GenreId { get; set; }

    [Required(ErrorMessage = "Please select an artist.")]
    public Guid? ArtistId { get; set; }

    public ICollection<Artist> Artists { get; set; } = new HashSet<Artist>();
    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
}