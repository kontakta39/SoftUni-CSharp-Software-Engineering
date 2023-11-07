using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories;

public class StudentRepository : IRepository<IStudent>
{
    private List<IStudent> studentRepository;

    public StudentRepository()
    {
        studentRepository = new();
    }

    public void AddModel(IStudent student)
    {
        studentRepository.Add(student);
    }

    public IStudent FindById(int id)
    {
        return studentRepository.FirstOrDefault(x => x.Id == id);
    }

    public IStudent FindByName(string name)
    {
        string[] currentName = name.Split(" "); 
        return studentRepository.FirstOrDefault(x => x.FirstName == currentName[0] && x.LastName == currentName[1]);
    }

    public IReadOnlyCollection<IStudent> Models => studentRepository.AsReadOnly();
}