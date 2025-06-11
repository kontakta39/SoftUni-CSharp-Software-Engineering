using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class EmailViewModel
{
    [Required]
    public string CurrentEmail { get; set; } = null!;

    public string? NewEmail { get; set; } 
}