using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P01_StudentSystem.Data.Models.Enumerations;

namespace P01_StudentSystem.Data.Models;

public class Resource
{
    public int ResourceId { get; set; }

    [StringLength(50)]
    public string Name { get; set; }
    public string Url { get; set; }
    public ResourceType ResourceType { get; set; }
    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
     public virtual Course Course { get; set; }
}