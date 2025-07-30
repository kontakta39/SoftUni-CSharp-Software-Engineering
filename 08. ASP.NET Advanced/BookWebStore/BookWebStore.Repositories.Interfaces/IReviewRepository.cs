using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IReviewRepository
{
    Task<List<Review>> GetBookReviewsAsync(Guid bookId);

    Task<Review?> ReviewExistsAsync(Guid bookId, string userId);
}