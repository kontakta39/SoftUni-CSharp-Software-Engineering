using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetAllBooksAsync();

    Task<Book?> GetBookByIdAsync(Guid id);

    Task<bool> HasBooksInStockByPropertyIdAsync(string propertyName, Guid nameId);
}