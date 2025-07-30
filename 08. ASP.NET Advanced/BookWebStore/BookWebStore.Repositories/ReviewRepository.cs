using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly BookStoreDbContext _context;

    public ReviewRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetBookReviewsAsync(Guid bookId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();
    }

    public async Task<Review?> ReviewExistsAsync(Guid bookId, string userId)
    {
        return await _context.Reviews
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);
    }
}