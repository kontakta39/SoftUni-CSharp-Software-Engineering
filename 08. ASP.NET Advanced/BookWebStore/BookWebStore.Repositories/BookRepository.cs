using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookStoreDbContext _context;

    public BookRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => !b.IsDeleted && b.Stock > 0)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    { 
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<bool> ExistsByNameAsync(string title, Guid? id = null)
    {
        return await _context.Books
            .AnyAsync(b => b.Title.ToLower() == title.ToLower() &&
            (id == null || b.Id != id) && !b.IsDeleted);
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasBooksInStockByGenreAsync(Guid genreId)
    {
        return await _context.Books
            .AnyAsync(b => b.GenreId == genreId && !b.IsDeleted && b.Stock > 0);
    }

    public async Task<bool> HasBooksInStockByAuthorAsync(Guid authorId)
    {
        return await _context.Books
            .AnyAsync(b => b.AuthorId == authorId && !b.IsDeleted && b.Stock > 0);
    }
}