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

        ArtistAddViewModel addArtist = await _artistService.Add();
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

        if (!ModelState.IsValid)
        {
            addArtist.Genres = await _context.Genres
                .Where(g => !g.IsDeleted)
                .ToListAsync();

            addArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (addArtist.ImageFile != null)
            {
                //Validate the uploaded image
                string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (!allowedContentTypes.Contains(addArtist.ImageFile.ContentType) ||
                    !allowedExtensions.Contains(Path.GetExtension(addArtist.ImageFile.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                }
                else
                {
                    string tempFileName = Path.GetFileName(addArtist.ImageFile.FileName);
                    string tempSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", tempFileName);

                    //Save the file temporarily
                    Directory.CreateDirectory(Path.GetDirectoryName(tempSavePath)); //Ensure the tmp folder exists
                    using (FileStream? stream = new FileStream(tempSavePath, FileMode.Create))
                    {
                        await addArtist.ImageFile.CopyToAsync(stream);
                    }

                    addArtist.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; //Store the file name temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addArtist.ImageUrl = TempData["ImageUrl"].ToString(); //Retrieve the file name from TempData
            }

            TempData.Keep("ImageUrl"); //Preserve TempData for subsequent requests

            return View(addArtist);
        }

        try
        {
            string tempDataImageUrl = TempData["ImageUrl"]?.ToString();
            await _artistService.Add(addArtist, tempDataImageUrl);

            TempData.Remove("ImageUrl");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(addArtist);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        ArtistDetailsViewModel? artist = await _artistService.Details(id);

        if (artist == null)
        {
            return NotFound();
        }

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

        ArtistEditViewModel? editArtist = await _artistService.Edit(id);

        if (editArtist == null)
        {
            return NotFound();
        }

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

        Artist? artistCheck = _context.Artists
        .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

        if (artistCheck == null)
        {
            return NotFound();
        }

        TempData["CurrentImageUrl"] = artistCheck.ImageUrl;

        //Validate the uploaded image using ImageHandler
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        //Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images");

        if (!ModelState.IsValid)
        {
            editArtist.Genres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

            editArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editArtist.ImageFile != null)
            {
                //Delete the old image if it's not the default one
                string validationError = ImageHandler.ValidateImage(editArtist.ImageFile, allowedContentTypes, allowedExtensions);

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
            string newImageUrl = TempData["NewImageUrl"].ToString();
            await _artistService.Edit(editArtist, id, newImageUrl);

            // Clean up TempData
            TempData.Remove("CurrentImageUrl");
            TempData.Remove("NewImageUrl");

            return RedirectToAction("Details", "Artist", new { id = artistCheck.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(editArtist);
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

        ArtistDeleteViewModel? deleteArtist = await _artistService.Delete(id);

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

       await _artistService.Delete(deleteArtist);
        return RedirectToAction(nameof(Index));
    }
}