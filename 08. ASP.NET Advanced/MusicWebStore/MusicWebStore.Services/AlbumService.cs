using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class AlbumService : IAlbumInterface
{
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public AlbumService(MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _context = context;
        _imageHandler = imageHandler;
    }

    public async Task<List<AlbumIndexViewModel>> Index()
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

        return albums;
    }

    public async Task<AlbumAddViewModel> Add()
    {
        AlbumAddViewModel addAlbum = new AlbumAddViewModel();
        addAlbum.Stock = addAlbum.Stock == 0 ? 1 : addAlbum.Stock; //Set default value for Stock if 0 or null
        addAlbum.Price = addAlbum.Price == 0 ? 1.00m : addAlbum.Price; //Set default value for Price if 0 or null

        addAlbum.Genres = await _context.Genres
            .Where(g => _context.Artists.Any(a => a.GenreId == g.Id) && g.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        addAlbum.Artists = await _context.Artists
            .Where(a => a.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();

        if (!addAlbum.Genres.Any() || !addAlbum.Artists.Any())
        {
            throw new ArgumentException();
        }

        return addAlbum;
    }

    public async Task Add(AlbumAddViewModel addAlbum, string tempFolderPath, string finalFolderPath, string tempImageUrl)
    {
        Album album = new Album()
        {
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

        //Handle image upload
        if (addAlbum.ImageFile != null)
        {
            string fileName = await _imageHandler.SaveFinalImageAsync(addAlbum.ImageFile);
            album.ImageUrl = fileName;
        }
        else if (tempImageUrl != null)
        {
            string tempFileName = tempImageUrl;
            string finalFileName = ImageHandler.MoveImageToFinalFolder(tempFileName, tempFolderPath, finalFolderPath);

            if (finalFileName != null)
            {
                album.ImageUrl = finalFileName;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        await _context.Albums.AddAsync(album);
        await _context.SaveChangesAsync();
    }

    public async Task<AlbumDetailsViewModel> Details(Guid id)
    {
        Album? albumCheck = _context.Albums
       .Where(a => a.Id == id && a.IsDeleted == false)
       .FirstOrDefault();

        if (albumCheck == null)
        {
            throw new ArgumentNullException();
        }

        AlbumDetailsViewModel? album = await _context.Albums
            .Where(a => a.Id == id && a.IsDeleted == false)
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

        return album;
    }

    public async Task<AlbumEditViewModel> Edit(Guid id)
    {
        Album? album = _context.Albums
        .Where(a => a.Id == a.Id && a.IsDeleted == false)
        .FirstOrDefault();

        if (album == null)
        {
            throw new ArgumentNullException();
        }

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
            throw new ArgumentException();
        }

        AlbumEditViewModel? editAlbum = new AlbumEditViewModel()
        {
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

        return editAlbum;
    }

    public async Task Edit(AlbumEditViewModel editAlbum, Guid id, string tempFolderPath, string finalFolderPath, string tempImageUrl, string role)
    {
        Album album = _context.Albums
        .FirstOrDefault(a => a.Id == id && a.IsDeleted == false)!;

        if (album == null)
        {
            throw new ArgumentNullException();
        }

        if (role == "Administrator")
        {
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
                    _imageHandler.DeleteImage(album.ImageUrl, finalFolderPath);
                }

                //Save the file to the final folder using ImageHandler
                string fileName = await _imageHandler.SaveFinalImageAsync(editAlbum.ImageFile);
                album.ImageUrl = fileName; //Save the file name in the database
            }
            else if (tempImageUrl != null && album.ImageUrl == null)
            {
                string fileName = tempImageUrl;
                string currentTempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderPath, fileName);

                if (File.Exists(currentTempFilePath))
                {
                    //Use ImageHandler to move the image from temp to final folder
                    ImageHandler.MoveImageToFinalFolder(fileName, tempFolderPath, finalFolderPath);
                    album.ImageUrl = fileName; //Save the file name in the database
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
        }
        else if (role == "Moderator")
        {
            album.Label = editAlbum.Label;
            album.ReleaseDate = string.IsNullOrEmpty(editAlbum.ReleaseDate)
                        ? null
                        : DateOnly.ParseExact(editAlbum.ReleaseDate, "yyyy-MM-dd", null);
            album.Description = editAlbum.Description;
            album.Price = editAlbum.Price;
            album.Stock = editAlbum.Stock;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<AlbumDeleteViewModel> Delete(Guid id)
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
            throw new ArgumentNullException();
        }

        return deleteAlbum;
    }

    public async Task Delete(AlbumDeleteViewModel deleteAlbum)
    {
        Album? album = await _context.Albums
           .Where(p => p.Id == deleteAlbum.Id && p.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (album != null)
        {
            album.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        else
        { 
            throw new ArgumentNullException();
        }
    }
}