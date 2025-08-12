using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IGenreService _genreService;
    private readonly IAuthorService _authorService;
    private readonly IReviewService _reviewService;

    public BookController(IBookService bookService, IGenreService genreService, IAuthorService authorService, IReviewService reviewService)
    {
        _bookService = bookService;
        _genreService = genreService;
        _authorService = authorService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm)
    {
        if (Request.Query.ContainsKey("searchTerm") && string.IsNullOrWhiteSpace(searchTerm))
        {
            TempData["ErrorMessage"] = "Please enter a search term.";
            return RedirectToAction("Index", "Book");
        }

        List<Book> books = new List<Book>();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            books = await _bookService.GetAllBooksAsync();
        }
        else
        {
            string loweredTerm = searchTerm.ToLower();
            books = await _bookService.SearchByTitleAsync(loweredTerm);
            ViewData["SearchTerm"] = searchTerm;

            if (!books.Any())
            {
                ViewData["NoResultsMessage"] = $"No books found matching \"{searchTerm}\".";
            }
        }

        List<BookIndexViewModel> getBooks = books
            .Select(b => new BookIndexViewModel()
            {
                Id = b.Id,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
                Author = b.Author.Name,
                Genre = b.Genre.Name
            })
            .ToList();

        return View(getBooks);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add()
    {
        List<Genre> genres = await _genreService.GetAllGenresAsync();
        List<Author> authors = await _authorService.GetAllAuthorsAsync();

        if (!genres.Any())
        {
            TempData["ErrorMessage"] = "The list of genres is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!authors.Any())
        {
            TempData["ErrorMessage"] = "The list of authors is empty.";
            return RedirectToAction("Index", "Book");
        }

        BookAddViewModel addBook = new BookAddViewModel
        {
            Genres = genres,
            Authors = authors
        };

        return View(addBook);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(BookAddViewModel addBook)
    {
        List<Genre> genres = await _genreService.GetAllGenresAsync();
        List<Author> authors = await _authorService.GetAllAuthorsAsync();

        if (!genres.Any())
        {
            TempData["ErrorMessage"] = "The list of genres is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!authors.Any())
        {
            TempData["ErrorMessage"] = "The list of authors is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!ModelState.IsValid)
        {
            addBook.Genres = genres;
            addBook.Authors = authors;

            return View(addBook);
        }

        bool bookExists = await _bookService.BookNameExistsAsync(addBook.Title);

        if (bookExists)
        {
            ModelState.AddModelError(nameof(addBook.Title), "A book with this name already exists.");
            addBook.Genres = genres;
            addBook.Authors = authors;
            return View(addBook);
        }

        await _bookService.AddBookAsync(addBook);
        return RedirectToAction("Index", "Book");
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Book? getBook = await _bookService.GetBookByIdAsync(id);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book you are looking for does not exist.";
            return RedirectToAction("Index", "Book");
        }

        List<Review> bookReviews = await _reviewService.GetBookReviewsAsync(getBook.Id);

        BookDetailsViewModel detailsBook = new BookDetailsViewModel()
        {
            Id = getBook.Id,
            Title = getBook.Title,
            Publisher = getBook.Publisher,
            ReleaseYear = getBook.ReleaseYear,
            PagesNumber = getBook.PagesNumber,
            ImageUrl = getBook.ImageUrl,
            Price = getBook.Price,
            Stock = getBook.Stock,
            Author = getBook.Author.Name,
            Genre = getBook.Genre.Name,
            AuthorId = getBook.Author.Id,
            IsDeleted = getBook.IsDeleted
        };

        detailsBook.Reviews = bookReviews
            .Select(r => new ReviewIndexViewModel
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                FirstName = r.User.FirstName,
                LastName = r.User.LastName,
                ReviewDate = r.ReviewDate,
                ReviewText = r.ReviewText,
                Rating = r.Rating,
                IsEdited = r.IsEdited
            })
            .ToList();

        return View(detailsBook);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        Book? getBook = await _bookService.GetBookByIdAsync(id);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book information you want to edit does not exist.";
            return RedirectToAction("Index", "Book");
        }

        List<Genre> genres = await _genreService.GetAllGenresAsync();
        List<Author> authors = await _authorService.GetAllAuthorsAsync();

        if (!genres.Any())
        {
            TempData["ErrorMessage"] = "The list of genres is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!authors.Any())
        {
            TempData["ErrorMessage"] = "The list of authors is empty.";
            return RedirectToAction("Index", "Book");
        }

        BookEditViewModel editBook = new BookEditViewModel()
        {
            Id = getBook.Id,
            Title = getBook.Title,
            Publisher = getBook.Publisher,
            ReleaseYear = getBook.ReleaseYear,
            PagesNumber = getBook.PagesNumber,
            ImageUrl = getBook.ImageUrl,
            Price = getBook.Price,
            Stock = getBook.Stock,
            AuthorId = getBook.AuthorId,
            GenreId = getBook.GenreId,
            Genres = genres,
            Authors = authors
        };

        return View(editBook);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(BookEditViewModel editBook)
    {
        Book? getBook = await _bookService.GetBookByIdAsync(editBook.Id);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book information you want to edit does not exist.";
            return RedirectToAction("Index", "Book");
        }

        List<Genre> genres = await _genreService.GetAllGenresAsync();
        List<Author> authors = await _authorService.GetAllAuthorsAsync();

        if (!genres.Any())
        {
            TempData["ErrorMessage"] = "The list of genres is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!authors.Any())
        {
            TempData["ErrorMessage"] = "The list of authors is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!ModelState.IsValid)
        {
            editBook.Genres = genres;
            editBook.Authors = authors;
            return View(editBook);
        }

        bool bookExists = await _bookService.BookNameExistsAsync(editBook.Title, editBook.Id);

        if (bookExists)
        {
            ModelState.AddModelError(nameof(editBook.Title), "A book with this name already exists.");
            editBook.Genres = genres;
            editBook.Authors = authors;
            return View(editBook);
        }

        await _bookService.EditBookAsync(editBook, getBook);
        return RedirectToAction("Index", "Book");
    }
}