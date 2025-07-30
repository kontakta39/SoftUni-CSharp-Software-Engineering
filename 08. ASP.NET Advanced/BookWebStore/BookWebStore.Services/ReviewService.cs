using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBaseRepository _baseRepository;

    public ReviewService(IReviewRepository reviewRepository, IBaseRepository baseRepository)
    {
        _reviewRepository = reviewRepository;
        _baseRepository = baseRepository;
    }

    public async Task<List<Review>> GetBookReviewsAsync(Guid bookId)
    {
        return await _reviewRepository.GetBookReviewsAsync(bookId);
    }

    public async Task<Review?> ReviewExistsAsync(Guid bookId, string userId)
    {
        return await _reviewRepository.ReviewExistsAsync(bookId, userId);
    }

    public async Task AddReviewAsync(ReviewAddViewModel addReview, string userId)
    {
        Review review = new Review
        {
            BookId = addReview.BookId,
            UserId = userId,
            Rating = addReview.Rating,
            ReviewText = addReview.ReviewText
        };

        await _baseRepository.AddAsync(review);
        await _baseRepository.SaveChangesAsync();
    }

    public async Task EditReviewAsync(ReviewEditViewModel editReview, Review review)
    {
        review.ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        review.Rating = editReview.Rating;
        review.ReviewText = editReview.ReviewText;
        review.IsEdited = true;

        await _baseRepository.SaveChangesAsync();
    }
}