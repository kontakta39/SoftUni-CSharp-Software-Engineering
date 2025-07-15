using BookWebStore.Data.Models;
using BookWebStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class BlogController : Controller
{
    private readonly BookStoreDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BlogController(BookStoreDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}