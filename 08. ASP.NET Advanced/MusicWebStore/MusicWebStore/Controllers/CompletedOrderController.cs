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
    public async Task<IActionResult> OrdersList()
    {
        string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (buyerId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        //Fetch all completed orders by a certain buyer
        List<CompletedOrderViewModel> completedOrders = await _context.Orders
        .Where(or => or.BuyerId == buyerId && or.IsCompleted == true)
        .Select(or => new CompletedOrderViewModel
        {
            Id = or.Id,
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
                    AlbumPrice = oa.Price,
                    isReturned = oa.isReturned
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
            return RedirectToAction("LogIn", "Account");
        }

        Order? orderToComplete = await _context.Orders
            .Where(or => or.Id == id && or.BuyerId == buyerId && or.IsCompleted == false)
            .FirstOrDefaultAsync();

        if (orderToComplete == null)
        {
            return NotFound();
        }
        else
        {
            orderToComplete.IsCompleted = true;
        }

        // Store the order number in TempData
        TempData["OrderNumber"] = orderToComplete.OrderNumber;

        await _context.SaveChangesAsync();

        return RedirectToAction("OrderSuccess", "CompletedOrder");
    }

    [HttpGet]
    public IActionResult OrderSuccess()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ReturnAlbum(Guid Id, Guid albumId)
    {
        string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (buyerId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        //Find the order from which the album should be returned
        Order? orderCheck = await _context.Orders
            .Where(or => or.Id == Id && or.BuyerId == buyerId && or.IsCompleted == true)
            .FirstOrDefaultAsync();

        if (orderCheck == null)
        {
            return NotFound();
        }

        //Take the current album from the order that should be returned
        OrderAlbum? albumFromTheOrder = await _context.OrdersAlbums
            .Where(a => a.OrderId == Id && a.AlbumId == albumId && a.isReturned == false)
            .FirstOrDefaultAsync();

         if (albumFromTheOrder == null)
        {
            return NotFound();
        }

        //Check if the current album is in stock
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return NotFound();
        }

        album.Stock += albumFromTheOrder.Quantity;
        album.IsDeleted = false;
        
        albumFromTheOrder.isReturned = true;

        // Store the album title and the order number in TempData
        TempData["AlbumTitle"] = albumFromTheOrder.Album.Title;
        TempData["OrderNumber"] = orderCheck.OrderNumber;

        await _context.SaveChangesAsync();

        return RedirectToAction("ReturnSuccess", "CompletedOrder");
    }

    [HttpGet]
    public IActionResult ReturnSuccess()
    {
        return View();
    }
}