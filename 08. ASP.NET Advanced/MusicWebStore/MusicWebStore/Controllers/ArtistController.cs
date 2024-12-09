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
        List<Genre> allGenres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        ArtistAddViewModel addArtist = new ArtistAddViewModel();
        addArtist.NationalityOptions = CountriesConstants.CountriesList();
        addArtist.Genres = allGenres;

        return View(addArtist);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ArtistAddViewModel addArtist)
    {
        List<Genre> allGenres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        addArtist.Genres = allGenres;
        addArtist.NationalityOptions = CountriesConstants.CountriesList();

        if (!ModelState.IsValid)
        {
            addArtist.Genres = allGenres;
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

        //Handle image upload (copy the file to server only if ModelState is valid)
        if (addArtist.ImageFile != null)
        {
            string fileName = Path.GetFileName(addArtist.ImageFile.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", fileName);

            //Save the file in the final destination
            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await addArtist.ImageFile.CopyToAsync(stream);
            }

            artist.ImageUrl = fileName; //Save the file name in the database
        }
        else if (TempData["ImageUrl"] != null)
        {
            string fileName = TempData["ImageUrl"].ToString();
            string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", fileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", fileName);

            if (System.IO.File.Exists(tempFilePath))
            {
                //Move the file from the temp folder to the final destination
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)); //Ensure the final folder exists
                System.IO.File.Move(tempFilePath, savePath);
                artist.ImageUrl = fileName; //Save the file name in the database
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
    public async Task<IActionResult> Edit(Guid id)
    {
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
        List<Genre> genres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        TempData["CurrentImageUrl"] = await _context.Artists
            .Where(a => a.Id == id && a.IsDeleted == false)
            .Select(a => a.ImageUrl)
            .FirstOrDefaultAsync();

        if (!ModelState.IsValid)
        {
            editArtist.Genres = genres;
            editArtist.NationalityOptions = CountriesConstants.CountriesList();

            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editArtist.ImageFile != null)
            {
                //Delete the old image if it's not the default one
                Artist? artistCheck = _context.Artists
                    .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

                if (artistCheck.ImageUrl != null)
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", artistCheck.ImageUrl);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //Validate the uploaded image
                string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (!allowedContentTypes.Contains(editArtist.ImageFile.ContentType) ||
                    !allowedExtensions.Contains(Path.GetExtension(editArtist.ImageFile.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPG, JPEG, PNG, GIF, WEBP).");
                }
                else
                {
                    string tempFileName = Path.GetFileName(editArtist.ImageFile.FileName);
                    string tempSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", tempFileName);

                    //Save the file temporarily
                    Directory.CreateDirectory(Path.GetDirectoryName(tempSavePath)); //Ensure the tmp folder exists
                    using (FileStream? stream = new FileStream(tempSavePath, FileMode.Create))
                    {
                        await editArtist.ImageFile.CopyToAsync(stream);
                    }

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

        ///Handle image upload (copy the file to server only if ModelState is valid)
        if (editArtist.ImageFile != null)
        {
            //Delete the old image if it's not the default one
            if (artist.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", artist.ImageUrl);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            string fileName = Path.GetFileName(editArtist.ImageFile.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", fileName);

            //Save the file in the final destination
            using (FileStream? stream = new FileStream(savePath, FileMode.Create))
            {
                await editArtist.ImageFile.CopyToAsync(stream);
            }

            artist.ImageUrl = fileName; // Save the file name in the database
        }
        else if (TempData["NewImageUrl"] != null && artist.ImageUrl == null)
        {
            string fileName = TempData["NewImageUrl"].ToString();
            string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp", fileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images", fileName);

            if (System.IO.File.Exists(tempFilePath))
            {
                // Move the file from the temp folder to the final destination
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)); // Ensure the final folder exists
                System.IO.File.Move(tempFilePath, savePath);
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

        if (deleteArtist == null)
        {
            return NotFound();
        }

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