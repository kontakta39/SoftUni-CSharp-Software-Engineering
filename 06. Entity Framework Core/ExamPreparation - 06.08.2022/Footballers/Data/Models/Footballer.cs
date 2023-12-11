using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models;

public class Footballer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    public DateTime ContractStartDate { get; set; }

    [Required]
    public DateTime ContractEndDate { get; set; }

    [Required]
    [Range(0, 3)]
    public PositionType PositionType { get; set; }

    [Required]
    [Range(0, 4)]
    public BestSkillType BestSkillType { get; set; }

    [Required]
    public int CoachId { get; set; }

    [ForeignKey(nameof(CoachId))]
    public Coach Coach { get; set; }

    public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new List<TeamFootballer>();
}