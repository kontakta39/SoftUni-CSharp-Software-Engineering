using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

[Authorize]
public class GenreController : Controller
{
    private readonly MusicStoreDbContext _context;

    public GenreController(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

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

        Genre genre = new Genre()
        {
            Name = addGenre.Name
        };

        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Genre? genre = _context.Genres
            .FirstOrDefault(g => g.Id == id && g.IsDeleted == false);

        if (genre == null)
        {
            return NotFound();
        }

        GenreEditViewModel editModel = new GenreEditViewModel()
        {
            Name = genre.Name
        };

        return View(editModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(GenreEditViewModel editModel, Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(editModel);
        }

        Genre genre = _context.Genres.FirstOrDefault(g => g.Id == id && g.IsDeleted == false)!;

        if (genre == null)
        {
            return NotFound();
        }

        genre.Name = editModel.Name;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        GenreDeleteViewModel? genre = await _context.Genres
            .Where(g => g.Id == id && g.IsDeleted == false)
            .Select(g => new GenreDeleteViewModel()
            {
                Id = id,
                Name = g.Name
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (genre == null)
        {
            return NotFound();
        }

        return View(genre);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(GenreDeleteViewModel model)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Genre? genre = await _context.Genres
           .Where(g => g.Id == model.Id && g.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (genre != null)
        {
            genre.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}