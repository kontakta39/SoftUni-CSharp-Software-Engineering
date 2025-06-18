using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class GenreDeleteViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}