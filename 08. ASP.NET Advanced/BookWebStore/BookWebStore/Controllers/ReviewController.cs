using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Controllers;

public class ReviewController : Controller
{
    private readonly BookStoreDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewController(BookStoreDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Add(Guid bookId)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Book? book = await _context.Books
            .Where(a => a.Id == bookId)
            .FirstOrDefaultAsync();

        if (book == null)
        {
            TempData["ErrorMessage"] = "The book does not exist.";
            return RedirectToAction("Index", "Book");
        }

        //Check if the user has already reviewed this album
        bool reviewExists = await _context.Reviews
            .AnyAsync(r => r.BookId == book.Id && r.UserId == user.Id);

        if (reviewExists)
        {
            TempData["ErrorMessage"] = "You have already written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = book.Id });
        }

        ReviewAddViewModel addReview = new ReviewAddViewModel
        {
            BookId = book.Id,
            BookTitle = book.Title
        };

        return View(addReview);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(ReviewAddViewModel addReview)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(addReview);
        }

        Book? book = await _context.Books
            .Where(a => a.Id == addReview.BookId)
            .FirstOrDefaultAsync();

        if (book == null)
        {
            TempData["ErrorMessage"] = "The book does not exist.";
            return RedirectToAction("Index", "Book");
        }

        //Check if the user has already reviewed this album
        bool reviewExists = await _context.Reviews
            .AnyAsync(r => r.BookId == book.Id && r.UserId == user.Id);

        if (reviewExists)
        {
            TempData["ErrorMessage"] = "You have already written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = book.Id });
        }

        Review review = new Review
        {
            BookId = book.Id,
            UserId = user.Id,
            Rating = addReview.Rating,
            ReviewText = addReview.ReviewText
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Book", new { id = book.Id });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(Guid reviewId, Guid bookId)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        //Find the review that should be edited
        Review? findReview = await _context.Reviews
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == user.Id && r.BookId == bookId && !r.IsEdited);

        if (findReview == null)
        {
            TempData["ErrorMessage"] = "You haven't written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = bookId });
        }

        ReviewEditViewModel editReview = new ReviewEditViewModel()
        {
            Id = findReview.Id,
            BookId = findReview.BookId,
            BookTitle = findReview.Book.Title,
            Rating = findReview.Rating,
            ReviewText = findReview.ReviewText
        };

        return View(editReview);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(ReviewEditViewModel editReview)
    { 
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(editReview);
        }

        //Find the review that should be edited
        Review? review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == editReview.Id && r.UserId == user.Id && r.BookId == editReview.BookId && !r.IsEdited);

        if (review == null)
        {
            TempData["ErrorMessage"] = "The review was not found.";
            return RedirectToAction("Details", "Book", new { id = editReview.BookId });
        }

        review.ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        review.Rating = editReview.Rating;
        review.ReviewText = editReview.ReviewText;
        review.IsEdited = true;

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Book", new { id = review.BookId });
    }
}