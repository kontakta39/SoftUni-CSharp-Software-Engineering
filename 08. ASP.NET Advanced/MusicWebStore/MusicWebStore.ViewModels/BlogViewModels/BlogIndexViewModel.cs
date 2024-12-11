using MusicWebStore.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicWebStore.ViewModels;

public class BlogIndexViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public string PublisherName { get; set; } = null!;
}