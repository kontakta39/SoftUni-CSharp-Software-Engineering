using System.ComponentModel.DataAnnotations;

namespace MusicWebStore.ViewModels;

public class ArtistDeleteViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}