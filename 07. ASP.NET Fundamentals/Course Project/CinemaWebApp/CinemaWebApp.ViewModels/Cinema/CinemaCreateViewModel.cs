using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.ViewModels.Cinema;

public class CinemaCreateViewModel
{
    [Required]
    [MinLength(3), MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(3), MaxLength(85)]
    public string Location { get; set; } = null!;
}