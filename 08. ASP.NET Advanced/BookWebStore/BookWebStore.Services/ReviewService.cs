using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
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

    public async Task<Review?> ReviewExistsAsync(Guid bookId, string userId)
    {
        //Check if the user has already reviewed this book
        return await _context.Reviews
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);
    }

    public async Task AddReviewAsync(ReviewAddViewModel addReview, ApplicationUser user)
    {
        Review review = new Review
        {
            BookId = addReview.BookId,
            UserId = user.Id,
            Rating = addReview.Rating,
            ReviewText = addReview.ReviewText
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task EditReviewAsync(ReviewEditViewModel editReview, Review review, ApplicationUser user)
    {
        review.ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        review.Rating = editReview.Rating;
        review.ReviewText = editReview.ReviewText;
        review.IsEdited = true;

        await _context.SaveChangesAsync();
    }
}