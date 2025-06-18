using System.ComponentModel.DataAnnotations;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Author
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(AuthorNameMaxLength, MinimumLength = AuthorNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(AuthorBiographyMaxLength, MinimumLength = AuthorBiographyMinLength)]
    public string Biography { get; set; } = null!;

    [StringLength(AuthorNationalityMaxLength, MinimumLength = AuthorNationalityMinLength)]
    public string? Nationality { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly? BirthDate { get; set; }

    [StringLength(AuthorWebsiteMaxLength, MinimumLength = AuthorWebsiteMinLength)]
    public string? Website { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
}