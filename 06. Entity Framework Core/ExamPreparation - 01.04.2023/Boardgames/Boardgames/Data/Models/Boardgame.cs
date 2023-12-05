using Boardgames.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models;

public class Boardgame
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 10)]
    public string Name { get; set; }

    [Required]
    [Range(1, 10.00)]
    public double Rating { get; set; }

    [Required]
    [Range(2018, 2023)]
    public int YearPublished { get; set; }

    [Required]
    public CategoryType CategoryType { get; set; }

    [Required]
    public string Mechanics { get; set; }

    [Required]
    public int CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))]
    public Creator Creator { get; set; }

    public ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new List<BoardgameSeller>();
}