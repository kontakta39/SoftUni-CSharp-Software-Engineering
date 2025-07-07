using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class ReviewEditViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid BookId { get; set; }

    [Required]
    public string BookTitle { get; set; } = null!;

    [Required(ErrorMessage = "Please select a rating.")]
    [Range(ReviewRatingMinLength, ReviewRatingMaxLength)]
    public int? Rating { get; set; }

    [Required(ErrorMessage = "Review text is required.")]
    [StringLength(ReviewTextMaxLength, MinimumLength = ReviewTextMinLength)]
    public string ReviewText { get; set; } = null!;
}