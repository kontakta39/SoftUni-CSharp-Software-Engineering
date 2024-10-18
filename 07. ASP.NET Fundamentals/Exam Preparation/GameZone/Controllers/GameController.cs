using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZone.Models.ViewModels;
using GameZone.Data;
using GameZone.Models;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameZoneDbContext _dbContext;

        public GameController(GameZoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            List<AllViewModel>? model = await _dbContext.Games
                .Where(g => g.IsDeleted == false)
                .Select(g => new AllViewModel()
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    Title = g.Title,
                    Genre = g.Genre.Name,
                    ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd"),
                    Publisher = g.Publisher.UserName ?? string.Empty
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddViewModel inputModel = new AddViewModel();
            inputModel.Genres = await _dbContext.Genres
                .AsNoTracking()
                .ToListAsync();

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Game game = new Game()
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl,
                ReleasedOn = DateTime.ParseExact(inputModel.ReleasedOn, "yyyy-MM-dd", null),
                GenreId = inputModel.GenreId,
                PublisherId = userId ?? string.Empty
            };

            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the game from the database
            Game? game = await _dbContext.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound(); // Return a 404 error if the game is not found
            }

            List<Genre> genres = await _dbContext.Genres.AsNoTracking().ToListAsync();

            EditViewModel? model = new EditViewModel
            {
                Title = game.Title,
                Description = game.Description,
                ReleasedOn = game.ReleasedOn.ToString("yyyy-MM-dd"),
                GenreId = game.GenreId,
                ImageUrl = game.ImageUrl,
                Genres = genres // Assign the fetched list of genres
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddViewModel inputModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            Game game = _dbContext.Games.FirstOrDefault(g => g.Id == id)!;

            game.Title = inputModel.Title;
            game.Description = inputModel.Description;
            game.ImageUrl = inputModel.ImageUrl;
            game.ReleasedOn = DateTime.ParseExact(inputModel.ReleasedOn, "yyyy-MM-dd", null);
            game.GenreId = inputModel.GenreId;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            DetailsViewModel? model = await _dbContext.Games
                .Where(g => g.Id == id)
                .Select(g => new DetailsViewModel()
                {
                    Id = id,
                    ImageUrl = g.ImageUrl,
                    Title = g.Title,
                    Description = g.Description,
                    Genre = g.Genre.Name,
                    ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd"),
                    Publisher = g.Publisher.UserName ?? string.Empty
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            DeleteViewModel? game = await _dbContext.Games
                .Where(g => g.Id == id && g.IsDeleted == false)
                .Select(g => new DeleteViewModel()
                {
                    Id = id,
                    Title = g.Title,
                    Publisher = g.Publisher.UserName ?? string.Empty
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(DeleteViewModel model)
        {
            Game? game = await _dbContext.Games
               .Where(g => g.Id == model.Id && g.IsDeleted == false)
               .AsNoTracking()
               .FirstOrDefaultAsync();

            if (game != null)
            {
                game.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            string getCurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            
            List<AllViewModel>? model = await _dbContext.Games
            .Where(g => g.IsDeleted == false)
            .Where(g => g.GamersGames.Any(gg => gg.GamerId == getCurrentUserId))
            .Select(g => new AllViewModel()
            {
                Id = g.Id,
                ImageUrl = g.ImageUrl,
                Title = g.Title,
                Genre = g.Genre.Name,
                ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd"),
                Publisher = g.Publisher.UserName ?? string.Empty
            })
            .AsNoTracking()
            .ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            string? gamerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            GamerGame? gamerGameCheck = await _dbContext.GamersGames
                .Where(gg => gg.GameId == id && gg.GamerId == gamerId)
                .FirstOrDefaultAsync();

            if (gamerGameCheck != null)
            {
                return RedirectToAction(nameof(All));
            }

            GamerGame gamerGame = new GamerGame()
            {
                GameId = id,
                GamerId = gamerId ?? string.Empty
            };

            await _dbContext.GamersGames.AddAsync(gamerGame!);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MyZone));  
        }

        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            string? gamerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            GamerGame? gamerGame = await _dbContext.GamersGames
                .Where(gg => gg.GameId == id && gg.GamerId == gamerId)
                .FirstOrDefaultAsync();

            if (gamerGame == null)
            {
                return RedirectToAction(nameof(MyZone));
            }

            _dbContext.GamersGames.Remove(gamerGame);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MyZone));
        }
    }
}