using System.ComponentModel.DataAnnotations;
using static GameZone.Constants.ModelConstants;

namespace GameZone.Models.ViewModels;

public class EditViewModel
{
    [Required]
    [StringLength(GameTitleMaxLength, MinimumLength = GameTitleMinLength)]
    public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(GameDescriptionMaxLength, MinimumLength = GameDescriptionMinLength)]
    public string Description { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public string ReleasedOn { get; set; } = null!;

    [Required]
    public int GenreId { get; set; }

    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
}