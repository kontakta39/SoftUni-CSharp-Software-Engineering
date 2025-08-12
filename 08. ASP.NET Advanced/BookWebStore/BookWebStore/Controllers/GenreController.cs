using BookWebStore.Data.Models;
using BookWebStore.Services;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore;

public class GenreController : Controller
{
    private readonly IGenreService _genreService;
    private readonly IBookService _bookService;

    public GenreController(IGenreService genreService, IBookService bookService)
    {
        _genreService = genreService;
        _bookService = bookService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Index(string? searchTerm)
    {
        if (Request.Query.ContainsKey("searchTerm") && string.IsNullOrWhiteSpace(searchTerm))
        {
            TempData["ErrorMessage"] = "Please enter a search term.";
            return RedirectToAction("Index", "Genre", new { area = "Admin" });
        }

        List<Genre> genres = new List<Genre>();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            genres = await _genreService.GetAllGenresAsync();
        }
        else
        {
            string loweredTerm = searchTerm.ToLower();
            genres = await _genreService.SearchGenresAsync(loweredTerm);
            ViewData["SearchTerm"] = searchTerm;

            if (!genres.Any())
            {
                ViewData["NoResultsMessage"] = $"No genres found matching \"{searchTerm}\".";
            }
        }

        List<GenreIndexViewModel> getGenres = genres
            .OrderBy(g => g.Name)
            .Select(g => new GenreIndexViewModel
            {
                Id = g.Id,
                Name = g.Name
            })
            .ToList();

        return View(getGenres);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Add()
    {
        GenreAddViewModel addGenre = new GenreAddViewModel();
        return View(addGenre);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(GenreAddViewModel addGenre)
    {
        if (!ModelState.IsValid)
        {
            return View(addGenre);
        }

        bool genreExists = await _genreService.GenreNameExistsAsync(addGenre.Name);

        if (genreExists)
        {
            ModelState.AddModelError(nameof(addGenre.Name), "A genre with this name already exists.");
            return View(addGenre);
        }

        await _genreService.AddGenreAsync(addGenre);
        return RedirectToAction("Index", "Genre");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        Genre? getGenre = await _genreService.GetGenreByIdAsync(id);

        if (getGenre == null)
        {
            TempData["ErrorMessage"] = "The genre you want to edit could not be found.";
            return RedirectToAction("Index", "Genre");
        }

        GenreEditViewModel editGenre = new GenreEditViewModel()
        {
            Id = getGenre.Id,
            Name = getGenre.Name
        };

        return View(editGenre);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(GenreEditViewModel editGenre)
     {
        Genre? getGenre = await _genreService.GetGenreByIdAsync(editGenre.Id);

        if (getGenre == null)
        {
            TempData["ErrorMessage"] = "The genre you want to edit could not be found.";
            return RedirectToAction("Index", "Genre");
        }

        if (!ModelState.IsValid)
        {
            return View(editGenre);
        }

        bool genreExists = await _genreService.GenreNameExistsAsync(editGenre.Name, editGenre.Id);

        if (genreExists)
        {
            ModelState.AddModelError(nameof(editGenre.Name), "A genre with this name already exists.");
            return View(editGenre);
        }

        await _genreService.EditGenreAsync(editGenre, getGenre);
        return RedirectToAction("Index", "Genre");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Genre? getGenre = await _genreService.GetGenreByIdAsync(id);

        if (getGenre == null)
        {
            TempData["ErrorMessage"] = "The genre you want to delete could not be found.";
            return RedirectToAction("Index", "Genre");
        }

        GenreDeleteViewModel deleteGenre = new GenreDeleteViewModel()
        {
            Id = getGenre.Id,
            Name = getGenre.Name
        };

        return View(deleteGenre);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(GenreDeleteViewModel deleteGenre)
    {
        Genre? getGenre = await _genreService.GetGenreByIdAsync(deleteGenre.Id);

        if (getGenre == null)
        {
            TempData["ErrorMessage"] = "The genre you want to edit could not be found.";
            return RedirectToAction("Index", "Genre");
        }

        bool hasBooksInStock = await _bookService.HasBooksInStockByGenreIdAsync(deleteGenre.Id);

        if (hasBooksInStock)
        {
            TempData["ErrorMessage"] = $"{deleteGenre.Name} genre cannot be deleted because there are still books in stock.";
            return RedirectToAction("Index", "Genre");
        }

        await _genreService.DeleteGenreAsync(getGenre);

        return RedirectToAction("Index", "Genre");
    }
}