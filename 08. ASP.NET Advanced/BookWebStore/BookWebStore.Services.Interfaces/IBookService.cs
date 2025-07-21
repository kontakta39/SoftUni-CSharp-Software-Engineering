using BookWebStore.Data.Models;
using BookWebStore.ViewModels;

namespace BookWebStore.Services.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();

    Task<Book?> GetBookByIdAsync(Guid id);

    Task<bool> BookNameExistsAsync(string title, Guid? id = null);

    Task AddBookAsync(BookAddViewModel addBook);

    Task EditBookAsync(BookEditViewModel editBook, Book book);

    Task<bool> HasBooksInStockByGenreIdAsync(Guid genreId);

    Task<bool> HasBooksInStockByAuthorIdAsync(Guid authorId);
}