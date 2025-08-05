using BookWebStore.Data;
using BookWebStore.Data.Entities;
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

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => !b.IsDeleted && b.Stock > 0)
            .ToListAsync();
    }

    public async Task<List<Book>> SearchByTitleAsync(string loweredTerm)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => b.Title.ToLower().Contains(loweredTerm) && !b.IsDeleted && b.Stock > 0)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(Guid id)
    { 
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<bool> HasBooksInStockByPropertyIdAsync(string propertyName, Guid nameId)
    {
        return await _context.Books
            .AnyAsync(b => EF.Property<Guid>(b, propertyName) == nameId && !b.IsDeleted && b.Stock > 0);
    }
}