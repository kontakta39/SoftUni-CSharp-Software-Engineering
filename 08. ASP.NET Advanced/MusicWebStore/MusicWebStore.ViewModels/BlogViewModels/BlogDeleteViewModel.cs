using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class BlogDeleteViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;
}