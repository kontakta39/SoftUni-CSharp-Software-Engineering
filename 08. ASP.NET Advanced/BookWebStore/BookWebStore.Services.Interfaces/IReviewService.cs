using BookWebStore.Data.Models;
using BookWebStore.ViewModels;

namespace BookWebStore.Services.Interfaces;

public interface IReviewService
{
    Task<List<Review>> GetBookReviewsAsync(Guid bookId);

    Task<Review?> ReviewExistsAsync(Guid bookId, string userId);

    Task AddReviewAsync(ReviewAddViewModel addReview, string userId);

    Task EditReviewAsync(ReviewEditViewModel editReview, Review review);
}