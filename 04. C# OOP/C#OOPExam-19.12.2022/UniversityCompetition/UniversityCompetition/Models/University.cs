using UniversityCompetition.Models.Contracts;

namespace UniversityCompetition.Models;

public class University : IUniversity
{
    private string name;
    private string category;
    private List<int> requiredSubjects;
    private int capacity;

    public University(int universityId, string univarsityName, string category, int capacity, List<int> subjects)
    {
        Id = universityId;
        Name = univarsityName;
        Category = category;
        Capacity = capacity;
        requiredSubjects = subjects;

    }

    public int Id { get; private set; }

    public string Name
    {
        get => name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or whitespace!");
            }

            name = value;
        }
    }

    public string Category
    {
        get => category;
        private set
        {
            if (value.ToLower() != "Technical".ToLower() && value.ToLower() != "Economical".ToLower()
                && value.ToLower() != "Humanity".ToLower())
            {
                throw new ArgumentException($"University category {value} is not allowed in the application!");
            }

            category = value;
        }
    }

    public int Capacity
    {
        get => capacity;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("University capacity cannot be a negative value!");
            }

            capacity = value;
        }
    }

    public IReadOnlyCollection<int> RequiredSubjects => requiredSubjects.AsReadOnly();
}