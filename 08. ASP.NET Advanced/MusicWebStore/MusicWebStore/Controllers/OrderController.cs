using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicWebStore.Data;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

public class OrderController : Controller
{
    private readonly IOrderInterface _orderService;

    public OrderController(IOrderInterface orderService)
    {
        _orderService = orderService;
    }

	[HttpGet]
	public async Task<IActionResult> Cart()
	{
		string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (string.IsNullOrEmpty(buyerId))
        {
            return NotFound();
        }

        List<OrderCartViewModel>? model = await _orderService.Cart(buyerId);
		return View(model);
	}

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToCart(Guid id)
    {
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (buyerId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        try
        {
            await _orderService.AddToCart(id, buyerId);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateQuantity(Guid id, Guid albumId, int quantity)
    {
        //Call the service to update the quantity
        var result = await _orderService.UpdateQuantity(id, albumId, quantity);

        //If the operation failed, return a JSON error response
        if (!result.Item1) //Use Item1 for Success
        {
            return Json(new
            {
                success = false,
                error = result.Item2 //Use Item2 for Error message
            });
        }

        //If successful, return a JSON success response with updated details
        return Json(new
        {
            success = true,
            updatedPrice = result.Item3.ToString("F2"), //Use Item3 for UpdatedPrice
            remainingStock = result.Item4 //Use Item4 for RemainingStock
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart(Guid id, Guid albumId)
    {
        string? buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (buyerId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        try
        {
            await _orderService.RemoveFromCart(id, albumId, buyerId);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Cart));
    }
}