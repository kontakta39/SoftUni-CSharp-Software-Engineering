using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class GenreService : IGenreService
{
    private readonly IBaseRepository _baseRepository;

    public GenreService(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _baseRepository.GetAllAsync<Genre>();
    }

    public async Task<List<Genre>> SearchGenresAsync(string loweredTerm)
    {
        return await _baseRepository.SearchByPropertyAsync<Genre>("Name", loweredTerm);
    }

    public async Task<Genre?> GetGenreByIdAsync(Guid id)
    {
        return await _baseRepository.GetByIdAsync<Genre>(id);
    }

    public async Task<bool> GenreNameExistsAsync(string name, Guid? id = null)
    {
        return await _baseRepository.ExistsByPropertyAsync<Genre>("Name", name, id);
    }

    public async Task AddGenreAsync(GenreAddViewModel addGenre)
    {
        Genre genre = new Genre
        {
            Name = addGenre.Name
        };

        await _baseRepository.AddAsync(genre);
        await _baseRepository.SaveChangesAsync();
    }

    public async Task EditGenreAsync(GenreEditViewModel editGenre, Genre genre)
    {
        genre.Name = editGenre.Name;
        await _baseRepository.SaveChangesAsync();
    }

    public async Task DeleteGenreAsync(Genre genre)
    {
        genre.IsDeleted = true;
        await _baseRepository.SaveChangesAsync();
    }
}