using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static GameZone.Constants.ModelConstants;

namespace GameZone.Models;

public class Game
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(GameTitleMaxLength, MinimumLength = GameTitleMinLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(GameDescriptionMaxLength, MinimumLength = GameDescriptionMinLength)]
    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    public string PublisherId { get; set; } = null!;

    [Required]
    public IdentityUser Publisher { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime ReleasedOn { get; set; }

    [Required]
    public int GenreId { get; set; }

    [Required]
    [ForeignKey(nameof(GenreId))]
    public Genre Genre { get; set; } = null!;

    public ICollection<GamerGame> GamersGames { get; set; } = new HashSet<GamerGame>();

    public bool IsDeleted { get; set; } = false;
}