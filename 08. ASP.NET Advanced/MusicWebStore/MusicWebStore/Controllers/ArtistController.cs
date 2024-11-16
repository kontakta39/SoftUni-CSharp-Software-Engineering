using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Constants;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;
using System.Text.RegularExpressions;

namespace MusicWebStore.Controllers;

public class ArtistController : Controller
{
    private readonly MusicStoreDbContext _context;

    public ArtistController(MusicStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<ArtistIndexViewModel> artists = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .Select(a => new ArtistIndexViewModel()
            {
                Id = a.Id,
                ImageUrl = a.ImageUrl,
                Name = a.Name,
                Genre = a.Genre.Name
            })
            .AsNoTracking()
            .ToListAsync();

        return View(artists);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        List<Genre> allGenres = await _context.Genres.ToListAsync();
        ArtistAddViewModel addArtist = new ArtistAddViewModel();
        addArtist.NationalityOptions = CountriesConstants.CountriesList();
        addArtist.Genres = allGenres;

        return View(addArtist);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ArtistAddViewModel addArtist)
    {
        List<Genre> allGenres = await _context.Genres.ToListAsync();
        addArtist.Genres = allGenres;
        addArtist.NationalityOptions = CountriesConstants.CountriesList();

        List<Artist> allArtists = await _context.Artists.ToListAsync();

        /*foreach (var artistCheck in allArtists)
        {
            string formattedName = Regex.Replace(artistCheck.Name, @"\W+", "").ToLower();
            string formatedNameToBeChecked = Regex.Replace(addArtist.Name, @"\W+", "").ToLower();

            if (formattedName == formatedNameToBeChecked) 
            {
                return RedirectToAction(nameof(Edit));
            }
        }*/

        if (!ModelState.IsValid)
        {
            return View(addArtist);
        }

        Artist artist = new Artist()
        {
            Id = addArtist.Id,
            Name = addArtist.Name,
            Biography = addArtist.Biography,
            Nationality = addArtist.Nationality,
            BirthDate = string.IsNullOrEmpty(addArtist.BirthDate)
                        ? null
                        : DateOnly.ParseExact(addArtist.BirthDate, "yyyy-MM-dd", null),
            Website = addArtist.Website,
            ImageUrl = addArtist.ImageUrl,
            GenreId = addArtist.GenreId
        };

        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Artist? artistCheck = _context.Artists
            .Where(a => a.IsDeleted == false)
            .FirstOrDefault(a => a.Id == id);

        if (artistCheck == null)
        {
            return RedirectToAction(nameof(Index));
        }

        ArtistDetailsViewModel? artist = await _context.Artists
            .Select(a => new ArtistDetailsViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                Biography = a.Biography,
                Nationality = a.Nationality,
                BirthDate = a.BirthDate,
                Website = a.Website,
                ImageUrl = a.ImageUrl,
                Genre = a.Genre.Name
            })
            .FirstOrDefaultAsync();

        return View(artist);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        Artist? artist = _context.Artists.FirstOrDefault(g => g.Id == id);

        if (artist == null)
        {
            return RedirectToAction(nameof(Index));
        }

        List<Genre> allGenres = await _context.Genres.ToListAsync();

        ArtistEditViewModel? editArtist = new ArtistEditViewModel
        {
            Name = artist.Name,
            Biography = artist.Biography,
            Nationality = artist.Nationality,
            BirthDate = artist.BirthDate.ToString(),
            Website = artist.Website,
            ImageUrl = artist.ImageUrl,
            GenreId = artist.GenreId,
            Genres = allGenres,
            NationalityOptions = CountriesConstants.CountriesList()
        };

        return View(editArtist);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ArtistEditViewModel editArtist, Guid id)
    {
        List<Genre> genres = await _context.Genres.ToListAsync();
        editArtist.Genres = genres;
        editArtist.NationalityOptions = CountriesConstants.CountriesList();

        if (!ModelState.IsValid)
        {
            return View(editArtist);
        }

        Artist artist = _context.Artists.FirstOrDefault(g => g.Id == id)!;

        artist.Name = editArtist.Name;
        artist.Biography = editArtist.Biography;
        artist.Nationality = editArtist.Nationality;
        artist.BirthDate = string.IsNullOrEmpty(editArtist.BirthDate)
                    ? null
                    : DateOnly.ParseExact(editArtist.BirthDate, "yyyy-MM-dd", null);
        artist.Website = editArtist.Website;
        artist.ImageUrl = editArtist.ImageUrl;
        artist.GenreId = editArtist.GenreId;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Artist", new { id = artist.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        ArtistDeleteViewModel? deleteArtist = await _context.Artists
            .Where(p => p.Id == id && p.IsDeleted == false)
            .Select(p => new ArtistDeleteViewModel()
            {
                Id = id,
                Name = p.Name
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return View(deleteArtist);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ArtistDeleteViewModel deleteArtist)
    {
        Artist? artist = await _context.Artists
           .Where(p => p.Id == deleteArtist.Id && p.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (artist != null)
        {
            artist.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}