using UniversityCompetition.Models.Contracts;

namespace UniversityCompetition.Models;

public class Student : IStudent
{
    private string firstName;
    private string lastName;
    private List<int> coveredExams;
    private IUniversity universityToJoin;

    public Student(int stidentId, string firstName, string lastName)
    {
        Id = stidentId;
        FirstName = firstName;
        LastName = lastName;
        coveredExams = new();
    }

    public int Id { get; private set; }

    public string FirstName
    {
        get => firstName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            firstName = value;
        }
    }

    public string LastName
    {
        get => lastName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            lastName = value;
        }
    }

    public IReadOnlyCollection<int> CoveredExams => coveredExams.AsReadOnly();

    public IUniversity University => universityToJoin;

    public void CoverExam(ISubject subject)
    {
        coveredExams.Add(subject.Id);
    }

    public void JoinUniversity(IUniversity university)
    {
        universityToJoin = university;
    }
}