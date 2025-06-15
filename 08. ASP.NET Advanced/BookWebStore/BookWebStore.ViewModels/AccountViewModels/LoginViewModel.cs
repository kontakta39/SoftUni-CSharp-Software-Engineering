using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "The Username/Email field is required.")]
    public string UsernameOrEmail { get; set; } = null!;

    [Required(ErrorMessage = "The Password field is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}