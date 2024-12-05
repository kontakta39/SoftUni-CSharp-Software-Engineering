using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class ReviewEditViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid AlbumId { get; set; }

    [Required]
    public string AlbumTitle { get; set; } = null!;

    [Required(ErrorMessage = "Please select a rating.")]
    [Range(RatingMinLength, RatingMaxLength)]
    public int? Rating { get; set; }

    [Required(ErrorMessage = "Review text is required.")]
    [StringLength(ReviewTextMaxLength, MinimumLength = ReviewTextMinLength)]
    public string ReviewText { get; set; } = null!;
}