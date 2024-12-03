using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

public class ReviewController : Controller
{
    private readonly MusicStoreDbContext _context;

    public ReviewController(MusicStoreDbContext context)
    {
        _context = context;    
    }

    public async Task<IActionResult> Index(Guid id)
    {
        //Display all reviews for a certain album
        Album? album = await _context.Albums
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (album == null)
        { 
            RedirectToAction("Index", "Album");
        }

        List<ReviewIndexViewModel> allReviews = await _context.Reviews
            .Where(r => r.AlbumId == id)
            .Select(r => new ReviewIndexViewModel()
            {
                Id = r.Id,
                AlbumId = r.AlbumId,
                UserId = r.UserId,
                ReviewDate = r.ReviewDate,
                ReviewText = r.ReviewText,
                Rating = r.Rating,
                isCommented = r.isCommented,
                IsEdited = r.IsEdited
            })
            .ToListAsync();

        return View(allReviews);
    }
}