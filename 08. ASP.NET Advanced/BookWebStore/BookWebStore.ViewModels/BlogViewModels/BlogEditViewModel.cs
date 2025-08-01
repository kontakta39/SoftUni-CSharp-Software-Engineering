﻿using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class BlogEditViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(BlogTitleMaxLength, MinimumLength = BlogTitleMinLength)]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    public string PublisherId { get; set; } = null!;

    [Required]
    [MinLength(BlogContentMinLength)]
    public string Content { get; set; } = null!;
}
