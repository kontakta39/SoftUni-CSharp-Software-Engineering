using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly IAccountService _accountService;

    public BlogController(IBlogService blogService, IAccountService accountService)
    {
        _blogService = blogService;
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<Blog> getAllBlogs = await _blogService.GetAllBlogsAsync();

        List<BlogIndexViewModel> blogs = getAllBlogs
           .Select(b => new BlogIndexViewModel()
           {
               Id = b.Id,
               Title = b.Title,
               ImageUrl = b.ImageUrl,
               Publisher = $"{b.Publisher.FirstName} {b.Publisher.LastName}"

           })
           .ToList();

        return View(blogs);
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
        ApplicationUser? publisher = await _accountService.GetCurrentUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(addBlog);
        }

        await _blogService.AddBlogAsync(addBlog, publisher);

        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Blog? blog = await _blogService.GetBlogByIdAsync(id);

        if (blog == null)
        {
            TempData["ErrorMessage"] = "Blog was not found.";
            return RedirectToAction("Index", "Blog");
        }

        BlogDetailsViewModel blogDetails = new BlogDetailsViewModel()
        {
            Id = blog.Id,
            Title = blog.Title,
            Publisher = $"{blog.Publisher.FirstName} {blog.Publisher.LastName}",
            PublishDate = blog.PublishDate,
            ImageUrl = blog.ImageUrl,
            Content = blog.Content
        };

        return View(blogDetails);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        ApplicationUser? publisher = await _accountService.GetCurrentUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        Blog? blog = await _blogService.GetBlogByIdAsync(id);

        if (blog == null)
        {
            TempData["ErrorMessage"] = "The blog you want to edit does not exist.";
            return RedirectToAction("Index", "Blog");
        }

        BlogEditViewModel editBlog = new BlogEditViewModel()
        {
            Id = blog.Id,
            Title = blog.Title,
            ImageUrl = blog.ImageUrl,
            PublisherId = blog.PublisherId,
            Content = blog.Content
        };

        return View(editBlog);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Moderator")]
    public async Task<IActionResult> Edit(BlogEditViewModel editBlog)
    {
        ApplicationUser? publisher = await _accountService.GetCurrentUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(editBlog);
        }

        Blog? blog = await _blogService.GetBlogByIdAsync(editBlog.Id);

        if (blog == null)
        {
            TempData["ErrorMessage"] = "The blog you want to edit does not exist.";
            return RedirectToAction("Index", "Blog");
        }

        await _blogService.EditBlogAsync(editBlog, blog);

        return RedirectToAction("Index", "Blog");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ApplicationUser? publisher = await _accountService.GetCurrentUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        Blog? blog = await _blogService.GetBlogByIdAsync(id);

        if (blog == null)
        {
            TempData["ErrorMessage"] = "The blog you want to delete does not exist.";
            return RedirectToAction("Index", "Blog");
        }

        //Check if the user is Master Admin
        bool isMasterAdmin = publisher.Email == "kontakta39@mail.bg";

        if (!isMasterAdmin)
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
        ApplicationUser? publisher = await _accountService.GetCurrentUserAsync(User);

        if (publisher == null)
        {
            return NotFound();
        }

        Blog? blog = await _blogService.GetBlogByIdAsync(deleteBlog.Id);

        if (blog == null)
        {
            TempData["ErrorMessage"] = "The blog you want to delete does not exist.";
            return RedirectToAction("Index", "Blog");
        }

        //Check if the user is Master Admin
        bool isMasterAdmin = publisher.Email == "kontakta39@mail.bg";

        if (!isMasterAdmin)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        await _blogService.DeleteBlogAsync(blog);

        return RedirectToAction("Index", "Blog");
    }
}