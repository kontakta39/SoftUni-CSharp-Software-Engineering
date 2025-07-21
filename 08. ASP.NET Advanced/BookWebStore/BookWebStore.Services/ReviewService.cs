using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class ReviewService : IReviewService
{
    private readonly BookStoreDbContext _context;

    public ReviewService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetBookReviewsAsync(Guid bookId)
    {
        List<Review> bookReviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();

        return bookReviews;
    }
}