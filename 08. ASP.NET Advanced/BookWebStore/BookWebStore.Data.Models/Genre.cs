using System.ComponentModel.DataAnnotations;
using BookWebStore.Data.Entities;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Genre : IEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(GenreNameMaxLength, MinimumLength = GenreNameMinLength)]
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}