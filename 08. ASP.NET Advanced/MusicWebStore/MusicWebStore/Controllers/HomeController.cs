using Microsoft.AspNetCore.Mvc;
using MusicWebStore.ViewModels;
using System.Diagnostics;

namespace MusicWebStore.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    //Action for custom 404 page
    public IActionResult NotFound()
    {
        return View("404");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        int statusCode = HttpContext.Response.StatusCode;

        if (statusCode == 500)
        {
            return View("500"); 
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}