using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

public class BlogController : Controller
{
    private readonly MusicStoreDbContext _context;

    public BlogController(MusicStoreDbContext context)
    {
        _context = context;
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
                ImageUrl = b.ImageUrl
            })
            .ToListAsync();

        return View(allBlogs);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Moderator")]
    public IActionResult Add()
    {
        BlogAddViewModel addBlog = new BlogAddViewModel(); 
        return View(addBlog);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> AddAsync(BlogAddViewModel addBlog)
    {
        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        if (!ModelState.IsValid)
        { 
            return View(addBlog);
        }

        Blog blog = new Blog()
        {
            Title = addBlog.Title,
            ImageUrl = addBlog.ImageUrl,
            PublisherId = publisherId,
            Content = addBlog.Content
        };

        await _context.Blogs.AddAsync(blog);

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
            PublisherId = findBlog.PublisherId,
            PublisherName = $"{findBlog.Publisher.FirstName} {findBlog.Publisher.LastName}",
            PublishDate = findBlog.PublishDate,
            ImageUrl = findBlog.ImageUrl,
            Content = findBlog.Content
        };

        return View(blog);
    }


    [HttpGet]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(Guid id)
    {
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
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(BlogEditViewModel editModel, Guid id)
    {
        string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (publisherId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        if (!ModelState.IsValid)
        { 
            return View(editModel);
        }

        Blog? findBlog = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (findBlog == null)
        {
            return NotFound();
        }

        findBlog.Title = editModel.Title;
        findBlog.ImageUrl = editModel.ImageUrl;
        findBlog.Content = editModel.Content;

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Blog", new { id = findBlog.Id });
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Delete(Guid id)
    {
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
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Delete(BlogDeleteViewModel deleteBlog)
    {
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