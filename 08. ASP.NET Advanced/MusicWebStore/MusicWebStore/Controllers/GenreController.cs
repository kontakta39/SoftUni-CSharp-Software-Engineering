using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

public class GenreController : Controller
{
    private readonly MusicStoreDbContext _context;

    public GenreController(MusicStoreDbContext context)
    {
        _context = context;
    }

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
        GenreAddViewModel addGenre = new GenreAddViewModel();

        return View(addGenre);
    }

    [HttpPost]
    public async Task<IActionResult> Add(GenreAddViewModel addGenre)
    {
        if (!ModelState.IsValid)
        {
            return View(addGenre);
        }

        Genre genre = new Genre()
        {
            Id = addGenre.Id,
            Name = addGenre.Name
        };

        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        Genre? genre = _context.Genres.FirstOrDefault(g => g.Id == id);

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
        if (!ModelState.IsValid)
        {
            return View(editModel);
        }

        Genre genre = _context.Genres.FirstOrDefault(g => g.Id == id)!;

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
        GenreDeleteViewModel? genre = await _context.Genres
            .Where(p => p.Id == id && p.IsDeleted == false)
            .Select(p => new GenreDeleteViewModel()
            {
                Id = id,
                Name = p.Name
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
        Genre? genre = await _context.Genres
           .Where(p => p.Id == model.Id && p.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (genre != null)
        {
            genre.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}