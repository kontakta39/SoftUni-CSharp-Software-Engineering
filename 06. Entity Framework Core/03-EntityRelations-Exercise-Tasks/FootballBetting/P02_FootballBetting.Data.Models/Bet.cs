using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace P02_FootballBetting.Data.Models;

public class Bet
{
    public int BetId { get; set; }

    [Precision(18,2)]
    public decimal Amount { get; set; }
    public string Prediction { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    public int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }
}