using BookWebStore.Data.Models;
using BookWebStore.ViewModels;

namespace BookWebStore.Services.Interfaces;

public interface IGenreService
{
    Task<List<Genre>> GetAllGenresAsync();

    Task<Genre?> GetGenreByIdAsync(Guid id);

    Task<bool> GenreNameExistsAsync(string name, Guid? id = null);

    Task AddGenreAsync(GenreAddViewModel addGenre);             

    Task EditGenreAsync(GenreEditViewModel editGenre, Genre genre);       

    Task DeleteGenreAsync(Genre genre);  
}