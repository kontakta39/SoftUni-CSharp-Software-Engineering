using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaWebApp.Data.Models;

public class Movie
{
    //First option to initialize Guid in the constructor
    /*public Movie() 
    {
      Id = Guid.NewGuid();
    }*/

    //Second option to initialize Guid in the property
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Genre { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public string Director { get; set; } = null!;
     
    public int Duration { get; set; }

    public string Description { get; set; } = null!;

    public ICollection<CinemaMovie> CinemasMovies { get; set; } = new HashSet<CinemaMovie>();
}