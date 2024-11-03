using MusicWebStore.Data.Models;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data;

public class Genre
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(GenreNameMaxLength, MinimumLength = GenreNameMinLength)]
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Artist> Artists { get; set; } = new HashSet<Artist>();
    public virtual ICollection<Album> Albums { get; set; } = new HashSet<Album>();
}