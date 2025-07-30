using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IBlogRepository
{
    Task<List<Blog>> GetAllBlogsAsync();

    Task<Blog?> GetBlogByIdAsync(Guid id);
}