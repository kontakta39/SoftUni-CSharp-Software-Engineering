using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

[Authorize]
public class ReviewController : Controller
{
    private readonly MusicStoreDbContext _context;

    public ReviewController(MusicStoreDbContext context)
    {
        _context = context;    
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid id)
    {
        //Display all reviews for a certain album
        Album? album = await _context.Albums
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (album == null)
        { 
            return RedirectToAction("Index", "Album");
        }

        List<ReviewIndexViewModel> allReviews = await _context.Reviews
            .Where(r => r.AlbumId == id)
            .Select(r => new ReviewIndexViewModel()
            {
                Id = r.Id,
                AlbumId = r.AlbumId,
                Username = r.User.UserName!,
                ReviewDate = r.ReviewDate,
                ReviewText = r.ReviewText,
                Rating = r.Rating,
                IsEdited = r.IsEdited
            })
            .ToListAsync();

        return View(allReviews);
    }

    [HttpGet]
    public async Task<IActionResult> AddReview(Guid id)
    {
        //Display all reviews for a certain album
        Album? album = await _context.Albums
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return RedirectToAction("Index", "Album");
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (userId == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        ReviewAddViewModel addReview = new ReviewAddViewModel();
        addReview.AlbumId = album.Id;
        addReview.AlbumTitle = album.Title;

        return View(addReview);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(ReviewAddViewModel addReview, Guid id)
    {
        Album? album = await _context.Albums
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
        {
            return RedirectToAction("Index", "Album");
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        // Check if the user has already reviewed this album
        Review? existingReview = await _context.Reviews
            .FirstOrDefaultAsync(r => r.AlbumId == id && r.UserId == userId);

        if (existingReview != null)
        {
            return RedirectToAction("Details", "Album", new { id = album.Id });
        }

        if (!ModelState.IsValid)
        {
            addReview.AlbumTitle = album.Title;
            return View(addReview);
        }

        // Create and save the review
        Review review = new Review
        {
            AlbumId = addReview.AlbumId,
            UserId = userId,
            ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ReviewText = addReview.ReviewText,
            Rating = addReview.Rating,
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { id = album.Id });
    }
}