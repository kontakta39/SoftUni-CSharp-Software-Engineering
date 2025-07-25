using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAsync();

    Task<Author?> GetByIdAsync(Guid id);

    Task<bool> ExistsByNameAsync(string name, Guid? id = null);

    Task AddAsync(Author author);

    Task SaveChangesAsync();
}