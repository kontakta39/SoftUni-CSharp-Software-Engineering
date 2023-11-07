using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Team
{
    public int TeamId { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }

    [MaxLength(4)]
    public string Initials { get; set; }

    [Precision(18, 2)]
    public decimal Budget { get; set; }
    public int PrimaryKitColorId { get; set; }

    [ForeignKey(nameof(PrimaryKitColorId))]
    public virtual Color PrimaryKitColor { get; set; }

    [ForeignKey(nameof(SecondaryKitColorId))]
    public virtual Color SecondaryKitColor { get; set; }
    public int SecondaryKitColorId { get; set; }

    public int TownId { get; set; }

    [ForeignKey(nameof(TownId))]
    public virtual Town Town { get; set; }

    public virtual ICollection<Player> Players { get; set; }

    [InverseProperty(nameof(Game.HomeTeam))]
    public virtual ICollection<Game> HomeGames { get; set; }

    [InverseProperty(nameof(Game.AwayTeam))]
    public virtual ICollection<Game> AwayGames { get; set; }
}