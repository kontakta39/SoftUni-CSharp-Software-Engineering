using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class ReviewService : IReviewInterface
{
    private readonly MusicStoreDbContext _context;

    public ReviewService(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<ReviewAddViewModel> Add(Guid id, string userId)
    {
        Album? album = await _context.Albums
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            throw new ArgumentNullException();
        }

        //Check if the user has already reviewed this album
        Review? existingReview = await _context.Reviews
            .FirstOrDefaultAsync(r => r.AlbumId == id && r.UserId == userId);

        if (existingReview != null)
        {
            throw new ArgumentNullException();
        }

        ReviewAddViewModel addReview = new ReviewAddViewModel();
        addReview.AlbumId = album.Id;
        addReview.AlbumTitle = album.Title;

        return addReview;
    }

    public async Task Add(ReviewAddViewModel addReview, Guid albumId, string userId)
    {
        Album? album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId && a.IsDeleted == false);

        if (album == null)
        {
            throw new ArgumentNullException();
        }

        Review review = new Review
        {
            AlbumId = album.Id,
            UserId = userId,
            ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ReviewText = addReview.ReviewText,
            Rating = addReview.Rating
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task<ReviewEditViewModel> Edit(Guid id, Guid albumId)
    {
        //Find the review that should be edited
        Review? findReview = await _context.Reviews
            .Where(r => r.Id == id && r.IsEdited == false)
            .FirstOrDefaultAsync();

        if (findReview == null)
        {
            throw new ArgumentNullException();
        }

        //Find the certain album, where the review is published
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            throw new ArgumentNullException();
        }

        ReviewEditViewModel editReview = new ReviewEditViewModel()
        {
            AlbumId = albumId,
            AlbumTitle = album.Title,
            ReviewText = findReview.ReviewText,
            Rating = findReview.Rating
        };

        return editReview;
    }

    public async Task Edit(ReviewEditViewModel editReview, Guid id, Guid albumId, string userId)
    {
        Album? album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId && a.IsDeleted == false);

        if (album == null)
        {
            throw new ArgumentNullException();
        }

        //Find the review that should be edited
        Review? findReview = await _context.Reviews
            .Where(r => r.Id == id && r.IsEdited == false)
            .FirstOrDefaultAsync();

        if (findReview == null)
        {
            throw new ArgumentNullException();
        }

        findReview.AlbumId = albumId;
        findReview.UserId = userId;
        findReview.ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        findReview.ReviewText = editReview.ReviewText;
        findReview.Rating = editReview.Rating;
        findReview.IsEdited = true;

        await _context.SaveChangesAsync();
    }
}