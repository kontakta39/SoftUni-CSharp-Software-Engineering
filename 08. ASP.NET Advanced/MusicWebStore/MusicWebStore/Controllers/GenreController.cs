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

        await _genreService.Add(addGenre);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        GenreEditViewModel editModel = await _genreService.Edit(id);

        if (editModel == null)
        {
            return RedirectToAction("NotFound", "Home");
        }

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

        editModel = await _genreService.Edit(editModel, id);

        if (editModel == null)
        {
            return RedirectToAction("NotFound", "Home");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        GenreDeleteViewModel genre = await _genreService.Delete(id);

        if (genre == null)
        {
            return RedirectToAction("NotFound", "Home");
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

        await _genreService.Delete(model);
        return RedirectToAction(nameof(Index));
    }
}