using MusicWebStore.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class BlogAddViewModel
{
    [Required]
    [StringLength(BlogTitleMaxLength, MinimumLength = BlogTitleMinLength)]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    [MinLength(BlogContentMinLength)]
    public string Content { get; set; } = null!;
}