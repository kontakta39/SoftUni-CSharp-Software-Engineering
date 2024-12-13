using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

public class AlbumController : Controller
{
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public AlbumController(MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _context = context;
        _imageHandler = imageHandler;
    }

    public async Task<IActionResult> Index()
    {
        List<AlbumIndexViewModel> albums = await _context.Albums
           .Where(a => a.IsDeleted == false && a.Stock > 0)
           .Select(a => new AlbumIndexViewModel() 
           {
             Id = a.Id,
             Title = a.Title,
             ImageUrl = a.ImageUrl,
             Stock = a.Stock,
             Price = a.Price,
             Artist = a.Artist.Name,
             Genre = a.Genre.Name
           })
            .ToListAsync(); 

        return View(albums);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        AlbumAddViewModel addAlbum = new AlbumAddViewModel();
        addAlbum.Stock = addAlbum.Stock == 0 ? 1 : addAlbum.Stock; // Set default value for Stock if 0 or null
        addAlbum.Price = addAlbum.Price == 0 ? 1.00m : addAlbum.Price; // Set default value for Price if 0 or null

        addAlbum.Genres = await _context.Genres
            .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        addAlbum.Artists = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        return View(addAlbum);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AlbumAddViewModel addAlbum)
    {
        // Retrieve genres and artists for the form
        List<Genre> allGenres = await _context.Genres
            .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        List<Artist> allArtists = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        // Define allowed content types and extensions
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");

        if (!ModelState.IsValid)
        {
            addAlbum.Genres = allGenres;
            addAlbum.Artists = allArtists;

            // Handle image upload
            if (addAlbum.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(addAlbum.ImageFile, allowedContentTypes, allowedExtensions);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    string tempFileName = await ImageHandler.SaveTempImageAsync(addAlbum.ImageFile, tempFolderPath);
                    addAlbum.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; // Store temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addAlbum.ImageUrl = TempData["ImageUrl"].ToString(); // Retrieve the file name
            }

            TempData.Keep("ImageUrl");

            return View(addAlbum);
        }

        // Model is valid, proceed to create the album
        Album album = new Album()
        {
            Id = addAlbum.Id,
            Title = addAlbum.Title,
            Label = addAlbum.Label,
            ReleaseDate = string.IsNullOrEmpty(addAlbum.ReleaseDate)
                         ? null
                         : DateOnly.ParseExact(addAlbum.ReleaseDate, "yyyy-MM-dd", null),
            Description = addAlbum.Description,
            Price = addAlbum.Price,
            Stock = addAlbum.Stock,
            ArtistId = addAlbum.ArtistId,
            GenreId = addAlbum.GenreId,
        };

        // Handle image upload
        if (addAlbum.ImageFile != null)
        {
            string fileName = await _imageHandler.SaveFinalImageAsync(addAlbum.ImageFile);
            album.ImageUrl = fileName;
        }
        else if (TempData["ImageUrl"] != null)
        {
            string tempFileName = TempData["ImageUrl"].ToString();
            string finalFileName = ImageHandler.MoveImageToFinalFolder(tempFileName, tempFolderPath, finalFolderPath);

            if (finalFileName != null)
            {
                album.ImageUrl = finalFileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "The temporary file is missing. Please re-upload the image.");
                return View(addAlbum);
            }
        }

        await _context.Albums.AddAsync(album);
        await _context.SaveChangesAsync();

