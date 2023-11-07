using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories;

public class UniversityRepository : IRepository<IUniversity>
{
    private List<IUniversity> universityRepository;

    public UniversityRepository()
    {
        universityRepository = new();
    }

    public void AddModel(IUniversity university)
    {
        universityRepository.Add(university);
    }

    public IUniversity FindById(int id)
    {
        return universityRepository.FirstOrDefault(x => x.Id == id);
    }

    public IUniversity FindByName(string name)
    {
        return universityRepository.FirstOrDefault(x => x.Name == name);
    }

    public IReadOnlyCollection<IUniversity> Models => universityRepository.AsReadOnly();
}