using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly MusicStoreDbContext _context;

    public OrderController(MusicStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        string getCurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        List<OrderCartViewModel>? model = await _context.Orders
        .Where(o => o.BuyerAlbums.Any(ba => ba.BuyerId == getCurrentUserId))
        .Select(o => new OrderCartViewModel()
        {
            Id = o.Id,
            ImageUrl = o.ImageUrl,
            AlbumTitle = o.AlbumTitle,
            Quantity = o.Quantity,
            UnitPrice = o.UnitPrice
        })
        .AsNoTracking()
        .ToListAsync();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(Guid albumId)
    {
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Album? albumCheck = await _context.Albums
            .Where(a => a.IsDeleted == false && a.Id == albumId)
            .FirstOrDefaultAsync();

        if (albumCheck != null)
        {
            return RedirectToAction("Index", "Album");
        }

        Order order = new Order()
        {
            AlbumTitle = albumCheck.Title,
            ImageUrl = albumCheck.ImageUrl!,
            OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Quantity = 1,
            UnitPrice = albumCheck.Price,
            BuyerId = buyerId!
        };

        BuyerAlbum buyerAlbum = new BuyerAlbum()
        {
            BuyerId = buyerId ?? string.Empty,
            AlbumId = albumId,
            OrderId = order.Id
        };

        await _context.Orders.AddAsync(order);
        await _context.BuyersAlbums.AddAsync(buyerAlbum!);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(OrderCartViewModel order, Guid albumId)
    {
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        BuyerAlbum? buyerAlbum = await _context.BuyersAlbums
            .Where(ba => ba.BuyerId == buyerId && ba.AlbumId == albumId &&
            ba.OrderId == order.Id)
            .FirstOrDefaultAsync();

        if (buyerAlbum == null)
        {
            return RedirectToAction("Index", "Album");
        }

        _context.BuyersAlbums.Remove(buyerAlbum);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Cart));
    }
}