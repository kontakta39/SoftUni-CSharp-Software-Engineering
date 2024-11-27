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
			.Where(o => o.IsCompleted == false && o.BuyerId == getCurrentUserId)
			.Select(o => new OrderCartViewModel()
			{
				Id = o.Id,
				AlbumId = _context.BuyersAlbums
					.Where(ba => ba.OrderId == o.Id)
					.Select(ba => ba.AlbumId)
					.FirstOrDefault(), // Fetch the AlbumId related to the Order
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
    public async Task<IActionResult> AddToCart(Guid id)
    {
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Album? albumCheck = await _context.Albums
            .Where(a => a.IsDeleted == false && a.Id == id)
            .FirstOrDefaultAsync();

        if (albumCheck == null)
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
            AlbumId = id,
            OrderId = order.Id
        };

        await _context.Orders.AddAsync(order);
        await _context.BuyersAlbums.AddAsync(buyerAlbum!);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(Guid id, Guid albumId, int quantity)
    {
        // Validate the incoming quantity
        if (quantity < 1)
        {
            return Json(new { success = false, error = "Quantity must be at least 1." });
        }

        // Fetch the order and album from the database
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id && o.BuyerAlbums.Any(ba => ba.AlbumId == albumId));

        if (order == null)
        {
            return Json(new { success = false, error = "Order not found." });
        }

        var album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId && !a.IsDeleted);

        if (album == null)
        {
            return Json(new { success = false, error = "Album not found or unavailable." });
        }

        // Check stock availability
        if (quantity > album.Stock)
        {
            return Json(new { success = false, error = $"Only {album.Stock} units of this album are available." });
        }

        // Update the quantity and recalculate the total price
        order.Quantity = quantity;
        order.UnitPrice = album.Price * quantity;

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            updatedPrice = order.UnitPrice.ToString("F2"),
            remainingStock = album.Stock
        });
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