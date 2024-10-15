using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Data;
using CinemaWebApp.Data.Models;
using CinemaWebApp.ViewModels.Movie;

namespace CinemaWebApp.Controllers;

public class MovieController : BaseController
{
    private readonly CinemaDbContext _dbContext;

    public MovieController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<Movie> allMovies = await _dbContext.Movies.ToArrayAsync();

        return View(allMovies);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddMovieInputModel inputModel)
    {
        if (!ModelState.IsValid) 
        {
            //Render the same form with user entered values + model errors
            return View(inputModel);    
        }

        string inputDate = inputModel.ReleaseDate.ToString();
        DateTime date;

        bool isValidDate = DateTime.TryParseExact(inputDate, "dd.MM.yyyy",
                  CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

        if (!isValidDate)
        {
            // Add a model error if the date format is incorrect
            ModelState.AddModelError("ReleaseDate", "Invalid date format. Please enter the date as dd.MM.yyyy.");
            return View(inputModel);
        }


        Movie movie = new Movie() 
        {
            Title = inputModel.Title,
            Genre = inputModel.Genre,
            ReleaseDate = date,
            Director = inputModel.Director,
            Duration = inputModel.Duration,
            Description = inputModel.Description
        };

        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Details(string Id)
    {
        Guid movieGuid = Guid.Empty;
        bool isIdValid = isGuidValid(Id, ref movieGuid);

        //Invalid Id format
        if (isIdValid = false)
        {
            return RedirectToAction(nameof(Index));
        }

        Movie? movie = await _dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == movieGuid);

        //The movie does not exist
        if (movie == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    [HttpGet]
    public async Task<IActionResult> AddToProgram(string? id) 
    {
        Guid movieGuid = Guid.Empty;
        bool isMovieIdValid = isGuidValid(id, ref movieGuid);

        if (!isMovieIdValid)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}