using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Index()
    {
        List<BlogIndexViewModel> allBlogs = await _context.Blogs
           .Where(b => !b.IsDeleted)
           .Include(b => b.Publisher)
           .Select(b => new BlogIndexViewModel()
           {
               Id = b.Id,
               Title = b.Title,
               ImageUrl = b.ImageUrl,
               Publisher = $"{b.Publisher.FirstName} {b.Publisher.LastName}"

           })
           .ToListAsync();

        return View(allBlogs);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Add()
    {
        BlogAddViewModel addBlog = new BlogAddViewModel();
        return View(addBlog);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(BlogAddViewModel addBlog)
    {
        ApplicationUser? publisher = await _userManager.GetUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(addBlog);
        }

        Blog blog = new Blog()
        {
            Title = addBlog.Title,
            ImageUrl = addBlog.ImageUrl,
            PublisherId = publisher.Id,
            Content = addBlog.Content
        };

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Blog? blogCheck = await _context.Blogs
           .Include(b => b.Publisher)
           .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (blogCheck == null)
        {
            TempData["ErrorMessage"] = "Blog was not found.";
            return RedirectToAction("Index", "Blog");
        }

        BlogDetailsViewModel blog = new BlogDetailsViewModel()
        {
            Id = blogCheck.Id,
            Title = blogCheck.Title,
            Publisher = $"{blogCheck.Publisher.FirstName} {blogCheck.Publisher.LastName}",
            PublishDate = blogCheck.PublishDate,
            ImageUrl = blogCheck.ImageUrl,
            Content = blogCheck.Content
        };

        return View(blog);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        ApplicationUser? publisher = await _userManager.GetUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        Blog? blog = await _context.Blogs
             .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (blog == null)
        {
            return NotFound();
        }

        //Check if the user is the author or is it Master Admin
        bool isOwner = blog.PublisherId == publisher.Id;
        bool isMasterAdmin = publisher.Email == "kontakta39@mail.bg";

        if (!isOwner && !isMasterAdmin)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        BlogEditViewModel editBlog = new BlogEditViewModel()
        {
            Id = blog.Id,
            Title = blog.Title,
            ImageUrl = blog.ImageUrl,
            Content = blog.Content
        };

        return View(editBlog);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(BlogEditViewModel editBlog)
    {
        ApplicationUser? publisher = await _userManager.GetUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(editBlog);
        }

        Blog? blog = await _context.Blogs
             .FirstOrDefaultAsync(b => b.Id == editBlog.Id && !b.IsDeleted);

        if (blog == null)
        {
            return NotFound();
        }

        //Check if the user is the author or is it Master Admin
        bool isOwner = blog.PublisherId == publisher.Id;
        bool isMasterAdmin = publisher.Email == "kontakta39@mail.bg";

        if (!isOwner && !isMasterAdmin)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        blog.Title = editBlog.Title;
        blog.ImageUrl = editBlog.ImageUrl;
        blog.Content = editBlog.Content;

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ApplicationUser? publisher = await _userManager.GetUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        Blog? blog = await _context.Blogs
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (blog == null)
        {
            return NotFound();
        }

        //Check if the user is the author or is it Master Admin
        bool isOwner = blog.PublisherId == publisher.Id;
        bool isMasterAdmin = publisher.Email == "kontakta39@mail.bg";

        if (!isOwner && !isMasterAdmin)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        BlogDeleteViewModel deleteBlog = new BlogDeleteViewModel()
        {
            Id = blog.Id,
            Title = blog.Title,
            Publisher = $"{blog.Publisher.FirstName} {blog.Publisher.LastName}"
        };

        return View(deleteBlog);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(BlogDeleteViewModel deleteBlog)
    {
        ApplicationUser? publisher = await _userManager.GetUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        Blog? blog = await _context.Blogs
            .FirstOrDefaultAsync(b => b.Id == deleteBlog.Id && !b.IsDeleted);

        if (blog == null)
        {
            return NotFound();
        }

        //Check if the user is the author or is it Master Admin
        bool isOwner = blog.PublisherId == publisher.Id;
        bool isMasterAdmin = publisher.Email == "kontakta39@mail.bg";

        if (!isOwner && !isMasterAdmin)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        blog.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Blog");
    }
}