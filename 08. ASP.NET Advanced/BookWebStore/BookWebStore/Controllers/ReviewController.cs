using BookWebStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class ReviewController : Controller
{
    private readonly BookStoreDbContext _context;

    public ReviewController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}