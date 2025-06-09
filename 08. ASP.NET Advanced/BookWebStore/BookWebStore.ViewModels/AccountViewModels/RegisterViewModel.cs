using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "The First Name field is required.")]
    [StringLength(ApplicationUserNameMaxLength, MinimumLength = ApplicationUserNameMinLength, ErrorMessage = "The First Name must be between 3 and 20 characters.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "The Last Name field is required.")]
    [StringLength(ApplicationUserNameMaxLength, MinimumLength = ApplicationUserNameMinLength, ErrorMessage = "The Last Name must be between 3 and 20 characters.")]
    public string LastName { get; set; } = null!;

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