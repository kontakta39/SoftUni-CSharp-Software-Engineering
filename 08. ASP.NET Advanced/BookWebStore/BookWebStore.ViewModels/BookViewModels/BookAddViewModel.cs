using System.ComponentModel.DataAnnotations;
using BookWebStore.Data.Models;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.ViewModels;

public class BookAddViewModel
{
    [Required]
    [StringLength(BookTitleMaxLength, MinimumLength = BookTitleMinLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(BookPublisherMaxLength, MinimumLength = BookPublisherMinLength)]
    public string Publisher { get; set; } = null!;

    [Required(ErrorMessage = "You have to enter release year between 1000 and 2025.")]
    [Range(BookMinReleaseYear, BookMaxReleaseYear)]
    public int ReleaseYear { get; set; }

    [Required(ErrorMessage = "You have to enter pages number between 50 and 1000.")]
    [Range(BookPagesNumberMinLength, BookPagesNumberMaxLength)]
    public int PagesNumber { get; set; }

    public string? ImageUrl { get; set; } = null!;

    [Required(ErrorMessage = "You have to enter price between 1 and 200.")]
    [Range(BookMinPrice, BookMaxPrice)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "You have to enter stock quantity between 1 and 100.")]
    [Range(BookStockMinLength, BookStockMaxLength)]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Please select a genre.")]
    public Guid GenreId { get; set; }

    [Required(ErrorMessage = "Please select an author.")]
    public Guid AuthorId { get; set; }

    public ICollection<Author> Authors { get; set; } = new List<Author>();

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}