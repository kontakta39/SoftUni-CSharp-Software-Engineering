using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.ViewModels.Movie;

public class AddMovieInputModel
{
    [Required(ErrorMessage = "The title is required.")]
    [MaxLength(50, ErrorMessage = "The title must be less than 50 characters long.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "The genre is required.")]
    [MinLength(5, ErrorMessage = "The genre must be at least 5 characters long.")]
    [MaxLength(20, ErrorMessage = "The genre must be less than 20 characters long.")]
    public string Genre { get; set; } = null!;

    [Required(ErrorMessage = "The release date is required.")]
    public string ReleaseDate { get; set; } = null!;

    [Required(ErrorMessage = "The duration is required.")]
    [Range(1, 999, ErrorMessage = "The duration must be between 1 and 999 minutes.")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "The director's name is required.")]
    [MinLength(10, ErrorMessage = "The director's name must be at least 10 characters long.")]
    [MaxLength(80, ErrorMessage = "The director's name must be less than 80 characters long.")]
    public string Director { get; set; } = null!;

    [Required(ErrorMessage = "The description is required.")]
    [MinLength(50, ErrorMessage = "The description must be at least 50 characters long.")]
    [MaxLength(500, ErrorMessage = "The description must be less than 500 characters long.")]
    public string Description { get; set; } = null!;
}