using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class BlogIndexViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    [Required]
    public string PublisherName { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly PublishDate { get; set; }
}