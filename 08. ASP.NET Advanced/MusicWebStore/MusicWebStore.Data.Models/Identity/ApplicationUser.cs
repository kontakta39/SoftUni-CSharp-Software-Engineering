using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static MusicWebStore.Constants.ModelConstants;

namespace MusicWebStore.Data.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(ApplicationUserNameMaxLength, MinimumLength = ApplicationUserNameMinLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(ApplicationUserNameMaxLength, MinimumLength = ApplicationUserNameMinLength)]
    public string LastName { get; set; } = null!;

    public HashSet<Blog> Blogs { get; set; } = new HashSet<Blog>();
}