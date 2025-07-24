using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllAsync();

    Task<Genre?> GetByIdAsync(Guid id);

    Task<bool> ExistsByNameAsync(string name, Guid? id = null);

    Task AddAsync(Genre genre);

    Task SaveChangesAsync();
}