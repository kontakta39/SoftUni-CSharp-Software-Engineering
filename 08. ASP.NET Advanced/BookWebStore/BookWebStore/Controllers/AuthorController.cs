using BookWebStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore;

public class AuthorController : Controller
{
    private readonly BookStoreDbContext _context;

    public AuthorController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
