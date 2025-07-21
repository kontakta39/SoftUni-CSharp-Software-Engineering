using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Data.Models;

public class Book
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

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

    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "You have to enter price between 1 and 200.")]
    [Range(BookMinPrice, BookMaxPrice)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "You have to enter stock quantity between 1 and 100.")]
    [Range(BookStockMinLength, BookStockMaxLength)]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Please select a genre.")]
    public Guid GenreId { get; set; }

    [ForeignKey(nameof(GenreId))]
    public Genre Genre { get; set; } = null!;

    [Required(ErrorMessage = "Please select an author.")]
    public Guid AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public Author Author { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public ICollection<OrderBook> OrdersBooks { get; set; } = new List<OrderBook>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}