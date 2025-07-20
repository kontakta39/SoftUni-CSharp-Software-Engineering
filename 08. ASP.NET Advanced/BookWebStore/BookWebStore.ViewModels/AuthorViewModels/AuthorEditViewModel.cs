using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class AuthorEditViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(AuthorNameMaxLength, MinimumLength = AuthorNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(AuthorBiographyMaxLength, MinimumLength = AuthorBiographyMinLength)]
    public string Biography { get; set; } = null!;

    [Required]
    public string Nationality { get; set; } = null!;

    [RegularExpression(AuthorBirthDateRegex, ErrorMessage = "Please enter a valid date and year between 1000 and 2024.")]
    public string? BirthDate { get; set; }

    [RegularExpression(AuthorWebsiteRegex, ErrorMessage = "Please enter a valid URL address.")]
    public string? Website { get; set; }

    public string? ImageUrl { get; set; }

    public DateOnly? ParsedBirthDate { get; set; }

    public ICollection<string> NationalityOptions { get; set; } = new List<string>();
}