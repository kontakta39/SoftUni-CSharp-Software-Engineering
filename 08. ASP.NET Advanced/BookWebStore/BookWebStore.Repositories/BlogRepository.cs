using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BookStoreDbContext _context;

    public BlogRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _context.Blogs
           .Include(b => b.Publisher)
           .Where(b => !b.IsDeleted)
           .ToListAsync();
    }

    public async Task<Blog?> GetBlogByIdAsync(Guid id)
    {
        return await _context.Blogs
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }
}