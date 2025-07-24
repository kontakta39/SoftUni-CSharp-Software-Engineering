using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly BookStoreDbContext _context;

    public GenreRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        return await _context.Genres
            .Where(g => !g.IsDeleted)
            .ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? id = null)
    {
        return await _context.Genres
            .AnyAsync(g => g.Name.ToLower() == name.ToLower() &&
            (id == null || g.Id != id) && !g.IsDeleted);
    }

    public async Task AddAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}