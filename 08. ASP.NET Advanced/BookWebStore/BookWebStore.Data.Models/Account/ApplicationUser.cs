using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(ApplicationUserNameMaxLength, MinimumLength = ApplicationUserNameMinLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(ApplicationUserNameMaxLength, MinimumLength = ApplicationUserNameMinLength)]
    public string LastName { get; set; } = null!;
}