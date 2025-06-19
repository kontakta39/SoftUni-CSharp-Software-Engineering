using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore;

public class GenreController : Controller
{
    private readonly BookStoreDbContext _context;

    public GenreController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Index()
    {
        List<GenreIndexViewModel> genres = await _context.Genres
           .Where(g => g.IsDeleted == false)
           .Select(g => new GenreIndexViewModel()
           {
               Id = g.Id,
               Name = g.Name
           })
           .ToListAsync();

        return View(genres);
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

        bool genreExists = await _context.Genres
            .AnyAsync(g => g.Name.ToLower() == addGenre.Name.ToLower() && !g.IsDeleted);

        if (genreExists)
        {
            ModelState.AddModelError("Name", "A genre with this name already exists.");
            return View(addGenre);
        }

        Genre genre = new Genre()
        {
            Name = addGenre.Name
        };

        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        Genre? genre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);

        if (genre == null)
        {
            return NotFound();
        }

        GenreEditViewModel editModel = new GenreEditViewModel()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return View(editModel);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(GenreEditViewModel editGenre)
    {
        if (!ModelState.IsValid)
        {
            return View(editGenre);
        }

        Genre? genre = _context.Genres
            .FirstOrDefault(g => g.Id == editGenre.Id && !g.IsDeleted);

        if (genre == null)
        {
            return NotFound();
        }

        bool genreExists = await _context.Genres
            .AnyAsync(g => g.Name.ToLower() == editGenre.Name.ToLower() && !g.IsDeleted);

        if (genreExists)
        {
            ModelState.AddModelError("Name", "A genre with this name already exists.");
            return View(editGenre);
        }

        genre.Name = editGenre.Name;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Genre");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Genre? genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);

        if (genre == null)
        {
            return NotFound();
        }

        GenreDeleteViewModel deleteGenre = new GenreDeleteViewModel()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return View(deleteGenre);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(GenreDeleteViewModel deleteGenre)
    {
        Genre? genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == deleteGenre.Id && g.Name == deleteGenre.Name && !g.IsDeleted);

        if (genre == null)
        {
            return NotFound();
        }

        genre.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Genre");
    }
}