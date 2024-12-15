using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class GenreService : IGenreService
{
    private readonly MusicStoreDbContext _context;

    public GenreService(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<GenreIndexViewModel>> Index()
    {
        List<GenreIndexViewModel> genres = await _context.Genres
           .Where(g => g.IsDeleted == false)
           .Select(g => new GenreIndexViewModel()
           {
               Id = g.Id,
               Name = g.Name
           })
           .OrderBy(g => g.Name)
           .AsNoTracking()
           .ToListAsync();

        return genres;
    }

    public async Task Add(GenreAddViewModel addGenre)
    {
        Genre genre = new Genre()
        {
            Name = addGenre.Name
        };

        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
    }

    public async Task<GenreEditViewModel> Edit(Guid id)
    {
        Genre? genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id && g.IsDeleted == false);

        if (genre == null)
        {
            return null;
        }

        GenreEditViewModel editModel = new GenreEditViewModel()
        {
            Name = genre.Name
        };

        return editModel;
    }

    public async Task Edit(GenreEditViewModel editModel, Guid id)
    {
        Genre genre = _context.Genres.FirstOrDefault(g => g.Id == id && g.IsDeleted == false)!;
        genre.Name = editModel.Name;

        await _context.SaveChangesAsync();
    }

    public async Task<GenreDeleteViewModel> Delete(Guid id)
    {
        GenreDeleteViewModel? genre = await _context.Genres
        .Where(g => g.Id == id && g.IsDeleted == false)
        .Select(g => new GenreDeleteViewModel()
        {
            Id = id,
            Name = g.Name
        })
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (genre == null)
        {
            return null;
        }

        return genre;
    }

    public async Task Delete(GenreDeleteViewModel model)
    {
        Genre? genre = await _context.Genres
        .Where(g => g.Id == model.Id && g.IsDeleted == false)
        .FirstOrDefaultAsync();

        if (genre != null)
        {
            genre.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}