using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data.Models;

public class Blog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(BlogTitleMaxLength, MinimumLength = BlogTitleMinLength)]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public string PublisherId { get; set; } = null!;

    [ForeignKey(nameof(PublisherId))]
    public ApplicationUser Publisher { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly PublishDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Required]
    [MinLength(BlogContentMinLength)]
    public string Content { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
}