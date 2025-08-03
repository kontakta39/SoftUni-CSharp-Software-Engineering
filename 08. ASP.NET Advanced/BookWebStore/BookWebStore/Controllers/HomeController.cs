using System.Diagnostics;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    //Action for custom 404 page
    public new IActionResult NotFound()
    {
        return View("404");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(bool isAdmin = false)
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}