using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data;

public class Review
{
    [Key]
    public Guid Id { get; set; }  = Guid.NewGuid();

    [Required]
    public Guid AlbumId { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public Album Album { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly ReviewDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Required]
    [StringLength(ReviewTextMaxLength, MinimumLength = ReviewTextMinLength)]
    public string ReviewText { get; set; } = null!;

    [Range(RatingMinLength, RatingMaxLength)]
    public int Rating { get; set; }

    public bool isCommented { get; set; } = false;

    public bool IsEdited { get; set; } = false;
}