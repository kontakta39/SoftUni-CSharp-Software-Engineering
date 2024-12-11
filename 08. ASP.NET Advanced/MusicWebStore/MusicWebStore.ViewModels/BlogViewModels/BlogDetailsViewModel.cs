﻿using MusicWebStore.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class BlogDetailsViewModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public string PublisherId { get; set; } = null!;

    [Required]
    public string PublisherName { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly PublishDate { get; set; }

    [Required]
    public string Content { get; set; } = null!;
}