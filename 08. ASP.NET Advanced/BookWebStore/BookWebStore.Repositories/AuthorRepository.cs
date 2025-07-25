using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookStoreDbContext _context;

    public AuthorRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllAsync()
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? id = null)
    {
        return await _context.Authors
            .AnyAsync(a => a.Name.ToLower() == name.ToLower() &&
            (id == null || a.Id != id) && !a.IsDeleted);
    }

    public async Task AddAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}