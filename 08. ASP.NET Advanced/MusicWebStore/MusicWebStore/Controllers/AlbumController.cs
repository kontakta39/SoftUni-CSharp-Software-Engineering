using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Constants;
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
            ImageUrl = addAlbum.ImageUrl,
            Price = addAlbum.Price,
            Stock = addAlbum.Stock,
            ArtistId = addAlbum.ArtistId,
            GenreId = addAlbum.GenreId,
        };

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
        album.ImageUrl = editAlbum.ImageUrl;
        album.Price = editAlbum.Price;
        album.Stock = editAlbum.Stock;
        album.ArtistId = editAlbum.ArtistId;
        album.GenreId = editAlbum.GenreId;

        await _context.SaveChangesAsync();

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