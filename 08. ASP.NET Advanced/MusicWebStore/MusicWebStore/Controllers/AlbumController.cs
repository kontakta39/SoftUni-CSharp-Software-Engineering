using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

public class AlbumController : Controller
{
    private readonly MusicStoreDbContext _context;

    public AlbumController(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<AlbumIndexViewModel> albums = await _context.Albums
           .Where(a => a.IsDeleted == false)
           .Select(a => new AlbumIndexViewModel() 
           {
             Id = a.Id,
             Title = a.Title,
             ImageUrl = a.ImageUrl,
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
        addAlbum.Artists = await _context.Artists
            .AsNoTracking()
            .ToListAsync();

        addAlbum.Genres = await _context.Genres
            .AsNoTracking()
            .ToListAsync();

        return View(addAlbum);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AlbumAddViewModel addAlbum)
    {
        addAlbum.Artists = await _context.Artists
            .AsNoTracking()
            .ToListAsync();

        addAlbum.Genres = await _context.Genres
            .AsNoTracking()
            .ToListAsync();

        if (!ModelState.IsValid)
        {
            return View(addAlbum);
        }

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
            // Validate the uploaded image
            string[] allowedContentTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowedContentTypes.Contains(addAlbum.ImageFile.ContentType) ||
                !allowedExtensions.Contains(Path.GetExtension(addAlbum.ImageFile.FileName).ToLower()))
            {
                ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                return View(addAlbum);
            }

            // Get the original file name
            string fileName = Path.GetFileName(addAlbum.ImageFile.FileName); // Extract only the file name
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", fileName);

            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await addAlbum.ImageFile.CopyToAsync(stream);
            }

            album.ImageUrl = fileName; // Save the original file name for future retrieval
        }

        await _context.Albums.AddAsync(album);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Album? albumCheck = _context.Albums
            .Where(a => a.IsDeleted == false)
            .FirstOrDefault(a => a.Id == id);

        if (albumCheck == null)
        {
            return RedirectToAction(nameof(Index));
        }

        AlbumDetailsViewModel? album = await _context.Albums
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
                Artist = a.Artist.Name
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
            return RedirectToAction(nameof(Index));
        }

        List<Genre> allGenres = await _context.Genres.ToListAsync();
        List<Artist> allArtists = await _context.Artists.ToListAsync();

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
        List<Genre> allGenres = await _context.Genres.ToListAsync();
        List<Artist> allArtists = await _context.Artists.ToListAsync();

        if (!ModelState.IsValid)
        {
            return View(editAlbum);
        }

        Album album = _context.Albums.FirstOrDefault(g => g.Id == id)!;

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

        // Handle image upload
        if (editAlbum.ImageFile != null)
        {
            // Validate the uploaded image
            string[] allowedContentTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowedContentTypes.Contains(editAlbum.ImageFile.ContentType) ||
                !allowedExtensions.Contains(Path.GetExtension(editAlbum.ImageFile.FileName).ToLower()))
            {
                ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                return View(editAlbum);
            }

            // Delete the old image if it's not the default one
            if (album.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", album.ImageUrl);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Get the original file name
            string fileName = Path.GetFileName(editAlbum.ImageFile.FileName); // Extract only the file name
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", fileName);

            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await editAlbum.ImageFile.CopyToAsync(stream);
            }

            album.ImageUrl = fileName; // Save the original file name for future retrieval
        }
        else
        {
            // Delete the old image if it's not the default one
            if (album.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", album.ImageUrl);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // If no new image is uploaded and no existing image, set null
            album.ImageUrl = null;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Album", new { id = album.Id });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        Album? album = await _context.Albums
            .Where(a => a.IsDeleted == false)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
        {
            return RedirectToAction(nameof(Index)); // Optionally redirect to another page
        }

        if (!string.IsNullOrEmpty(album.ImageUrl))
        {
            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", album.ImageUrl);

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            album.ImageUrl = null;
            await _context.SaveChangesAsync();
        }

        return Ok(); // This returns a success response to the client
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