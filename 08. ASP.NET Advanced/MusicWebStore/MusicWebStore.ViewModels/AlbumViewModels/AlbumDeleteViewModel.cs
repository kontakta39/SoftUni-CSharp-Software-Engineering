using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class AlbumDeleteViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Artist { get; set; } = null!;
}