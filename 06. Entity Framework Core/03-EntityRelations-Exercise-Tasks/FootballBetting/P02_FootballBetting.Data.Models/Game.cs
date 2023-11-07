﻿using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    public int GameId { get; set; }
    public int HomeTeamId { get; set; }
    [ForeignKey(nameof(HomeTeamId))]
    public virtual Team HomeTeam { get; set; }

    public int AwayTeamId { get; set; }
    [ForeignKey(nameof(AwayTeamId))]
    public virtual Team AwayTeam { get; set; }

    public int HomeTeamGoals { get; set; }
    public int AwayTeamGoals { get; set; }
    public DateTime DateTime { get; set; }
    public double HomeTeamBetRate { get; set; }
    public double AwayTeamBetRate { get; set; }
    public double DrawBetRate { get; set; }
    public int Result { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }
    public virtual ICollection<Bet> Bets { get; set; }
}