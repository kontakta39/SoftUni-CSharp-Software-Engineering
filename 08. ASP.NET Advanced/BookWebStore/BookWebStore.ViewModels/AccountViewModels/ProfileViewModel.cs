using BookWebStore.Constants;
using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class ProfileViewModel
{
    [Required]
    public string Username { get; set; } = null!;

    public string? CurrentPhoneNumber { get; set; }

    [RegularExpression(PhoneNumberRegex,
       ErrorMessage = "Phone number must start with +359 and be followed by 9 digits.")]
    public string? NewPhoneNumber { get; set; }
}