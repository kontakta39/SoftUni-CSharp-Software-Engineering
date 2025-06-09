using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The Password field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}