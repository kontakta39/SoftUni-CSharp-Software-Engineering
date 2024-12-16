using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

[Authorize]
public class CompletedOrderController : Controller
{
    private readonly ICompletedOrderInterface _completedOrderService;
    private readonly MusicStoreDbContext _context;

    public CompletedOrderController(ICompletedOrderInterface completedOrderService, MusicStoreDbContext context)
    {
        _completedOrderService = completedOrderService;
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
        List<CompletedOrderViewModel> completedOrders = await _completedOrderService.OrdersList(buyerId);
        return View(completedOrders);
    }

    [HttpPost]
    public async Task<IActionResult> CompleteOrder(Guid id)
    {
        string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (string.IsNullOrEmpty(buyerId))
        {
            return RedirectToAction("LogIn", "Account");
        }

        try
        {
            Order? orderToComplete = await _completedOrderService.CompleteOrder(id, buyerId);
            //Store the order number in TempData for use in the view
            TempData["OrderNumber"] = orderToComplete.OrderNumber;
            return RedirectToAction("OrderSuccess", "CompletedOrder");
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
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

        try
        {
            //Call the service to handle the return logic
            OrderAlbum? albumFromOrder = await _completedOrderService.ReturnAlbum(Id, albumId, buyerId);

            //Retrieve the album and order details to show in TempData
            Order? order = await _context.Orders
                .Where(o => o.Id == Id && o.BuyerId == buyerId && o.IsCompleted == true)
                .FirstOrDefaultAsync();

            if (order != null && albumFromOrder != null)
            {
                //Store the album title and order number in TempData for the return success page
                TempData["AlbumTitle"] = albumFromOrder.Album.Title;
                TempData["OrderNumber"] = order.OrderNumber;
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("ReturnSuccess", "CompletedOrder");
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        } 
    }

    [HttpGet]
    public IActionResult ReturnSuccess()
    {
        return View();
    }
}