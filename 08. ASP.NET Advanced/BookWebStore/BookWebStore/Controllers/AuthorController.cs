using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace BookWebStore;

public class AuthorController : Controller
{
    private readonly BookStoreDbContext _context;

    public AuthorController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Index()
    {
        List<AuthorIndexViewModel> artists = await _context.Authors
        .Where(a => a.IsDeleted == false)
        .Select(a => new AuthorIndexViewModel()
        {
            Id = a.Id,
            ImageUrl = a.ImageUrl,
            Name = a.Name,
            Nationality = a.Nationality
        })
        .ToListAsync();

        return View(artists);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add()
    {
        string countriesJsonPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BookWebStore.Data", "Seed", "SeedData", "countries.json");
        string jsonContent = await System.IO.File.ReadAllTextAsync(countriesJsonPath);
        List<string>? countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

        if (!countries.Any())
        {
            TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
            return RedirectToAction("Index", "Author");
        }

        AuthorAddViewModel addAuthor = new AuthorAddViewModel();
        addAuthor.NationalityOptions = countries;

        return View(addAuthor);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(AuthorAddViewModel addAuthor)
    {
        if (!ModelState.IsValid)
        {
            string countriesJsonPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BookWebStore.Data", "Seed", "SeedData", "countries.json");
            string jsonContent = await System.IO.File.ReadAllTextAsync(countriesJsonPath);
            List<string>? countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

            if (!countries.Any())
            {
                TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
                return RedirectToAction("Index", "Author");
            }

            addAuthor.NationalityOptions = countries;
            return View(addAuthor);
        }

        bool authorExists = await _context.Authors
            .AnyAsync(a => a.Name.ToLower() == addAuthor.Name && !a.IsDeleted);

        if (authorExists)
        {
            ModelState.AddModelError("Name", "An author with this name already exists.");
            return View(addAuthor);
        }

        DateOnly? parsedBirthDate = null;

        if (!string.IsNullOrEmpty(addAuthor.BirthDate))
        {
            bool success = DateOnly.TryParseExact(
                addAuthor.BirthDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly tempDate);

            if (success)
            {
                parsedBirthDate = tempDate;
            }
            else
            {
                parsedBirthDate = null;
            }
        }

        Author author = new Author
        {
            Name = addAuthor.Name,
            Biography = addAuthor.Biography,
            Nationality = addAuthor.Nationality,
            ImageUrl = addAuthor.ImageUrl,
            BirthDate = parsedBirthDate,
            Website = addAuthor.Website
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Author");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(Guid id)
    {
        Author? authorCheck = await _context.Authors
           .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (authorCheck == null)
        {
            return NotFound();
        }

        AuthorDetailsViewModel detailsAuthor = new AuthorDetailsViewModel()
        {
            Id = authorCheck.Id,
            Name = authorCheck.Name,
            Biography = authorCheck.Biography,
            Nationality = authorCheck.Nationality,
            BirthDate = authorCheck.BirthDate,
            Website = authorCheck.Website,
            ImageUrl = authorCheck.ImageUrl
        };

        return View(detailsAuthor);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        Author? author = await _context.Authors
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (author == null)
        {
            return NotFound();
        }

        string countriesJsonPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BookWebStore.Data", "Seed", "SeedData", "countries.json");
        string jsonContent = await System.IO.File.ReadAllTextAsync(countriesJsonPath);
        List<string>? countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

        if (!countries.Any())
        {
            TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
            return RedirectToAction("Index", "Author");
        }

        AuthorEditViewModel editAuthor = new AuthorEditViewModel()
        {
            Id = id,
            Name = author.Name,
            Biography = author.Biography,
            Nationality = author.Nationality,
            ImageUrl = author.ImageUrl,
            Website = author.Website,
            BirthDate = author.BirthDate?.ToString("yyyy-MM-dd") ?? string.Empty,
            NationalityOptions = countries
        };

        return View(editAuthor);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(AuthorEditViewModel editAuthor)
    {
        if (!ModelState.IsValid)
        {
            string countriesJsonPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BookWebStore.Data", "Seed", "SeedData", "countries.json");
            string jsonContent = await System.IO.File.ReadAllTextAsync(countriesJsonPath);
            List<string>? countries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();

            if (!countries.Any())
            {
                TempData["ErrorMessage"] = "The list of countries is empty. Please check the data source.";
                return RedirectToAction("Index", "Author");
            }

            editAuthor.NationalityOptions = countries;
            return View(editAuthor);
        }

        bool authorExists = await _context.Authors
            .AnyAsync(a => a.Id != editAuthor.Id && a.Name.ToLower() == editAuthor.Name.ToLower() && !a.IsDeleted);

        if (authorExists)
        {
            ModelState.AddModelError("Name", "An author with this name already exists.");
            return View(editAuthor);
        }

        Author? author = _context.Authors
            .FirstOrDefault(a => a.Id == editAuthor.Id && a.Name.ToLower() == editAuthor.Name.ToLower() && !a.IsDeleted);

        if (author == null)
        {
            return NotFound();
        }

        DateOnly? parsedBirthDate = null;

        if (!string.IsNullOrEmpty(editAuthor.BirthDate))
        {
            bool success = DateOnly.TryParseExact(
                editAuthor.BirthDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly tempDate);

            if (success)
            {
                parsedBirthDate = tempDate;
            }
            else
            {
                parsedBirthDate = null;
            }
        }

        author.Name = editAuthor.Name;
        author.Biography = editAuthor.Biography;
        author.Nationality = editAuthor.Nationality;
        author.ImageUrl = editAuthor.ImageUrl;
        author.BirthDate = parsedBirthDate;
        author.Website = editAuthor.Website;

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Author");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Author? author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (author == null)
        {
            return NotFound();
        }

        AuthorDeleteViewModel deleteAuthor = new AuthorDeleteViewModel()
        {
            Id = author.Id,
            Name = author.Name
        };

        return View(deleteAuthor);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(AuthorDeleteViewModel deleteAuthor)
    {
        Author? author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == deleteAuthor.Id && a.Name.ToLower() == deleteAuthor.Name.ToLower() && !a.IsDeleted);

        if (author == null)
        {
            return NotFound();
        }

        //Check if the author still has books in stock
        bool hasBooksInStock = await _context.Books
            .AnyAsync(b => b.AuthorId == author.Id && !b.IsDeleted && b.Stock > 0);

        if (hasBooksInStock)
        {
            TempData["ErrorMessage"] = "This author cannot be deleted because there are still books in stock.";
            return RedirectToAction("Index", "Author");
        }

        author.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Author");
    }
}