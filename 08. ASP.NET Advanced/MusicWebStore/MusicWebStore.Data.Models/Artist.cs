using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data.Models;

public class Artist
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(ArtistNameMaxLength, MinimumLength = ArtistNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(BiographyMaxLength, MinimumLength = BiographyMinLength)]
    public string Biography { get; set; } = null!;

    [StringLength(NationalityMaxLength, MinimumLength = NationalityMinLength)]
    public string? Nationality { get; set; } = null!; 

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly BirthDate { get; set; }

    [StringLength(LabelMaxLength, MinimumLength = LabelMinLength)]
    public string? Label { get; set; }

    public string? ImageUrl { get; set; }
    public bool IsDeleted { get; set; } = false;

    [Required]
    public Guid GenreId { get; set; }

    [Required]
    [ForeignKey(nameof(GenreId))]
    public Genre Genre { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new HashSet<Album>();   
}