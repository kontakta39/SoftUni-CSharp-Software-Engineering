using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class GenreIndexViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}