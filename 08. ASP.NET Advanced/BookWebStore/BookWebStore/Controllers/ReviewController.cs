using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IAccountService _accountService;
    private readonly IBookService _bookService;

    public ReviewController(IReviewService reviewService, IAccountService accountService, IBookService bookService)
    {
        _reviewService = reviewService;
        _accountService = accountService;
        _bookService = bookService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Add(Guid bookId)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Book? getBook = await _bookService.GetBookByIdAsync(bookId);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book does not exist.";
            return RedirectToAction("Index", "Book");
        }

        Review? review = await _reviewService.ReviewExistsAsync(getBook.Id, user.Id);

        if (review != null)
        {
            TempData["ErrorMessage"] = "You have already written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = getBook.Id });
        }

        ReviewAddViewModel reviewAddViewModel = new ReviewAddViewModel
        {
            BookId = getBook.Id,
            BookTitle = getBook.Title
        };

        return View(reviewAddViewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(ReviewAddViewModel addReview)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Book? getBook = await _bookService.GetBookByIdAsync(addReview.BookId);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book does not exist.";
            return RedirectToAction("Index", "Book");
        }

        if (!ModelState.IsValid)
        {
            return View(addReview);
        }

        Review? review = await _reviewService.ReviewExistsAsync(getBook.Id, user.Id);

        if (review != null)
        {
            TempData["ErrorMessage"] = "You have already written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = getBook.Id });
        }

        await _reviewService.AddReviewAsync(addReview, user.Id);
            
        return RedirectToAction("Details", "Book", new { id = addReview.BookId });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(Guid bookId)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Book? getBook = await _bookService.GetBookByIdAsync(bookId);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book does not exist.";
            return RedirectToAction("Index", "Book");
        }

        Review? review = await _reviewService.ReviewExistsAsync(getBook.Id, user.Id);

        if (review == null)
        {
            TempData["ErrorMessage"] = "You haven't written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = getBook.Id });
        }

        if (review.IsEdited)
        {
            TempData["ErrorMessage"] = "You have already written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = getBook.Id });
        }

        ReviewEditViewModel editReview = new ReviewEditViewModel()
        {
            Id = review.Id,
            BookId = review.BookId,
            BookTitle = review.Book.Title,
            Rating = review.Rating,
            ReviewText = review.ReviewText
        };

        return View(editReview);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(ReviewEditViewModel editReview)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Book? getBook = await _bookService.GetBookByIdAsync(editReview.BookId);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book does not exist.";
            return RedirectToAction("Index", "Book");
        }

        if (!ModelState.IsValid)
        {
            return View(editReview);
        }

        Review? review = await _reviewService.ReviewExistsAsync(getBook.Id, user.Id);

        if (review == null)
        {
            TempData["ErrorMessage"] = "You haven't written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = getBook.Id });
        }

        if (review.IsEdited)
        {
            TempData["ErrorMessage"] = "You have already written a review for this book.";
            return RedirectToAction("Details", "Book", new { id = getBook.Id });
        }

        await _reviewService.EditReviewAsync(editReview, review);

        return RedirectToAction("Details", "Book", new { id = editReview.BookId });
    }
}