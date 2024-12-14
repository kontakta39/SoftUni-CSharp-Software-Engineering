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
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public BlogController(MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _context = context;
        _imageHandler = imageHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<BlogIndexViewModel> allBlogs = await _context.Blogs
            .Where(b => b.IsDeleted == false)
            .Select(b => new BlogIndexViewModel() 
            {
                Id = b.Id,
                Title = b.Title,
                PublisherName = $"{b.Publisher.FirstName} {b.Publisher.LastName}",
                PublishDate = b.PublishDate,
                ImageUrl = b.ImageUrl
            })
            .ToListAsync();

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
    public async Task<IActionResult> AddAsync(BlogAddViewModel addBlog)
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

        // Define allowed content types and extensions
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Blogs Images");

        if (!ModelState.IsValid)
        {
            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (addBlog.ImageFile != null)
            {
                string validationError = ImageHandler.ValidateImage(addBlog.ImageFile, allowedContentTypes, allowedExtensions);

                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("ImageFile", validationError);
                }
                else
                {
                    string tempFileName = await ImageHandler.SaveTempImageAsync(addBlog.ImageFile, tempFolderPath);
                    addBlog.ImageUrl = tempFileName;
                    TempData["ImageUrl"] = tempFileName; // Store temporarily
                }
            }
            else if (TempData["ImageUrl"] != null)
            {
                addBlog.ImageUrl = TempData["ImageUrl"].ToString(); // Retrieve the file name
            }

            TempData.Keep("ImageUrl");

            return View(addBlog);
        }

        Blog blog = new Blog()
        {
            Title = addBlog.Title,
            PublisherId = publisherId,
            Content = addBlog.Content
        };

        // Handle image upload
        if (addBlog.ImageFile != null)
        {
            string fileName = await _imageHandler.SaveFinalImageAsync(addBlog.ImageFile);
            blog.ImageUrl = fileName;
        }
        else if (TempData["ImageUrl"] != null)
        {
            string tempFileName = TempData["ImageUrl"].ToString();
            string finalFileName = ImageHandler.MoveImageToFinalFolder(tempFileName, tempFolderPath, finalFolderPath);

            if (finalFileName != null)
            {
                blog.ImageUrl = finalFileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "The temporary file is missing. Please re-upload the image.");
                return View(addBlog);
            }
        }

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        // Clean up TempData
        TempData.Remove("ImageUrl");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Blog? findBlog = await _context.Blogs
            .Where(b => b.Id == id && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (findBlog == null)
        { 
            return NotFound();
        }

        BlogDetailsViewModel blog = new BlogDetailsViewModel()
        {
            Id = findBlog.Id,
            Title = findBlog.Title,
            PublisherName = $"{findBlog.Publisher.FirstName} {findBlog.Publisher.LastName}",
            PublishDate = findBlog.PublishDate,
            ImageUrl = findBlog.ImageUrl,
            Content = findBlog.Content
        };

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

        Blog? findBlog = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (findBlog == null)
        { 
            return NotFound();
        }

        BlogEditViewModel editBlog = new BlogEditViewModel()
        {
            Title = findBlog.Title,
            ImageUrl = findBlog.ImageUrl,
            Content = findBlog.Content
        };

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

        TempData["CurrentImageUrl"] = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .Select(b => b.ImageUrl)
            .FirstOrDefaultAsync();

        // Validate the uploaded image using ImageHandler
        string[] allowedContentTypes = { "image/jpg", "image/jpeg", "image/png", "image/gif", "image/webp" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Ensure that the ImageHandler is properly initialized
        string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Temp");
        string finalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Blogs Images");

        if (!ModelState.IsValid)
        {
            //Handle image upload (only store the filename in TempData if ModelState is not valid)
            if (editBlog.ImageFile != null)
            {
                // Delete the old image if it's not the default one
                Blog? blogCheck = _context.Blogs
                    .FirstOrDefault(b => b.Id == id && b.IsDeleted == false);

                string validationError = ImageHandler.ValidateImage(editBlog.ImageFile, allowedContentTypes, allowedExtensions);

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

        Blog? blog = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (blog == null)
        {
            return NotFound();
        }

        blog.Title = editBlog.Title;
        blog.ImageUrl = editBlog.ImageUrl;
        blog.Content = editBlog.Content;

        //Handle image upload (copy the file to server only if ModelState is valid)
        if (editBlog.ImageFile != null)
        {
            // Delete the old image if it's not the default one
            if (blog.ImageUrl != null)
            {
                _imageHandler.DeleteImage(blog.ImageUrl, finalFolderPath);
            }

            // Save the file to the final folder using ImageHandler
            string fileName = await _imageHandler.SaveFinalImageAsync(editBlog.ImageFile);
            blog.ImageUrl = fileName; // Save the file name in the database
        }
        else if (TempData["NewImageUrl"] != null && blog.ImageUrl != null)
        {
            string fileName = TempData["NewImageUrl"].ToString();
            string currentTempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderPath, fileName);

            if (System.IO.File.Exists(currentTempFilePath))
            {
                // Use ImageHandler to move the image from temp to final folder
                ImageHandler.MoveImageToFinalFolder(fileName, tempFolderPath, finalFolderPath);
                blog.ImageUrl = fileName; // Save the file name in the database
            }
            else
            {
                ModelState.AddModelError("ImageFile", "The temporary file is missing. Please re-upload the image.");
                return View(editBlog);
            }
        }

        await _context.SaveChangesAsync();

        // Clean up TempData
        TempData.Remove("CurrentImageUrl");
        TempData.Remove("NewImageUrl");

        return RedirectToAction("Details", "Blog", new { id = blog.Id });
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

        BlogDeleteViewModel? deleteBlog = await _context.Blogs
            .Where(b => b.Id == id && b.IsDeleted == false)
            .Select(b => new BlogDeleteViewModel()
            {
                Id = b.Id,
                Title = b.Title
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (deleteBlog == null)
        {
            return NotFound();
        }

        return View(deleteBlog);
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

        Blog? blog = await _context.Blogs
           .Where(b => b.Id == deleteBlog.Id && b.PublisherId == publisherId && b.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (blog != null)
        {
            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}