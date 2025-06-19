using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class AuthorAddViewModel
{
    [Required]
    [StringLength(AuthorNameMaxLength, MinimumLength = AuthorNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(AuthorBiographyMaxLength, MinimumLength = AuthorBiographyMinLength)]
    public string Biography { get; set; } = null!;

    [Required]
    [StringLength(AuthorNationalityMaxLength, MinimumLength = AuthorNationalityMinLength)]
    public string Nationality { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly? BirthDate { get; set; } = null!;

    [StringLength(AuthorWebsiteMaxLength, MinimumLength = AuthorWebsiteMinLength)]
    public string? Website { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    public ICollection<string> NationalityOptions { get; set; } = new List<string>();
}