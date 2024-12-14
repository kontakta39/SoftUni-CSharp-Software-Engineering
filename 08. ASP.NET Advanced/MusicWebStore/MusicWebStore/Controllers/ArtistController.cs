using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Constants;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

public class ArtistController : Controller
{
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public ArtistController(MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _context = context;
        _imageHandler = imageHandler;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

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
    [Authorize]
    public async Task<IActionResult> Add()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        List<Genre> allGenres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        ArtistAddViewModel addArtist = new ArtistAddViewModel();
        addArtist.NationalityOptions = CountriesConstants.CountriesList();
        addArtist.Genres = allGenres;

        return View(addArtist);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(ArtistAddViewModel addArtist)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        List<Genre> allGenres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        addArtist.Genres = allGenres;
        addArtist.NationalityOptions = CountriesConstants.CountriesList();

        // Define allowed content types and extensions
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images");

        if (!ModelState.IsValid)
        {
            addArtist.Genres = allGenres;
            addArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (addArtist.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(addArtist.ImageFile, allowedContentTypes, allowedExtensions);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    string tempFileName = await ImageHandler.SaveTempImageAsync(addArtist.ImageFile, tempFolderPath);
                    addArtist.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; // Store temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addArtist.ImageUrl = TempData["ImageUrl"].ToString(); // Retrieve the file name
            }

            TempData.Keep("ImageUrl");

            return View(addArtist);
        }

        Artist artist = new Artist()
        {
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
            string fileName = await _imageHandler.SaveFinalImageAsync(addArtist.ImageFile);
            artist.ImageUrl = fileName;
        }
        else if (TempData["ImageUrl"] != null)
        {
            string tempFileName = TempData["ImageUrl"].ToString();
            string finalFileName = ImageHandler.MoveImageToFinalFolder(tempFileName, tempFolderPath, finalFolderPath);

            if (finalFileName != null)
            {
                artist.ImageUrl = finalFileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "The temporary file is missing. Please re-upload the image.");
                return View(addArtist);
            }
        }

        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();

        // Clean up TempData
        TempData.Remove("ImageUrl");

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
            return NotFound();
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
    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Artist? artist = _context.Artists
            .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

        if (artist == null)
        {
            return NotFound();
        }

        List<Genre> allGenres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

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
    [Authorize]
    public async Task<IActionResult> Edit(ArtistEditViewModel editArtist, Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        List<Genre> genres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        TempData["CurrentImageUrl"] = await _context.Artists
            .Where(a => a.Id == id && a.IsDeleted == false)
            .Select(a => a.ImageUrl)
            .FirstOrDefaultAsync();

        // Validate the uploaded image using ImageHandler
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images");

        if (!ModelState.IsValid)
        {
            editArtist.Genres = genres;
            editArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editArtist.ImageFile != null)
            {
                // Delete the old image if it's not the default one
                Artist? artistCheck = _context.Artists
                    .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

                string validationError = ImageHandler.ValidateImage(editArtist.ImageFile, allowedContentTypes, allowedExtensions);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    if (artistCheck.ImageUrl != null)
                    {
                        // Use ImageHandler to delete the old image
                        _imageHandler.DeleteImage(artistCheck.ImageUrl, finalFolderPath);
                    }

                    // Save the file temporarily using ImageHandler
                    string tempFileName = await ImageHandler.SaveTempImageAsync(editArtist.ImageFile, tempFolderPath);
                    editArtist.ImageUrl = tempFileName;
                    TempData["NewImageUrl"] = tempFileName; // Store the file name temporarily
                }
            }
            else if (TempData["NewImageUrl"] != null)
            {
                editArtist.ImageUrl = TempData["NewImageUrl"].ToString(); // Retrieve the file name from TempData
                TempData.Keep("NewImageUrl"); // Preserve TempData for subsequent requests
            }
            else
            {
                editArtist.ImageUrl = TempData["CurrentImageUrl"].ToString(); // Retrieve the file name from TempData
            }

            return View(editArtist);
        }

        Artist artist = _context.Artists
            .FirstOrDefault(a => a.Id == id && a.IsDeleted == false)!;

        artist.Name = editArtist.Name;
        artist.Biography = editArtist.Biography;
        artist.Nationality = editArtist.Nationality;
        artist.BirthDate = string.IsNullOrEmpty(editArtist.BirthDate)
                    ? null
                    : DateOnly.ParseExact(editArtist.BirthDate, "yyyy-MM-dd", null);
        artist.Website = editArtist.Website;
        artist.GenreId = editArtist.GenreId;

        //Handle image upload (copy the file to server only if ModelState is valid)
        if (editArtist.ImageFile != null)
        {
            // Delete the old image if it's not the default one
            if (artist.ImageUrl != null)
            {
                _imageHandler.DeleteImage(artist.ImageUrl, finalFolderPath);
            }

            // Save the file to the final folder using ImageHandler
            string fileName = await _imageHandler.SaveFinalImageAsync(editArtist.ImageFile);
            artist.ImageUrl = fileName; // Save the file name in the database
        }
        else if (TempData["NewImageUrl"] != null && artist.ImageUrl != null)
        {
            string fileName = TempData["NewImageUrl"].ToString();
            string currentTempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderPath, fileName);

            if (System.IO.File.Exists(currentTempFilePath))
            {
                // Use ImageHandler to move the image from temp to final folder
                ImageHandler.MoveImageToFinalFolder(fileName, tempFolderPath, finalFolderPath);
                artist.ImageUrl = fileName; // Save the file name in the database
            }
            else
            {
                ModelState.AddModelError("ImageFile", "The temporary file is missing. Please re-upload the image.");
                return View(editArtist);
            }
        }

        await _context.SaveChangesAsync();

        // Clean up TempData
        TempData.Remove("CurrentImageUrl");
        TempData.Remove("NewImageUrl");

        return RedirectToAction("Details", "Artist", new { id = artist.Id });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        ArtistDeleteViewModel? deleteArtist = await _context.Artists
            .Where(p => p.Id == id && p.IsDeleted == false)
            .Select(p => new ArtistDeleteViewModel()
            {
                Id = id,
                Name = p.Name
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (deleteArtist == null)
        {
            return NotFound();
        }

        return View(deleteArtist);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(ArtistDeleteViewModel deleteArtist)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

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