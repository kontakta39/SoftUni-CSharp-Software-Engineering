using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class BookService : IBookService
{
    private readonly BookStoreDbContext _context;

    public BookService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => !b.IsDeleted && b.Stock > 0)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<bool> BookNameExistsAsync(string title, Guid? id = null)
    {
        return await _context.Books
            .AnyAsync(b => b.Title.ToLower() == title.ToLower() &&
            (id == null || b.Id != id) && !b.IsDeleted);
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

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
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

        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasBooksInStockByGenreIdAsync(Guid genreId)
    {
        return await _context.Books
            .AnyAsync(b => b.GenreId == genreId && !b.IsDeleted && b.Stock > 0);
    }

    public async Task<bool> HasBooksInStockByAuthorIdAsync(Guid authorId)
    {
        return await _context.Books
            .AnyAsync(b => b.AuthorId == authorId && !b.IsDeleted && b.Stock > 0);
    }
}