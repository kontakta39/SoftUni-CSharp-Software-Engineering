using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<Review>> GetBookReviewsAsync(Guid bookId)
    {
        return await _reviewRepository.GetBookReviewsAsync(bookId);
    }

    public async Task<Review?> ReviewExistsAsync(Guid bookId, string userId)
    {
        return await _reviewRepository.ReviewExistsAsync(bookId, userId);
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

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task EditReviewAsync(ReviewEditViewModel editReview, Review review, ApplicationUser user)
    {
        review.ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        review.Rating = editReview.Rating;
        review.ReviewText = editReview.ReviewText;
        review.IsEdited = true;

        await _reviewRepository.SaveChangesAsync();
    }
}