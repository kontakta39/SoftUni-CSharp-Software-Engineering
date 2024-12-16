using Microsoft.EntityFrameworkCore;
using MusicWebStore.Constants;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class ArtistService : IArtistInterface
{
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public ArtistService(MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _context = context;
        _imageHandler = imageHandler;
    }

    public async Task<List<ArtistIndexViewModel>> Index()
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

        return artists;
    }

    public async Task<ArtistAddViewModel> Add()
    {
        List<Genre> allGenres = await _context.Genres
           .Where(g => g.IsDeleted == false)
           .ToListAsync();

        if (!allGenres.Any())
        {
            throw new ArgumentException();
        }

        ArtistAddViewModel addArtist = new ArtistAddViewModel();
        addArtist.NationalityOptions = CountriesConstants.CountriesList();
        addArtist.Genres = allGenres;

        return addArtist;
    }

    public async Task Add(ArtistAddViewModel addArtist, string tempFolderPath, string finalFolderPath, string tempImageUrl)
    {
        Artist artist = new Artist
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

        if (addArtist.ImageFile != null)
        {
            artist.ImageUrl = await _imageHandler.SaveFinalImageAsync(addArtist.ImageFile);
        }
        else if (!string.IsNullOrEmpty(tempImageUrl))
        {
            string finalFileName = ImageHandler.MoveImageToFinalFolder(tempImageUrl, tempFolderPath, finalFolderPath);

            if (finalFileName == null)
            {
                throw new ArgumentNullException();
            }

            artist.ImageUrl = finalFileName;
        }

        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();
    }

    public async Task<ArtistDetailsViewModel> Details(Guid id)
    {
        Artist? artistCheck = await _context.Artists
           .Where(a => a.IsDeleted == false)
           .FirstOrDefaultAsync(a => a.Id == id);

        if (artistCheck == null)
        {
            throw new ArgumentNullException();
        }

        Genre? currentGenre = await _context.Genres
            .Where(g => g.Id == artistCheck.GenreId)
            .FirstOrDefaultAsync();

        if (currentGenre == null)
        {
            throw new ArgumentNullException();
        }

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

        return artist;
    }

    public async Task<ArtistEditViewModel> Edit(Guid id)
    {
        Artist? artist = _context.Artists
           .FirstOrDefault(a => a.Id == id && a.IsDeleted == false);

        if (artist == null)
        {
            throw new ArgumentException();
        }

        List<Genre> allGenres = await _context.Genres
            .Where(g => g.IsDeleted == false)
            .ToListAsync();

        if (!allGenres.Any())
        { 
            throw new ArgumentException();
        }

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

        return editArtist;
    }

    public async Task Edit(ArtistEditViewModel editArtist, Guid id, string tempFolderPath, string finalFolderPath, string tempImageUrl)
    {
        Artist artist = _context.Artists
           .FirstOrDefault(a => a.Id == id && a.IsDeleted == false)!;

        if (artist == null)
        {
            throw new ArgumentNullException();
        }

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
            //Delete the old image if it's not the default one
            if (artist.ImageUrl != null)
            {
                _imageHandler.DeleteImage(artist.ImageUrl, finalFolderPath);
            }

            //Save the file to the final folder using ImageHandler
            string fileName = await _imageHandler.SaveFinalImageAsync(editArtist.ImageFile);
            artist.ImageUrl = fileName; //Save the file name in the database
        }
        else if (tempImageUrl != null && artist.ImageUrl != null)
        {
            string fileName = tempImageUrl;
            string currentTempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderPath, fileName);

            if (File.Exists(currentTempFilePath))
            {
                //Use ImageHandler to move the image from temp to final folder
                ImageHandler.MoveImageToFinalFolder(fileName, tempFolderPath, finalFolderPath);
                artist.ImageUrl = fileName; //Save the file name in the database
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ArtistDeleteViewModel> Delete(Guid id)
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
            throw new ArgumentNullException();
        }

        return deleteArtist;
    }

    public async Task Delete(ArtistDeleteViewModel deleteArtist)
    {
        Artist? artist = await _context.Artists
          .Where(p => p.Id == deleteArtist.Id && p.IsDeleted == false)
          .FirstOrDefaultAsync();

        if (artist != null)
        {
            artist.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
}