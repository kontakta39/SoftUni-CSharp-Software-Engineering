using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Controllers;

public class AlbumController : Controller
{
    private readonly IAlbumInterface _albumService;
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public AlbumController(IAlbumInterface albumService, MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _albumService = albumService;
        _context = context;
        _imageHandler = imageHandler;
    }

    public async Task<IActionResult> Index()
    {
        List<AlbumIndexViewModel> albums = await _albumService.Index();
        return View(albums);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Add()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            AlbumAddViewModel addAlbum = await _albumService.Add();
            return View(addAlbum);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(AlbumAddViewModel addAlbum)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home"); 
        }

        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");

        if (!ModelState.IsValid)
        {
            List<Genre> allGenres = await _context.Genres
                 .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
                 .AsNoTracking()
                 .ToListAsync();

            List<Artist> allArtists = await _context.Artists
                .Where(a => a.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();

            if (allGenres == null || allArtists == null)
            { 
                return NotFound();
            }

            //Handle image upload
            if (addAlbum.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(addAlbum.ImageFile);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    string tempFileName = await ImageHandler.SaveTempImageAsync(addAlbum.ImageFile, tempFolderPath);
                    addAlbum.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; //Store temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addAlbum.ImageUrl = TempData["ImageUrl"].ToString(); //Retrieve the file name
            }

            TempData.Keep("ImageUrl");

            return View(addAlbum);
        }

        try
        {
            string tempImageUrl = TempData["ImageUrl"].ToString();
            await _albumService.Add(addAlbum, tempFolderPath, finalFolderPath, tempImageUrl);
            TempData.Remove("ImageUrl");
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetArtistsByGenre(Guid genreId)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        var artists = await _context.Artists
            .Where(a => a.GenreId == genreId && a.IsDeleted == false)
            .Select(a => new { a.Id, a.Name })
            .ToListAsync();

        if (!artists.Any())
        {
            return NotFound();
        }

        return Json(artists);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            AlbumDetailsViewModel? album = await _albumService.Details(id);
            return View(album);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }


    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!User.IsInRole("Administrator") && !User.IsInRole("Moderator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            AlbumEditViewModel? editAlbum = await _albumService.Edit(id);
            return View(editAlbum);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(AlbumEditViewModel editAlbum, Guid id)
    {
        if (!User.IsInRole("Administrator") && !User.IsInRole("Moderator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Album album = _context.Albums
        .FirstOrDefault(a => a.Id == id && a.IsDeleted == false)!;

        if (album == null)
        {
            return NotFound();
        }

        //Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");

        List<Genre> allGenres = await _context.Genres
        .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
        .AsNoTracking()
        .ToListAsync();

        List<Artist> allArtists = await _context.Artists
        .Where(a => a.IsDeleted == false)
        .AsNoTracking()
        .ToListAsync();

        if (!allGenres.Any() || !allArtists.Any())
        {
            return NotFound();
        }

        if (User.IsInRole("Administrator"))
        {
            if (!ModelState.IsValid)
            {
                editAlbum.Genres = allGenres;
                editAlbum.Artists = allArtists;

                //Handle image upload (only store the filename in TempData if ModelState is not valid)
                if (editAlbum.ImageFile != null)
                {
                    //Delete the old image if it's not the default one
                    TempData["CurrentImageUrl"] = album.ImageUrl;

                    string validationError = ImageHandler.ValidateImage(editAlbum.ImageFile);

                    if (!string.IsNullOrEmpty(validationError))
                    {
                        ModelState.AddModelError("ImageFile", validationError);
                    }
                    else
                    {
                        if (album.ImageUrl != null)
                        {
                            //Use ImageHandler to delete the old image
                            _imageHandler.DeleteImage(album.ImageUrl, finalFolderPath);
                        }

                        //Save the file temporarily using ImageHandler
                        string tempFileName = await ImageHandler.SaveTempImageAsync(editAlbum.ImageFile, tempFolderPath);
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

            try
            {
                string role = "Administrator";
                string tempImageUrl = TempData["NewImageUrl"].ToString();
                await _albumService.Edit(editAlbum, id, tempFolderPath, finalFolderPath, tempImageUrl, role);
                TempData.Remove("CurrentImageUrl");
                TempData.Remove("NewImageUrl");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
        else if (User.IsInRole("Moderator"))
        {
            if (!ModelState.IsValid)
            {
                editAlbum.Genres = allGenres;
                editAlbum.Artists = allArtists;

                return View(editAlbum);
            }

            string role = "Moderator";
            string tempImageUrl = TempData["NewImageUrl"].ToString();
            await _albumService.Edit(editAlbum, id, tempFolderPath, finalFolderPath, tempImageUrl, role);
        }

        return RedirectToAction("Details", "Album", new { id = album.Id });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            AlbumDeleteViewModel? deleteAlbum = await _albumService.Delete(id);
            return View(deleteAlbum);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(AlbumDeleteViewModel deleteAlbum)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            await _albumService.Delete(deleteAlbum);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }
}