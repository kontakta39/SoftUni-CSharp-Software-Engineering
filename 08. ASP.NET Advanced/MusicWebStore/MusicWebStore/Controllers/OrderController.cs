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
		string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (buyerId == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        List<OrderCartViewModel>? model = await _context.OrdersAlbums
			.Where(oa => oa.Order.IsCompleted == false && oa.Order.BuyerId == buyerId)
			.Select(oa => new OrderCartViewModel()
			{
				Id = oa.OrderId,
				AlbumId = oa.AlbumId,
				ImageUrl = oa.Album.ImageUrl!,
				AlbumTitle = oa.Album.Title,
				Quantity = oa.Quantity,
				UnitPrice = oa.Price
            })
			.AsNoTracking()
			.ToListAsync();

		return View(model);
	}

    [HttpPost]
    public async Task<IActionResult> AddToCart(Guid id)
    {
        int quantity = 1;
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (buyerId == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        Order? order = await _context.Orders
            .Where(o => o.BuyerId == buyerId && o.IsCompleted == false)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            //Creating the order number
            Random random = new Random();

            //Create a list of numbers from 1 to 9
            List<int> digits = Enumerable.Range(1, 9).ToList();
            List<int> shuffledDigits = digits.OrderBy(x => random.Next()).ToList();

            // Concatenate the digits into a single number
            string randomNumber = "#" + string.Join("", shuffledDigits);

            order = new Order()
            {
                BuyerId = buyerId,
                OrderNumber = randomNumber,
                OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
                TotalQuantity = 0,
                TotalPrice = 0
            };

            _context.Orders.Add(order);
        }
        else
        {
            return RedirectToAction(nameof(Cart));
        }

        Album? album = await _context.Albums
            .Where(a => a.Id == id && a.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return RedirectToAction("Index", "Album");
        }

        OrderAlbum? existingOrderAlbum = await _context.OrdersAlbums
            .Where(oa => oa.OrderId == order.Id && oa.AlbumId == id)
            .FirstOrDefaultAsync();

        if (existingOrderAlbum != null)
        {
            return RedirectToAction("Index", "Album");
        }
        else
        {
            order.OrdersAlbums.Add(new OrderAlbum
            {
                OrderId = order.Id,
                AlbumId = id,
                Quantity = quantity,
                Price = quantity * album.Price
            });

            order.TotalQuantity += quantity;
            order.TotalPrice += quantity * album.Price;

            album.Stock -= quantity;

            if (album.Stock < 1)
            { 
                album.IsDeleted = true;
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(Guid id, Guid albumId, int quantity)
    {
        if (quantity < 1)
        {
            return Json(new { success = false, error = "Quantity must be at least 1." });
        }

        Order? order = await _context.Orders
            .Include(o => o.OrdersAlbums)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return RedirectToAction(nameof(Cart));
        }

        OrderAlbum? orderAlbum = order.OrdersAlbums.FirstOrDefault(oa => oa.AlbumId == albumId);

        if (orderAlbum == null)
        {
            return RedirectToAction(nameof(Cart));
        }

        Album? album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId && a.IsDeleted == false);
        
        if (album == null)
        {
            return RedirectToAction(nameof(Cart));
        }

        if (quantity > album.Stock + orderAlbum.Quantity)
        {
            return Json(new { success = false, error = $"Only {album.Stock + orderAlbum.Quantity} units are available." });
        }

        //Change quantity and totals.
        int quantityChange = quantity - orderAlbum.Quantity;
        orderAlbum.Quantity = quantity;
        orderAlbum.Price = quantity * album.Price;
        order.TotalQuantity += quantityChange;
        order.TotalPrice += quantityChange * album.Price;

        //Update the available albums.
        album.Stock -= quantityChange;

        if (album.Stock < 1)
        { 
           album.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
        return Json(new
        {
            success = true,
            updatedPrice = orderAlbum.Price.ToString("F2"),
            remainingStock = album.Stock
        });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(Guid id, Guid albumId)
    {
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (buyerId == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        OrderAlbum? orderAlbumToBeDeleted = await _context.OrdersAlbums
            .Where(oa => oa.OrderId == id && oa.AlbumId == albumId)
            .FirstOrDefaultAsync();

        if (orderAlbumToBeDeleted == null)
        {
            return RedirectToAction("Index", "Album");
        }

        //Returning back the quantities to the album stock /without isDeleted because album stock may be 0/
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return RedirectToAction("Index", "Album");
        }

        //Changing the total quantity and the price of the order
        Order? currentOrder = await _context.Orders
            .Where(o => o.Id == id && o.BuyerId == buyerId &&
                        o.IsCompleted == false)
            .FirstOrDefaultAsync();

        if (currentOrder == null)
        {
            return RedirectToAction(nameof(Cart));
        }

        currentOrder.TotalQuantity -= orderAlbumToBeDeleted.Quantity;
        currentOrder.TotalPrice -= orderAlbumToBeDeleted.Price;
        album.Stock += orderAlbumToBeDeleted.Quantity;

        if (album.Stock >= 1)
        {
            album.IsDeleted = false;
        }

        _context.OrdersAlbums.Remove(orderAlbumToBeDeleted);

        if (currentOrder.TotalQuantity == 0 && currentOrder.TotalPrice == 0)
        {
            _context.Orders.Remove(currentOrder);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Cart));
    }
}