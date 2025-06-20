using BookWebStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class BookController : Controller
{
    private readonly BookStoreDbContext _context;

    public BookController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
