using BookWebStore.Data;
using BookWebStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class BookService : IBookService
{
    private readonly BookStoreDbContext _context;

    public BookService(BookStoreDbContext context)
    {
        _context = context;
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