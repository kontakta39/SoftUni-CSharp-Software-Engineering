using System.ComponentModel.DataAnnotations;
using static GameZone.Constants.ModelConstants;

namespace GameZone.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(GenreTitleMaxLength, MinimumLength = GenreTitleMinLength)]
    public string Name { get; set; } = null!;

    public ICollection<Game> Games { get; set; } = new HashSet<Game>();
}