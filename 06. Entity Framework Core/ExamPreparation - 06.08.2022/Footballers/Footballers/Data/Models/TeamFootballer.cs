using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models;

public class TeamFootballer
{
    [Key]
    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team Team { get; set; }

    [Key]
    public int FootballerId { get; set; }

    [ForeignKey(nameof(FootballerId))]
    public Footballer Footballer { get; set; }
}