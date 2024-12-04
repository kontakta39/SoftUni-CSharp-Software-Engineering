using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class ReviewAddViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid AlbumId { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string AlbumTitle { get; set; } = null!;

    [Required]
    [StringLength(ReviewTextMaxLength, MinimumLength = ReviewTextMinLength)]
    public string ReviewText { get; set; } = null!;

    [Required]
    [Range(RatingMinLength, RatingMaxLength)]
    public int Rating { get; set; }
}