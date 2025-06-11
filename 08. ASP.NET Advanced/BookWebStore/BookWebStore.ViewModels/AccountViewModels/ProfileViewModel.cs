using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class ProfileViewModel
{
    [Required]
    public string Username { get; set; } = null!;

    public string? PhoneNumber { get; set; }
}