using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IReviewInterface
{
    Task<ReviewAddViewModel> Add(Guid id, string userId);
    Task Add(ReviewAddViewModel addReview, Guid albumId, string userId);
    Task<ReviewEditViewModel> Edit(Guid id, Guid albumId);
    Task Edit(ReviewEditViewModel editReview, Guid id, Guid albumId, string userId);
}