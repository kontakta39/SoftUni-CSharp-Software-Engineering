using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data.Models;

namespace MusicWebStore.Data;

public class MusicStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderAlbum> OrdersAlbums { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
                .Entity<Genre>()
                .HasData(
                    new Genre { Name = "Blues" },
                    new Genre { Name = "Heavy Metal" },
                    new Genre { Name = "Jazz" },
                    new Genre { Name = "Pop" },
                    new Genre { Name = "Rock" });
    }
}