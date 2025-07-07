using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Review
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly ReviewDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Required(ErrorMessage = "Please select a rating.")]
    [Range(ReviewRatingMinLength, ReviewRatingMaxLength)]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Review text is required.")]
    [StringLength(ReviewTextMaxLength, MinimumLength = ReviewTextMinLength)]
    public string ReviewText { get; set; } = null!;

    public bool IsEdited { get; set; } = false;

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [Required]
    public Guid BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; } = null!;
}