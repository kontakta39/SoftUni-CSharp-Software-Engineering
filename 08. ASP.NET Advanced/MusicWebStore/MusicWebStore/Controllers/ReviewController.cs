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
    public async Task<IActionResult> Add(Guid id)
    {
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

        // Check if the user has already reviewed this album
        Review? existingReview = await _context.Reviews
            .FirstOrDefaultAsync(r => r.AlbumId == id && r.UserId == userId);

        if (existingReview != null)
        {
            return RedirectToAction("Details", "Album", new { id = album.Id });
        }

        ReviewAddViewModel addReview = new ReviewAddViewModel();
        addReview.AlbumId = album.Id;
        addReview.AlbumTitle = album.Title;

        return View(addReview);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ReviewAddViewModel addReview, Guid id)
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

        if (!ModelState.IsValid)
        {
            addReview.AlbumTitle = album.Title;
            addReview.AlbumId = album.Id;
            return View(addReview);
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

        return RedirectToAction("Details", "Album", new { id = album.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, Guid albumId)
    {
        //Find the review that should be edited
        Review? findReview = await _context.Reviews
            .Where(r => r.Id == id && r.IsEdited == false)
            .FirstOrDefaultAsync();

        if (findReview == null)
        {
            return RedirectToAction("Index", "Album");
        }

        //Find the certain album, where the review is published
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
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

        ReviewEditViewModel editReview = new ReviewEditViewModel()
        { 
            AlbumId = albumId,
            ReviewText = findReview.ReviewText,
            Rating = findReview.Rating
        };

        editReview.AlbumTitle = album.Title;

        return View(editReview);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ReviewEditViewModel editReview, Guid id, Guid albumId)
    {
        //Find the review that should be edited
        Review? findReview = await _context.Reviews
            .Where(r => r.Id == id && r.IsEdited == false)
            .FirstOrDefaultAsync();

        if (findReview == null)
        {
            return RedirectToAction("Index", "Album");
        }

        //Find the certain album, where the review is published
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
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

        if (!ModelState.IsValid)
        {
            editReview.AlbumTitle = album.Title;
            return View(editReview);
        }

        findReview.AlbumId = album.Id;
        findReview.UserId = userId;
        findReview.ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        findReview.ReviewText = editReview.ReviewText;
        findReview.Rating = editReview.Rating;
        findReview.IsEdited = true;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Album", new { id = album.Id });
    }
}