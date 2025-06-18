using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
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
    public async Task<IActionResult> Index()
    {
        List<GenreIndexViewModel> genres = await _context.Genres
           .Where(g => g.IsDeleted == false)
           .Select(g => new GenreIndexViewModel()
           {
               Id = g.Id,
               Name = g.Name
           })
           .OrderBy(g => g.Name)
           .AsNoTracking()
           .ToListAsync();

        return View(genres);
    }

    [HttpGet]
    public IActionResult Add()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        GenreAddViewModel addGenre = new GenreAddViewModel();
        return View(addGenre);
    }

    [HttpPost]
    public async Task<IActionResult> Add(GenreAddViewModel addGenre)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(addGenre);
        }

        // Check if genre already exists (case-insensitive)
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
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Genre? genre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);

        if (genre == null)
        {
            return View("NotFound");
        }

        GenreEditViewModel editModel = new GenreEditViewModel()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return View(editModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(GenreEditViewModel editGenre)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(editGenre);
        }

        Genre? genre = _context.Genres
            .FirstOrDefault(g => g.Id == editGenre.Id && !g.IsDeleted);

        if (genre == null)
        {
            return View("NotFound");
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
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Genre? genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);

        if (genre == null)
        {
            return View("NotFound");
        }

        GenreDeleteViewModel deleteGenre = new GenreDeleteViewModel()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return View(deleteGenre);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(GenreDeleteViewModel deleteGenre)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Genre? genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == deleteGenre.Id && g.Name == deleteGenre.Name && !g.IsDeleted);

        if (genre == null)
        {
            return View("NotFound");
        }

        genre.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Genre");
    }
}