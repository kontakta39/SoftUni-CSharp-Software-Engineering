using BookWebStore.Data.Entities;

namespace BookWebStore.Repositories.Interfaces;

public interface IBaseRepository
{
    Task<List<TType>> GetAllAsync<TType>() where TType : class, IEntity;

    Task<List<TType>> SearchByPropertyAsync<TType>(string propertyName, string loweredTerm)
    where TType : class, IEntity;

    Task<TType?> GetByIdAsync<TType>(Guid id) where TType : class, IEntity;

    Task<bool> ExistsByPropertyAsync<TType>(string propertyName, string nameKey, Guid? id = null) 
    where TType : class, IEntity;

    Task AddAsync<TType>(TType entity) where TType : class;

    void Remove<TType>(TType entity) where TType : class;

    Task SaveChangesAsync();
}