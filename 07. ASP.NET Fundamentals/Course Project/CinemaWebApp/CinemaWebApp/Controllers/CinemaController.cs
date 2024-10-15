using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Data;
using CinemaWebApp.Data.Models;
using CinemaWebApp.ViewModels.Cinema;
using CinemaWebApp.ViewModels.Movie;

namespace CinemaWebApp.Controllers;

public class CinemaController : BaseController
{
    private readonly CinemaDbContext _dbContext;

    public CinemaController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext; //Dependency Injection
    }

    [HttpGet]
    public async Task<IActionResult> Index() 
    {
         IEnumerable<CinemaIndexViewModel> cinemas = await _dbContext
            .Cinemas
            .Select(c => new CinemaIndexViewModel 
            { 
                Id = c.Id.ToString(),
                Name = c.Name,
                Location = c.Location,
            })
            .OrderBy(c => c.Location)
            .ToArrayAsync();

        return View(cinemas);
    }

    [HttpGet]
    public async Task<IActionResult> Create() 
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CinemaCreateViewModel model)
    {
        if (!ModelState.IsValid) 
        {
            return View(model);
        }

        Cinema cinema = new Cinema()
        {
            Name = model.Name,
            Location = model.Location
        };

        await _dbContext.Cinemas.AddAsync(cinema);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details (string? id)
    {
        Guid guidId = Guid.Empty;
        bool isIdValid = isGuidValid(id, ref guidId);

        if (!isIdValid) 
        {
            return RedirectToAction(nameof(Index));
        }

        Cinema? cinema = await _dbContext
        .Cinemas
        .Include(cm => cm.CinemasMovies)
        .ThenInclude(m => m.Movie)
        .FirstOrDefaultAsync(c => c.Id == guidId);

        //Invalid (non-existing) Guid in the URL
        if (cinema == null)
        {
            return RedirectToAction(nameof(Index));
        }

        CinemaDetailsViewModel viewModel = new CinemaDetailsViewModel()
        {
            Name = cinema.Name,
            Location = cinema.Location,
            Movies = cinema.CinemasMovies
                .Select(cm => new CinemaMovieViewModel() 
                {
                    Title = cm.Movie.Title,
                    Duration = cm.Movie.Duration
                })
                .ToArray()
        };

        return View(viewModel);
    }
}