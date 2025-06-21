using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Controllers;

public class BookController : Controller
{
    private readonly BookStoreDbContext _context;

    public BookController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<BookIndexViewModel> books = await _context.Books
          .Where(b => !b.IsDeleted && b.Stock > 0)
          .Select(b => new BookIndexViewModel()
          {
              Id = b.Id,
              Title = b.Title,
              ImageUrl = b.ImageUrl,
              Author = b.Author.Name,
              Genre = b.Genre.Name
          })
          .ToListAsync();

        return View(books);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add()
    {
        BookAddViewModel addBook = new BookAddViewModel();

        addBook.Genres = await _context.Genres
            .Where(g => !g.IsDeleted)
            .ToListAsync();

        addBook.Authors = await _context.Authors
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        if (!addBook.Genres.Any())
        {
            TempData["ErrorMessage"] = "The list of genres is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!addBook.Authors.Any())
        {
            TempData["ErrorMessage"] = "The list of authors is empty.";
            return RedirectToAction("Index", "Author");
        }

        return View(addBook);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(BookAddViewModel addBook)
    {
        if (!ModelState.IsValid)
        {
            addBook.Genres = await _context.Genres
               .Where(g => !g.IsDeleted)
               .ToListAsync();

            addBook.Authors = await _context.Authors
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            if (!addBook.Genres.Any())
            {
                TempData["ErrorMessage"] = "The list of genres is empty.";
                return RedirectToAction("Index", "Book");
            }

            if (!addBook.Authors.Any())
            {
                TempData["ErrorMessage"] = "The list of authors is empty.";
                return RedirectToAction("Index", "Author");
            }

            return View(addBook);
        }

        bool bookExists = await _context.Books
            .AnyAsync(a => a.Title.ToLower() == addBook.Title.ToLower() && !a.IsDeleted);

        if (bookExists)
        {
            ModelState.AddModelError("Title", "A book with this title already exists.");
            return View(addBook);
        }

        Book book = new Book()
        {
            Title = addBook.Title,
            Publisher = addBook.Publisher,
            ReleaseYear = addBook.ReleaseYear,
            PagesNumber = addBook.PagesNumber,
            ImageUrl = addBook.ImageUrl,
            Price = addBook.Price,
            Stock = addBook.Stock,
            AuthorId = addBook.AuthorId,
            GenreId = addBook.GenreId
        };

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Book");
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Book? bookCheck = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (bookCheck == null)
        {
            return NotFound();
        }

        BookDetailsViewModel detailsBook = new BookDetailsViewModel()
        {
            Id = bookCheck.Id,
            Title = bookCheck.Title,
            Publisher = bookCheck.Publisher,
            ReleaseYear = bookCheck.ReleaseYear,
            PagesNumber = bookCheck.PagesNumber,
            ImageUrl = bookCheck.ImageUrl,
            Price = bookCheck.Price,
            Stock = bookCheck.Stock,
            Author = bookCheck.Author.Name,
            Genre = bookCheck.Genre.Name,
            AuthorId = bookCheck.Author.Id
        };

        return View(detailsBook);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        Book? book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (book == null)
        {
            return NotFound();
        }

        List<Genre> allGenres = await _context.Genres
            .Where(g => !g.IsDeleted)
            .ToListAsync();

        List<Author> allAuthors = await _context.Authors
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        if (!allGenres.Any())
        {
            TempData["ErrorMessage"] = "The list of genres is empty.";
            return RedirectToAction("Index", "Book");
        }

        if (!allAuthors.Any())
        {
            TempData["ErrorMessage"] = "The list of authors is empty.";
            return RedirectToAction("Index", "Author");
        }

        BookEditViewModel editBook = new BookEditViewModel()
        {
            Id = book.Id,
            Title = book.Title,
            Publisher = book.Publisher,
            ReleaseYear = book.ReleaseYear,
            PagesNumber = book.PagesNumber,
            ImageUrl = book.ImageUrl,
            Price = book.Price,
            Stock = book.Stock,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId
        };

        editBook.Genres = allGenres;
        editBook.Authors = allAuthors;

        return View(editBook);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(BookEditViewModel editBook)
    {
        if (!ModelState.IsValid)
        {
            editBook.Genres = await _context.Genres
               .Where(g => !g.IsDeleted)
               .ToListAsync();

            editBook.Authors = await _context.Authors
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            if (!editBook.Genres.Any())
            {
                TempData["ErrorMessage"] = "The list of genres is empty.";
                return RedirectToAction("Index", "Book");
            }

            if (!editBook.Authors.Any())
            {
                TempData["ErrorMessage"] = "The list of authors is empty.";
                return RedirectToAction("Index", "Author");
            }

            return View(editBook);
        }

        bool bookExists = await _context.Books
            .AnyAsync(b => b.Id != editBook.Id && b.Title.ToLower() == editBook.Title.ToLower() && !b.IsDeleted);

        if (bookExists)
        {
            ModelState.AddModelError("Title", "A book with this title already exists.");
            return View(editBook);
        }

        Book? book = _context.Books
           .FirstOrDefault(a => a.Id == editBook.Id && a.Title.ToLower() == editBook.Title.ToLower() && !a.IsDeleted);

        if (book == null)
        {
            return NotFound();
        }

        book.Title = editBook.Title;
        book.Publisher = editBook.Publisher;
        book.ReleaseYear = editBook.ReleaseYear;
        book.PagesNumber = editBook.PagesNumber;
        book.ImageUrl = editBook.ImageUrl;
        book.Price = editBook.Price;
        book.Stock = editBook.Stock;
        book.AuthorId = editBook.AuthorId;
        book.GenreId = editBook.GenreId;

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Book");
    }
}
