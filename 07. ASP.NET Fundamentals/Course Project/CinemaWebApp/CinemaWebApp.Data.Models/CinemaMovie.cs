using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.Data.Models;

public class CinemaMovie
{
    [Key]
    public Guid CinemaId { get; set; }
    public Cinema Cinema { get; set; } = null!;

    [Key]
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}