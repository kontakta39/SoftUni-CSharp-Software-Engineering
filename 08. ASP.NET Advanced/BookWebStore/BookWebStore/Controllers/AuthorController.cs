using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        Author author = new Author
        {
            Name = addAuthor.Name,
            Biography = addAuthor.Biography,
            Nationality = addAuthor.Nationality,
            ImageUrl = addAuthor.ImageUrl,
            BirthDate = addAuthor.BirthDate,
            Website = addAuthor.Website
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Author");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
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


}
