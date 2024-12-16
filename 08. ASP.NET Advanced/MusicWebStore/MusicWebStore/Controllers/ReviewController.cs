using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

[Authorize]
public class ReviewController : Controller
{
    private readonly IReviewInterface _reviewService;
    private readonly MusicStoreDbContext _context;

    public ReviewController(IReviewInterface reviewService, MusicStoreDbContext context)
    {
        _reviewService = reviewService;
        _context = context;    
    }

    [HttpGet]
    public async Task<IActionResult> Add(Guid id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (userId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        try
        {
            ReviewAddViewModel addReview = await _reviewService.Add(id, userId);
            return View(addReview);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(ReviewAddViewModel addReview, Guid id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("LogIn", "Account");
        }

        Album? album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            addReview.AlbumTitle = album.Title;
            addReview.AlbumId = album.Id;
            return View(addReview);
        }

        try
        {
            await _reviewService.Add(addReview, id, userId);
            return RedirectToAction("Details", "Album", new { id });
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, Guid albumId)
    {
        try
        {
            ReviewEditViewModel editReview = await _reviewService.Edit(id, albumId);
            return View(editReview);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ReviewEditViewModel editReview, Guid id, Guid albumId)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (userId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        Album? album = await _context.Albums
        .Where(a => a.Id == albumId)
        .FirstOrDefaultAsync();

        if (album == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            editReview.AlbumTitle = album.Title;
            return View(editReview);
        }

        try
        {
            await _reviewService.Edit(editReview, id, albumId, userId);
            return RedirectToAction("Details", "Album", new { id = album.Id });
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }
}