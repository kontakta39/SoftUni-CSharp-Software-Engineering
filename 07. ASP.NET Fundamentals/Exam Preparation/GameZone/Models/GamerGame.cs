using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Models;

[PrimaryKey(nameof(GameId), nameof(GamerId))]
public class GamerGame
{
    [Key]
    [Required]
    public int GameId { get; set; }

    [ForeignKey("GameId")]
    public Game Game { get; set; } = null!;

    [Key]
    [Required]
    public string GamerId { get; set; } = null!;

    [ForeignKey("GamerId")]
    public IdentityUser Gamer { get; set; } = null!;
}