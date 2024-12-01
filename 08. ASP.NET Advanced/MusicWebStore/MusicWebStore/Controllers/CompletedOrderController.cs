using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

[Authorize]
public class CompletedOrderController : Controller
{
    private readonly MusicStoreDbContext _context;

    public CompletedOrderController(MusicStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (buyerId == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        //Fetch all completed orders by a certain buyer
        List<CompletedOrderViewModel> completedOrders = await _context.Orders
        .Where(or => or.BuyerId == buyerId && or.IsCompleted == true)
        .Select(or => new CompletedOrderViewModel
        {
            OrderNumber = or.OrderNumber,
            OrderDate = or.OrderDate,
            TotalPrice = or.TotalPrice,
            OrderedAlbums = or.OrdersAlbums
                .Select(oa => new OrderedAlbumViewModel()
                {
                    AlbumId = oa.AlbumId,
                    AlbumImageUrl = oa.Album.ImageUrl!,
                    AlbumTitle = oa.Album.Title,
                    AlbumQuantity = oa.Quantity,
                    AlbumPrice = oa.Price
                })
                .ToList()
        })
        .ToListAsync();

        return View(completedOrders);
    }

    [HttpPost]
    public async Task<IActionResult> CompleteOrder(Guid id)
    {
        string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (buyerId == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        Order? orderToComplete = await _context.Orders
            .Where(or => or.Id == id && or.BuyerId == buyerId && or.IsCompleted == false)
            .FirstOrDefaultAsync();

        if (orderToComplete == null)
        {
            return RedirectToAction("Cart", "Order");
        }
        else
        {
            orderToComplete.IsCompleted = true;
        }
        
        return View(nameof(Index));
    }
}