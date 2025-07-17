using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class GenreService : IGenreService
{
    private readonly BookStoreDbContext _context;

    public GenreService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _context.Genres
            .Where(g => !g.IsDeleted)
            .ToListAsync();
    }

    public async Task<Genre?> GetGenreByIdAsync(Guid id)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
    }

    public async Task<bool> GenreNameExistsAsync(string name, Guid? id = null)
    {
        return await _context.Genres
            .AnyAsync(g => g.Name.ToLower() == name.ToLower() &&
            (id == null || g.Id != id) && !g.IsDeleted);
    }

    public async Task AddGenreAsync(GenreAddViewModel addGenre)
    {
        Genre genre = new Genre
        {
            Name = addGenre.Name
        };

        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
    }

    public async Task EditGenreAsync(GenreEditViewModel editGenre, Genre genre)
    {
        genre.Name = editGenre.Name;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGenreAsync(GenreDeleteViewModel deleteGenre, Genre genre)
    {
        genre.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}