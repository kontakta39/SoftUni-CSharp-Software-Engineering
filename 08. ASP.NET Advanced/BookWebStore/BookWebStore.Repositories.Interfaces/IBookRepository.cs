using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetAllBooksAsync();

    Task<List<Book>> SearchByTitleAsync(string loweredTerm);

    Task<Book?> GetBookByIdAsync(Guid id);

    Task<bool> HasBooksInStockByPropertyIdAsync(string propertyName, Guid nameId);
}