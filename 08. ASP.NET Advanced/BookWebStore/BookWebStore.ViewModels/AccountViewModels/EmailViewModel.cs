using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class EmailViewModel
{
    [Required]
    public string CurrentEmail { get; set; } = null!;

    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? NewEmail { get; set; } 
}