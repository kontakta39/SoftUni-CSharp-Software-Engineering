using System.ComponentModel.DataAnnotations.Schema;
using P01_StudentSystem.Data.Models.Enumerations;

namespace P01_StudentSystem.Data.Models;

public class Homework
{
    public int HomeworkId { get; set; }
    public string Content { get; set; }
    public ContentType ContentType { get; set; }
    public DateTime SubmissionTime { get; set; }
    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; }
}