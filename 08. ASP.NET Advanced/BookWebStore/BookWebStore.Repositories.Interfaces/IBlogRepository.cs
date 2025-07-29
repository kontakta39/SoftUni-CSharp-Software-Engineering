using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IBlogRepository
{
    Task<List<Blog>> GetAllAsync();

    Task<Blog?> GetByIdAsync(Guid id);

    Task AddAsync(Blog blog);

    Task SaveChangesAsync();
}