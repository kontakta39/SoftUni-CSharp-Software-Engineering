using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class ChangePasswordViewModel
{
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [DataType(DataType.Password)]
    [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "The password must be between 6 and 20 characters.")]
    public string? NewPassword { get; set; } 

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; } 
}