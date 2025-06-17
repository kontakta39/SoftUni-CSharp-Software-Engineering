using BookWebStore.Data;
using BookWebStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore;

public class GenreController : Controller
{
    private readonly BookStoreDbContext _context;

    public GenreController(BookStoreDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}