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

    public IActionResult Cause500Error()
    {
        //Simulate a 500 error by throwing an exception
        throw new Exception("This is a test 500 error");
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