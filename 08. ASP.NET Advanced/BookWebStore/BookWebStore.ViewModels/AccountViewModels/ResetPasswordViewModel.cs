using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public string Token { get; set; } = null!;

    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The Password field is required.")]
    [DataType(DataType.Password)]
    [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "The password must be between 6 and 20 characters.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "The Confirm Password field is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}