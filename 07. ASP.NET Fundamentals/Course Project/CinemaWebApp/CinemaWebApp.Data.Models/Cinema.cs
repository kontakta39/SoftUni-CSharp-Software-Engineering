using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.Data.Models;

public class Cinema
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;

    public virtual ICollection<CinemaMovie> CinemasMovies { get; set; } = new HashSet<CinemaMovie>();
}
