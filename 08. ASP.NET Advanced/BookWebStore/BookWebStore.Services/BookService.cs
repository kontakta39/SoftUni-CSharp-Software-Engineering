using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllAsync(); 
    }

    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<bool> BookNameExistsAsync(string title, Guid? id = null)
    {
        return await _bookRepository.ExistsByNameAsync(title, id);
    }

    public async Task AddBookAsync(BookAddViewModel addBook)
    {
        Book book = new Book()
        {
            Title = addBook.Title,
            Publisher = addBook.Publisher,
            ReleaseYear = addBook.ReleaseYear,
            PagesNumber = addBook.PagesNumber,
            ImageUrl = addBook.ImageUrl,
            Price = addBook.Price,
            Stock = addBook.Stock,
            AuthorId = addBook.AuthorId,
            GenreId = addBook.GenreId
        };

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
    }

    public async Task EditBookAsync(BookEditViewModel editBook, Book book)
    {
        book.Title = editBook.Title;
        book.Publisher = editBook.Publisher;
        book.ReleaseYear = editBook.ReleaseYear;
        book.PagesNumber = editBook.PagesNumber;
        book.ImageUrl = editBook.ImageUrl;
        book.Price = editBook.Price;
        book.Stock = editBook.Stock;
        book.AuthorId = editBook.AuthorId;
        book.GenreId = editBook.GenreId;

        await _bookRepository.SaveChangesAsync();
    }

    public async Task<bool> HasBooksInStockByGenreIdAsync(Guid genreId)
    {
        return await _bookRepository.HasBooksInStockByGenreAsync(genreId);
    }

    public async Task<bool> HasBooksInStockByAuthorIdAsync(Guid authorId)
    {
        return await _bookRepository.HasBooksInStockByAuthorAsync(authorId);
    }
}