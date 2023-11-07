using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;
using System.Numerics;

namespace P02_FootballBetting.Data;

public class FootballBettingContext : DbContext
{
    public FootballBettingContext(DbContextOptions<FootballBettingContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<PlayerStatistic> PlayersStatistics { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Bet> Bets { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerStatistic>()
            .HasKey(pk => new { pk.GameId, pk.PlayerId });
    }
}