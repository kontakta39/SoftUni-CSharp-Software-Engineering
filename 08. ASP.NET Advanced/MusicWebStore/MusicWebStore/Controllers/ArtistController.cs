using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Constants;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;

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
            GenreId = addArtist.GenreId
        };

        // Handle image upload
        if (addArtist.ImageFile != null)
        {
            // Validate the uploaded image
            string[] allowedContentTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowedContentTypes.Contains(addArtist.ImageFile.ContentType) ||
                !allowedExtensions.Contains(Path.GetExtension(addArtist.ImageFile.FileName).ToLower()))
            {
                ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                return View(addArtist);
            }

            // Get the original file name
            string fileName = Path.GetFileName(addArtist.ImageFile.FileName); // Extract only the file name
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", fileName);

            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await addArtist.ImageFile.CopyToAsync(stream);
            }

            artist.ImageUrl = fileName; // Save the original file name for future retrieval
        }

        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Artist? artistCheck = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artistCheck == null)
        {
            return RedirectToAction(nameof(Index));
        }

        Genre? currentGenre = await _context.Genres
            .Where(g => g.Id == artistCheck.GenreId)
            .FirstOrDefaultAsync();

        ArtistDetailsViewModel? artist = new ArtistDetailsViewModel()
        {
            Id = artistCheck.Id,
            Name = artistCheck.Name,
            Biography = artistCheck.Biography,
            Nationality = artistCheck.Nationality,
            BirthDate = artistCheck.BirthDate,
            Website = artistCheck.Website,
            ImageUrl = artistCheck.ImageUrl,
            Genre = currentGenre.Name
        };

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
            Id = artist.Id,
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
        artist.GenreId = editArtist.GenreId;

        // Handle image upload
        if (editArtist.ImageFile != null)
        {
            // Validate the uploaded image
            string[] allowedContentTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowedContentTypes.Contains(editArtist.ImageFile.ContentType) ||
                !allowedExtensions.Contains(Path.GetExtension(editArtist.ImageFile.FileName).ToLower()))
            {
                ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                return View(editArtist);
            }

            // Delete the old image if it's not the default one
            if (artist.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", artist.ImageUrl);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Get the original file name
            string fileName = Path.GetFileName(editArtist.ImageFile.FileName); // Extract only the file name
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", fileName);

            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await editArtist.ImageFile.CopyToAsync(stream);
            }

            artist.ImageUrl = fileName; // Save the original file name for future retrieval
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Artist", new { id = artist.Id });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        Artist? artist = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artist == null)
        {
            return RedirectToAction(nameof(Index)); // Optionally redirect to another page
        }

        if (!string.IsNullOrEmpty(artist.ImageUrl))
        {
            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", artist.ImageUrl);

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            artist.ImageUrl = null;
            await _context.SaveChangesAsync();
        }

        return Ok(); // This returns a success response to the client
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