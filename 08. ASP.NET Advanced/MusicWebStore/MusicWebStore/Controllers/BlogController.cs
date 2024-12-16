using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

public class BlogController : Controller
{
    private readonly IBlogInterface _blogService;
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public BlogController(IBlogInterface blogService, MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _blogService = blogService;
        _context = context;
        _imageHandler = imageHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<BlogIndexViewModel> allBlogs = await _blogService.Index();
        return View(allBlogs);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Add()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        BlogAddViewModel addBlog = new BlogAddViewModel(); 
        return View(addBlog);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(BlogAddViewModel addBlog)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        //Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Blogs Images");

        if (!ModelState.IsValid)
        {
            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (addBlog.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(addBlog.ImageFile);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    string tempFileName = await ImageHandler.SaveTempImageAsync(addBlog.ImageFile, tempFolderPath);
                    addBlog.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; //Store temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addBlog.ImageUrl = TempData["ImageUrl"].ToString(); //Retrieve the file name
            }

            TempData.Keep("ImageUrl");
            return View(addBlog);
        }

        try
        {
            string tempImageUrl = TempData["ImageUrl"].ToString();
            await _blogService.Add(addBlog, publisherId, tempFolderPath, finalFolderPath, tempImageUrl);
            TempData.Remove("ImageUrl");
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        BlogDetailsViewModel blog = await _blogService.Details(id);

        if (blog == null)
        {
            return NotFound();
        }

        return View(blog);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!User.IsInRole("Administrator") && !User.IsInRole("Moderator"))
        {
            return RedirectToAction("AccessDenied", "Home"); 
        }

        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        BlogEditViewModel editBlog = await _blogService.Edit(id, publisherId);

        if (editBlog == null)
        {
            return NotFound();
        }

        return View(editBlog);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(BlogEditViewModel editBlog, Guid id)
    {
        if (!User.IsInRole("Administrator") && !User.IsInRole("Moderator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        Blog? blogCheck = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        TempData["CurrentImageUrl"] = blogCheck.ImageUrl;

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Blogs Images");

        if (!ModelState.IsValid)
        {
            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editBlog.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(editBlog.ImageFile);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    if (blogCheck.ImageUrl != null)
                    {
                        // Use ImageHandler to delete the old image
                        _imageHandler.DeleteImage(blogCheck.ImageUrl, finalFolderPath);
                    }

                    // Save the file temporarily using ImageHandler
                    string tempFileName = await ImageHandler.SaveTempImageAsync(editBlog.ImageFile, tempFolderPath);
                    editBlog.ImageUrl = tempFileName;
                    TempData["NewImageUrl"] = tempFileName; // Store the file name temporarily
                }
            }
            else if (TempData["NewImageUrl"] != null)
            {
                editBlog.ImageUrl = TempData["NewImageUrl"].ToString(); // Retrieve the file name from TempData
                TempData.Keep("NewImageUrl"); // Preserve TempData for subsequent requests
            }
            else
            {
                editBlog.ImageUrl = TempData["CurrentImageUrl"].ToString(); // Retrieve the file name from TempData
            }

            return View(editBlog);
        }

        try
        {
            string tempImageUrl = TempData["NewImageUrl"].ToString();
            await _blogService.Edit(editBlog, id, publisherId, tempFolderPath, finalFolderPath, tempImageUrl);
            TempData.Remove("CurrentImageUrl");
            TempData.Remove("NewImageUrl");
            return RedirectToAction("Details", "Blog", new { id = blogCheck.Id });
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        try
        {
            BlogDeleteViewModel? deleteBlog = await _blogService.Delete(id, publisherId);
            return View(deleteBlog);
        }
        catch (ArgumentNullException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(BlogDeleteViewModel deleteBlog)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        await _blogService.Delete(deleteBlog, publisherId);
        return RedirectToAction(nameof(Index));
    }
}