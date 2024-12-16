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
    private readonly IArtistInterface _artistService;
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public ArtistController(IArtistInterface artistInterface, MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _artistService = artistInterface;
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

        List<ArtistIndexViewModel> artists = await _artistService.Index();
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

        try
        {
            ArtistAddViewModel addArtist = await _artistService.Add();
            return View(addArtist);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(ArtistAddViewModel addArtist)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");

        if (!ModelState.IsValid)
        {
            addArtist.Genres = await _context.Genres
                .Where(g => !g.IsDeleted)
                .ToListAsync();

            if (!addArtist.Genres.Any())
            {
                return NotFound();
            }

            addArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload
            if (addArtist.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(addArtist.ImageFile);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    string tempFileName = await ImageHandler.SaveTempImageAsync(addArtist.ImageFile, tempFolderPath);
                    addArtist.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; //Store temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addArtist.ImageUrl = TempData["ImageUrl"].ToString(); //Retrieve the file name
            }

            TempData.Keep("ImageUrl");

            return View(addArtist);
        }

        try
        {
            string tempDataImageUrl = TempData["ImageUrl"]?.ToString();
            await _artistService.Add(addArtist, tempFolderPath, finalFolderPath, tempDataImageUrl);
            TempData.Remove("ImageUrl");
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            ArtistDetailsViewModel? artist = await _artistService.Details(id);
            return View(artist);
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
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            ArtistEditViewModel? editArtist = await _artistService.Edit(id);
            return View(editArtist);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(ArtistEditViewModel editArtist, Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        Artist? artistCheck = _context.Artists
        .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

        if (artistCheck == null)
        {
            return NotFound();
        }

        TempData["CurrentImageUrl"] = artistCheck.ImageUrl;

        //Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images");

        if (!ModelState.IsValid)
        {
            editArtist.Genres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

            if (!editArtist.Genres.Any())
            {
                return NotFound();
            }

            editArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editArtist.ImageFile != null)
            {
                //Delete the old image if it's not the default one
                string validationError = ImageHandler.ValidateImage(editArtist.ImageFile);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    if (artistCheck.ImageUrl != null)
                    {
                        //Use ImageHandler to delete the old image
                        _imageHandler.DeleteImage(artistCheck.ImageUrl, finalFolderPath);
                    }

                    //Save the file temporarily using ImageHandler
                    string tempFileName = await ImageHandler.SaveTempImageAsync(editArtist.ImageFile, tempFolderPath);
                    editArtist.ImageUrl = tempFileName;
                    TempData["NewImageUrl"] = tempFileName; //Store the file name temporarily
                }
            }
            else if (TempData["NewImageUrl"] != null)
            {
                editArtist.ImageUrl = TempData["NewImageUrl"].ToString(); //Retrieve the file name from TempData
                TempData.Keep("NewImageUrl"); //Preserve TempData for subsequent requests
            }
            else
            {
                editArtist.ImageUrl = TempData["CurrentImageUrl"].ToString(); //Retrieve the file name from TempData
            }

            return View(editArtist);
        }

        try
        {
            string tempImageUrl = TempData["NewImageUrl"].ToString();
            await _artistService.Edit(editArtist, id, tempFolderPath, finalFolderPath, tempImageUrl);

            TempData.Remove("CurrentImageUrl");
            TempData.Remove("NewImageUrl");

            return RedirectToAction("Details", "Artist", new { id = artistCheck.Id });
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
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
            ArtistDeleteViewModel? deleteArtist = await _artistService.Delete(id);
            return View(deleteArtist);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(ArtistDeleteViewModel deleteArtist)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        try
        {
            await _artistService.Delete(deleteArtist);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }
}