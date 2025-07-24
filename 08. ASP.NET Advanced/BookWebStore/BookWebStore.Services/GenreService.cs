using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _genreRepository.GetAllAsync();
    }

    public async Task<Genre?> GetGenreByIdAsync(Guid id)
    {
        return await _genreRepository.GetByIdAsync(id);
    }

    public async Task<bool> GenreNameExistsAsync(string name, Guid? id = null)
    {
        return await _genreRepository.ExistsByNameAsync(name, id);
    }

    public async Task AddGenreAsync(GenreAddViewModel addGenre)
    {
        Genre genre = new Genre
        {
            Name = addGenre.Name
        };

        await _genreRepository.AddAsync(genre);
        await _genreRepository.SaveChangesAsync();
    }

    public async Task EditGenreAsync(GenreEditViewModel editGenre, Genre genre)
    {
        genre.Name = editGenre.Name;
        await _genreRepository.SaveChangesAsync();
    }

    public async Task DeleteGenreAsync(GenreDeleteViewModel deleteGenre, Genre genre)
    {
        genre.IsDeleted = true;
        await _genreRepository.SaveChangesAsync();
    }
}