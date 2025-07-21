using BookWebStore.Data.Models;

namespace BookWebStore.Services.Interfaces;

public interface IReviewService
{
    Task<List<Review>> GetBookReviewsAsync(Guid bookId);
}