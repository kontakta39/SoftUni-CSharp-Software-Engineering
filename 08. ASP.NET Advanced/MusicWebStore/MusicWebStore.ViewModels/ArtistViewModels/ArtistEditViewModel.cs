using MusicWebStore.Data;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class ArtistEditViewModel
{
    [Required]
    [StringLength(ArtistNameMaxLength, MinimumLength = ArtistNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(ArtistBiographyMaxLength, MinimumLength = ArtistBiographyMinLength)]
    public string Biography { get; set; } = null!;

    [StringLength(ArtistNationalityMaxLength, MinimumLength = ArtistNationalityMinLength)]
    public string? Nationality { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public string? BirthDate { get; set; } = null!;

    [StringLength(ArtistWebsiteMaxLength, MinimumLength = ArtistWebsiteMinLength)]
    public string? Website { get; set; }

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public Guid GenreId { get; set; }

    public ICollection<Genre> Genres = new HashSet<Genre>();

    public ICollection<string> NationalityOptions { get; set; } = new SortedSet<string>();
}