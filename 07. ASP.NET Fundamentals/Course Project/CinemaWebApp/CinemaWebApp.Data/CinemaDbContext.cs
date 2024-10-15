using CinemaWebApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Data;

public class CinemaDbContext : DbContext
{
    public CinemaDbContext()
    {
    }
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; } = null!;
    public virtual DbSet<Cinema> Cinemas { get; set; } = null!;
    public virtual DbSet<CinemaMovie> CinemasMovies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>().HasData(
      new Movie
      {
          Title = "Harry Potter and the Goblet of Fire",
          Genre = "Fantasy",
          ReleaseDate = new DateTime(2005, 11, 10),
          Director = "Mike Newel",
          Duration = 157,
          Description = "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling. It is the sequel to Harry Potter and the Prisoner of Azkaban (2004) and the fourth instalment in the Harry Potter film series."
      },
        new Movie
        {
            Title = "Lord of the Rings",
            Genre = "Fantasy",
            ReleaseDate = new DateTime(2001, 05, 22),
            Director = "Peter Jackson",
            Duration = 178,
            Description = "The Lord of the Rings is a trilogy of epic fantasy adventure films directed by Peter Jackson, based on the novel The Lord of the Rings by British author J. R. R. Tolkien. The films are subtitled The Fellowship of the Ring (2001), The Two Towers (2002), and The Return of the King (2003)."
        }
      );

        modelBuilder.Entity<Cinema>().HasData(
       new Cinema
       {
           Name = "Cinema City",
           Location = "Sofia"
       },
       new Cinema
       {
           Name = "Cinema City",
           Location = "Plovdiv"
       },
        new Cinema
        {
            Name = "Latona Cinema",
            Location = "Veliko Tarnovo"
        }
       );

        modelBuilder.Entity<CinemaMovie>()
       .HasKey(cm => new { cm.CinemaId, cm.MovieId });

        modelBuilder.Entity<CinemaMovie>()
           .HasOne(cm => cm.Cinema)
           .WithMany(c => c.CinemasMovies) // Assuming Cinema has a collection of CinemaMovies
           .HasForeignKey(cm => cm.CinemaId)
           .OnDelete(DeleteBehavior.Restrict);  // Restrict delete

        modelBuilder.Entity<CinemaMovie>()
            .HasOne(cm => cm.Movie)
            .WithMany(m => m.CinemasMovies) // Assuming Movie has a collection of CinemaMovies
            .HasForeignKey(cm => cm.MovieId)
            .OnDelete(DeleteBehavior.Restrict);  // Restrict delete
    }
}