using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(Guid id);

    Task<bool> ExistsByNameAsync(string title, Guid? id = null);

    Task AddAsync(Book book);

    Task SaveChangesAsync();

    Task<bool> HasBooksInStockByGenreAsync(Guid genreId);

    Task<bool> HasBooksInStockByAuthorAsync(Guid authorId);
}