using System.ComponentModel.DataAnnotations;
using BookWebStore.Data.Entities;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Author : IEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(AuthorNameMaxLength, MinimumLength = AuthorNameMinLength)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(AuthorBiographyMaxLength, MinimumLength = AuthorBiographyMinLength)]
    public string Biography { get; set; } = null!;

    [Required]
    public string Nationality { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string? Website { get; set; }

    public string? ImageUrl { get; set; } 

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}