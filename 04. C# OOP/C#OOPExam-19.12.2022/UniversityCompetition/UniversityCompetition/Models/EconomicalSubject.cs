namespace UniversityCompetition.Models;

public class EconomicalSubject : Subject
{
    public const double subjectRate = 1.0;

    public EconomicalSubject(int subjectId, string subjectName) 
        : base(subjectId, subjectName, subjectRate)
    {
    }
}