        // Clean up TempData
        TempData.Remove("ImageUrl");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetArtistsByGenre(Guid genreId)
    {
        var artists = await _context.Artists
            .Where(a => a.GenreId == genreId && a.IsDeleted == false)
            .Select(a => new { a.Id, a.Name })
            .ToListAsync();

        return Json(artists);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Album? albumCheck = _context.Albums.FirstOrDefault(a => a.Id == id);

        if (albumCheck == null)
        {
            return NotFound();
        }

        AlbumDetailsViewModel? album = await _context.Albums
            .Where(a => a.Id == id)
            .Select(a => new AlbumDetailsViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                Label = a.Label,
                ReleaseDate = a.ReleaseDate,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                Price = a.Price,
                Stock = a.Stock,
                Genre = a.Genre.Name,
                Artist = a.Artist.Name,
                ArtistId = a.Artist.Id,
                IsDeleted = a.IsDeleted,
                Reviews = _context.Reviews
                    .Where(r => r.AlbumId == id)
                    .Select(r => new ReviewIndexViewModel()
                    {
                        Id = r.Id,
                        AlbumId = r.AlbumId,
                        UserId = r.UserId,
                        FirstName = r.User.FirstName!,
                        LastName = r.User.LastName!,
                        ReviewDate = r.ReviewDate,
                        ReviewText = r.ReviewText,
                        Rating = r.Rating,
                        IsEdited = r.IsEdited
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return View(album);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        Album? album = _context.Albums.FirstOrDefault(g => g.Id == id);

        if (album == null)
        {
            return NotFound();
        }

        List<Genre> allGenres = await _context.Genres
            .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        List<Artist> allArtists = await _context.Artists
            .AsNoTracking()
            .ToListAsync();

        AlbumEditViewModel? editAlbum = new AlbumEditViewModel()
            {
                Id = album.Id,
                Title = album.Title,
                Label = album.Label,
                ReleaseDate = album.ReleaseDate.ToString(),
                Description = album.Description,
                ImageUrl = album.ImageUrl,
                Price = album.Price,
                Stock = album.Stock,
                ArtistId = album.ArtistId,
                GenreId = album.GenreId,
                Genres = allGenres,
                Artists = allArtists
            };
       
        return View(editAlbum);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AlbumEditViewModel editAlbum, Guid id)
    {
        List<Genre> allGenres = await _context.Genres
            .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        List<Artist> allArtists = await _context.Artists
            .AsNoTracking()
            .ToListAsync();

        TempData["CurrentImageUrl"] = await _context.Albums
            .Where(a => a.Id == id && a.IsDeleted == false)
            .Select(a => a.ImageUrl)
            .FirstOrDefaultAsync();

        // Validate the uploaded image using ImageHandler
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");

        if (!ModelState.IsValid)
        {
            editAlbum.Genres = allGenres;
            editAlbum.Artists = allArtists;

            // Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editAlbum.ImageFile != null)
            {
                // Delete the old image if it's not the default one
                Album? albumCheck = _context.Albums
                    .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

                string validationError = ImageHandler.ValidateImage(editAlbum.ImageFile, allowedContentTypes, allowedExtensions);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    if (albumCheck.ImageUrl != null)
                    {
                        // Use ImageHandler to delete the old image
                        _imageHandler.DeleteImage(albumCheck.ImageUrl, finalFolderPath);
                    }

                    // Save the file temporarily using ImageHandler
                    string tempFileName = await ImageHandler.SaveTempImageAsync(editAlbum.ImageFile, tempFolderPath);
                    editAlbum.ImageUrl = tempFileName;
                    TempData["NewImageUrl"] = tempFileName; // Store the file name temporarily
                }
            }
            else if (TempData["NewImageUrl"] != null)
            {
                editAlbum.ImageUrl = TempData["NewImageUrl"].ToString(); // Retrieve the file name from TempData
                TempData.Keep("NewImageUrl"); // Preserve TempData for subsequent requests
            }
            else
            {
                editAlbum.ImageUrl = TempData["CurrentImageUrl"].ToString(); // Retrieve the file name from TempData
            }

            return View(editAlbum);
        }

        Album album = _context.Albums
            .FirstOrDefault(a => a.Id == id && a.IsDeleted == false)!;

        album.Id = editAlbum.Id;
        album.Title = editAlbum.Title;
        album.Label = editAlbum.Label;
        album.ReleaseDate = string.IsNullOrEmpty(editAlbum.ReleaseDate)
                    ? null
                    : DateOnly.ParseExact(editAlbum.ReleaseDate, "yyyy-MM-dd", null);
        album.Description = editAlbum.Description;
        album.Price = editAlbum.Price;
        album.Stock = editAlbum.Stock;
        album.ArtistId = editAlbum.ArtistId;
        album.GenreId = editAlbum.GenreId;

        // Handle image upload (copy the file to server only if ModelState is valid)
        if (editAlbum.ImageFile != null)
        {
            // Delete the old image if it's not the default one
            if (album.ImageUrl != null)
            {
                _imageHandler.DeleteImage(album.ImageUrl, finalFolderPath);
            }

            // Save the file to the final folder using ImageHandler
            string fileName = await _imageHandler.SaveFinalImageAsync(editAlbum.ImageFile);
            album.ImageUrl = fileName; // Save the file name in the database
        }
        else if (TempData["NewImageUrl"] != null && album.ImageUrl == null)
        {
            string fileName = TempData["NewImageUrl"].ToString();
            string currentTempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderPath, fileName);

            if (System.IO.File.Exists(currentTempFilePath))
            {
                // Use ImageHandler to move the image from temp to final folder
                ImageHandler.MoveImageToFinalFolder(fileName, tempFolderPath, finalFolderPath);
                album.ImageUrl = fileName; // Save the file name in the database
            }
            else
            {
                ModelState.AddModelError("ImageFile", "The temporary file is missing. Please re-upload the image.");
                return View(editAlbum);
            }
        }

        await _context.SaveChangesAsync();

        // Clean up TempData
        TempData.Remove("CurrentImageUrl");
        TempData.Remove("NewImageUrl");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        AlbumDeleteViewModel? deleteAlbum = await _context.Albums
            .Where(p => p.Id == id && p.IsDeleted == false)
            .Select(p => new AlbumDeleteViewModel()
            {
                Id = id,
                Title = p.Title,
                Artist = p.Artist.Name
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (deleteAlbum == null)
        {
            return NotFound();
        }

        return View(deleteAlbum);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(AlbumDeleteViewModel deleteAlbum)
    {
        Album? album = await _context.Albums
           .Where(p => p.Id == deleteAlbum.Id && p.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (album != null && album?.Stock == 0)
        {
            album.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}