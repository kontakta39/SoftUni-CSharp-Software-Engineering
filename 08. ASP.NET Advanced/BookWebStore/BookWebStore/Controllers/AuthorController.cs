using System.Globalization;
using System.Text.Json;
using BookWebStore.Data.Models;
using BookWebStore.Services;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore;

public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IBookService _bookService;
    private readonly string _countriesJsonPath;

    public AuthorController(IAuthorService authorService, IBookService bookService)
    {
        _authorService = authorService;
        _bookService = bookService;
        _countriesJsonPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BookWebStore.Data", "Seed", "SeedData", "countries.json");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Index(string? searchTerm)
    {
        if (Request.Query.ContainsKey("searchTerm") && string.IsNullOrWhiteSpace(searchTerm))
        {
            TempData["ErrorMessage"] = "Please enter a search term.";
            return RedirectToAction("Index", "Author", new { area = "Admin" });
        }

        List<Author> authors = new List<Author>();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            authors = await _authorService.GetAllAuthorsAsync();
        }
        else
        {
            string loweredTerm = searchTerm.ToLower();
            authors = await _authorService.SearchAuthorsAsync(loweredTerm);
            ViewData["SearchTerm"] = searchTerm;

            if (!authors.Any())
            {
                ViewData["NoResultsMessage"] = $"No authors found matching \"{searchTerm}\".";
            }
        }

        List<AuthorIndexViewModel>? getAuthors = authors
            .Select(a => new AuthorIndexViewModel
            {
                Id = a.Id,
                ImageUrl = a.ImageUrl,
                Name = a.Name,
                Nationality = a.Nationality
            })
            .ToList();

        return View(getAuthors);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add()
    {
        string jsonContent = await System.IO.File.ReadAllTextAsync(_countriesJsonPath);
        List<string> countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

        if (!countries.Any())
        {
            TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
            return RedirectToAction("Index", "Author");
        }

        AuthorAddViewModel addAuthor = new AuthorAddViewModel()
        {
            NationalityOptions = countries
        };

        return View(addAuthor);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(AuthorAddViewModel addAuthor)
    {
        string jsonContent = await System.IO.File.ReadAllTextAsync(_countriesJsonPath);
        List<string> countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

        if (!countries.Any())
        {
            TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
            return RedirectToAction("Index", "Author");
        }

        if (!ModelState.IsValid)
        {
            addAuthor.NationalityOptions = countries;
            return View(addAuthor);
        }

        if (addAuthor.BirthDate != null)
        {
            addAuthor.ParsedBirthDate = DateOnly.ParseExact(addAuthor.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        bool authorExists = await _authorService.AuthorNameExistsAsync(addAuthor.Name);

        if (authorExists)
        {
            ModelState.AddModelError(nameof(addAuthor.Name), "An author with this name already exists.");
            addAuthor.NationalityOptions = countries;
            return View(addAuthor);
        }

        await _authorService.AddAuthorAsync(addAuthor);
        return RedirectToAction("Index", "Author");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(Guid id)
    {
        Author? getAuthor = await _authorService.GetAuthorByIdAsync(id);

        if (getAuthor == null)
        {
            TempData["ErrorMessage"] = "The author you are looking for does not exist.";
            return RedirectToAction("Index", "Author");
        }

        AuthorDetailsViewModel authorDetails = new AuthorDetailsViewModel()
        {
            Id = getAuthor.Id,
            Name = getAuthor.Name,
            Biography = getAuthor.Biography,
            Nationality = getAuthor.Nationality,
            BirthDate = getAuthor.BirthDate,
            Website = getAuthor.Website,
            ImageUrl = getAuthor.ImageUrl
        };

        return View(authorDetails);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        Author? getAuthor = await _authorService.GetAuthorByIdAsync(id);

        if (getAuthor == null)
        {
            TempData["ErrorMessage"] = "The author information you want to edit does not exist.";
            return RedirectToAction("Index", "Author");
        }

        string jsonContent = await System.IO.File.ReadAllTextAsync(_countriesJsonPath);
        List<string> countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

        if (!countries.Any())
        {
            TempData["ErrorMessage"] = "The list of countries is empty.";
            return RedirectToAction("Index", "Author");
        }

        string formattedDate = getAuthor.BirthDate?.ToString("yyyy-MM-dd") ?? "";

        AuthorEditViewModel editAuthor = new AuthorEditViewModel()
        {
            Id = getAuthor.Id,
            Name = getAuthor.Name,
            Biography = getAuthor.Biography,
            Nationality = getAuthor.Nationality,
            ImageUrl = getAuthor.ImageUrl,
            Website = getAuthor.Website,
            BirthDate = formattedDate,
            NationalityOptions = countries
        };

        return View(editAuthor);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(AuthorEditViewModel editAuthor)
    {
        Author? getAuthor = await _authorService.GetAuthorByIdAsync(editAuthor.Id);

        if (getAuthor == null)
        {
            TempData["ErrorMessage"] = "The author information you want to edit does not exist.";
            return RedirectToAction("Index", "Author");
        }

        string jsonContent = await System.IO.File.ReadAllTextAsync(_countriesJsonPath);
        List<string> countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

        if (!countries.Any())
        {
            TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
            return RedirectToAction("Index", "Author");
        }

        if (!ModelState.IsValid)
        {
            editAuthor.NationalityOptions = countries;
            return View(editAuthor);
        }

        if (editAuthor.BirthDate != null)
        {
            editAuthor.ParsedBirthDate = DateOnly.ParseExact(editAuthor.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        bool authorExists = await _authorService.AuthorNameExistsAsync(editAuthor.Name, editAuthor.Id);

        if (authorExists)
        {
            ModelState.AddModelError(nameof(editAuthor.Name), "An author with this name already exists.");
            editAuthor.NationalityOptions = countries;
            return View(editAuthor);
        }

        await _authorService.EditAuthorAsync(editAuthor, getAuthor);
        return RedirectToAction("Index", "Author");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Author? getAuthor = await _authorService.GetAuthorByIdAsync(id);

        if (getAuthor == null)
        {
            TempData["ErrorMessage"] = "The author you want to delete could not be found.";
            return RedirectToAction("Index", "Author");
        }

        AuthorDeleteViewModel deleteAuthor = new AuthorDeleteViewModel()
        {
            Id = getAuthor.Id,
            Name = getAuthor.Name
        };

        return View(deleteAuthor);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(AuthorDeleteViewModel deleteAuthor)
    {
        Author? getAuthor = await _authorService.GetAuthorByIdAsync(deleteAuthor.Id);

        if (getAuthor == null)
        {
            TempData["ErrorMessage"] = "The author you want to delete could not be found.";
            return RedirectToAction("Index", "Author");
        }

        bool hasBooksInStock = await _bookService.HasBooksInStockByAuthorIdAsync(deleteAuthor.Id);

        if (hasBooksInStock)
        {
            TempData["ErrorMessage"] = $"Аuthor {deleteAuthor.Name} cannot be deleted because there are still books in stock.";
            return RedirectToAction("Index", "Author");
        }

        await _authorService.DeleteAuthorAsync(getAuthor);

        return RedirectToAction("Index", "Author");
    }
}