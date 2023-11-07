namespace UniversityCompetition.Models;

public class TechnicalSubject : Subject
{
    public const double subjectRate = 1.3;

    public TechnicalSubject(int subjectId, string subjectName) 
        : base(subjectId, subjectName, subjectRate)
    {
    }
}