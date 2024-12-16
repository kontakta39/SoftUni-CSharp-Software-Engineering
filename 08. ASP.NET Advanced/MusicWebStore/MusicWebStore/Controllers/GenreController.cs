using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

[Authorize]
public class GenreController : Controller
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        List<GenreIndexViewModel> genres = await _genreService.Index();
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

        try
        {
            await _genreService.Add(addGenre);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            GenreEditViewModel editModel = await _genreService.Edit(id);
            return View(editModel);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
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

        try
        {
            await _genreService.Edit(editModel, id);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            GenreDeleteViewModel genre = await _genreService.Delete(id);
            return View(genre);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(GenreDeleteViewModel model)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            await _genreService.Delete(model);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }
}