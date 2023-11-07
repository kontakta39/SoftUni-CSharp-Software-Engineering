using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories;

public class SubjectRepository : IRepository<ISubject>
{
    private List<ISubject> subjectRepository;

    public SubjectRepository()
    {
        subjectRepository = new();
    }

    public void AddModel(ISubject model)
    {
        subjectRepository.Add(model);
    }

    public ISubject FindById(int id)
    {
        return subjectRepository.FirstOrDefault(x => x.Id == id);
    }

    public ISubject FindByName(string name)
    {
        return subjectRepository.FirstOrDefault(x => x.Name == name);
    }

    public IReadOnlyCollection<ISubject> Models => subjectRepository.AsReadOnly();
}