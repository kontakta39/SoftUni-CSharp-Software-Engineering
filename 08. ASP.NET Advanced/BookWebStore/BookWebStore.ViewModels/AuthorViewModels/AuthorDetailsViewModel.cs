using System.ComponentModel.DataAnnotations;

namespace BookWebStore.ViewModels;

public class AuthorDetailsViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Biography { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly? BirthDate { get; set; }

    public string? Website { get; set; } 

    public string? ImageUrl { get; set; } 
}