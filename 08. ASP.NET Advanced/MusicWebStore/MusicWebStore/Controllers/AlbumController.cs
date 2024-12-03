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
        List<Genre> allGenres = await _context.Genres
            .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        List<Artist> allArtists = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        if (!ModelState.IsValid)
        {
            addAlbum.Genres = allGenres;
            addAlbum.Artists = allArtists;

            // Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (addAlbum.ImageFile != null)
            {
                // Validate the uploaded image
                string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (!allowedContentTypes.Contains(addAlbum.ImageFile.ContentType) ||
                    !allowedExtensions.Contains(Path.GetExtension(addAlbum.ImageFile.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                }
                else
                {
                    string tempFileName = Path.GetFileName(addAlbum.ImageFile.FileName);
                    string tempSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", tempFileName);

                    // Save the file temporarily
                    Directory.CreateDirectory(Path.GetDirectoryName(tempSavePath)); // Ensure the tmp folder exists
                    using (FileStream? stream = new FileStream(tempSavePath, FileMode.Create))
                    {
                        await addAlbum.ImageFile.CopyToAsync(stream);
                    }

                    addAlbum.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; // Store the file name temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            { 
                addAlbum.ImageUrl = TempData["ImageUrl"].ToString(); // Retrieve the file name from TempData
            }

            TempData.Keep("ImageUrl"); // Preserve TempData for subsequent requests

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

        // Handle image upload (copy the file to server only if ModelState is valid)
        if (addAlbum.ImageFile != null)
        {
            string fileName = Path.GetFileName(addAlbum.ImageFile.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", fileName);

            // Save the file in the final destination
            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await addAlbum.ImageFile.CopyToAsync(stream);
            }

            album.ImageUrl = fileName; // Save the file name in the database
        }
        else if (TempData["ImageUrl"] != null)
        {
            string fileName = TempData["ImageUrl"].ToString();
            string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", fileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", fileName);

            if (System.IO.File.Exists(tempFilePath))
            {
                // Move the file from the temp folder to the final destination
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)); // Ensure the final folder exists
                System.IO.File.Move(tempFilePath, savePath);
                album.ImageUrl = fileName; // Save the file name in the database
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
            return RedirectToAction(nameof(Index));
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
                ArtistId = a.Artist.Id
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

        if (!ModelState.IsValid)
        {
            editAlbum.Genres = allGenres;
            editAlbum.Artists = allArtists;

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editAlbum.ImageFile != null)
            {
                //Delete the old image if it's not the default one
                Album? albumCheck = _context.Albums
                    .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

                if (albumCheck.ImageUrl != null)
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", albumCheck.ImageUrl);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //Validate the uploaded image
                string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (!allowedContentTypes.Contains(editAlbum.ImageFile.ContentType) ||
                    !allowedExtensions.Contains(Path.GetExtension(editAlbum.ImageFile.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                }
                else
                {
                    string tempFileName = Path.GetFileName(editAlbum.ImageFile.FileName);
                    string tempSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", tempFileName);

                    //Save the file temporarily
                    Directory.CreateDirectory(Path.GetDirectoryName(tempSavePath)); //Ensure the tmp folder exists
                    using (FileStream? stream = new FileStream(tempSavePath, FileMode.Create))
                    {
                        await editAlbum.ImageFile.CopyToAsync(stream);
                    }

                    editAlbum.ImageUrl = tempFileName;
                    TempData["NewImageUrl"] = tempFileName; //Store the file name temporarily
                }
            }
            else if (TempData["NewImageUrl"] != null)
            {
                editAlbum.ImageUrl = TempData["NewImageUrl"].ToString(); //Retrieve the file name from TempData
                TempData.Keep("NewImageUrl"); //Preserve TempData for subsequent requests
            }
            else 
            {
                editAlbum.ImageUrl = TempData["CurrentImageUrl"].ToString(); //Retrieve the file name from TempData
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

        //Handle image upload (copy the file to server only if ModelState is valid)
        if (editAlbum.ImageFile != null)
        {
            //Delete the old image if it's not the default one
            if (album.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", album.ImageUrl);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            string fileName = Path.GetFileName(editAlbum.ImageFile.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", fileName);

            //Save the file in the final destination
            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await editAlbum.ImageFile.CopyToAsync(stream);
            }

            album.ImageUrl = fileName; // Save the file name in the database
        }
        else if (TempData["NewImageUrl"] != null && album.ImageUrl == null)
        {
            string fileName = TempData["NewImageUrl"].ToString();
            string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", fileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers", fileName);

            if (System.IO.File.Exists(tempFilePath))
            {
                // Move the file from the temp folder to the final destination
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)); // Ensure the final folder exists
                System.IO.File.Move(tempFilePath, savePath);
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

        return RedirectToAction("Details", "Album", new { id = album.Id });
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