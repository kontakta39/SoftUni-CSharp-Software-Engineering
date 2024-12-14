using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.ViewModels;

public class BlogAddViewModel
{
    [Required]
    [StringLength(BlogTitleMaxLength, MinimumLength = BlogTitleMinLength)]
    public string Title { get; set; } = null!;

    public IFormFile? ImageFile { get; set; }
    public string? ImageUrl { get; set; } = null!;

    [Required]
    [MinLength(BlogContentMinLength)]
    public string Content { get; set; } = null!;
}