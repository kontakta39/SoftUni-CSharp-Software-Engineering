using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class BlogService : IBlogService
{
    private readonly BookStoreDbContext _context;

    public BlogService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _context.Blogs
           .Include(b => b.Publisher)
           .Where(b => !b.IsDeleted)
           .ToListAsync();
    }

    public async Task<Blog?> GetBlogByIdAsync(Guid id)
    {
        return await _context.Blogs
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task AddBlogAsync(BlogAddViewModel addBlog, ApplicationUser publisher)
    {
        Blog blog = new Blog()
        {
            Title = addBlog.Title,
            ImageUrl = addBlog.ImageUrl,
            PublisherId = publisher.Id,
            Content = addBlog.Content
        };

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
    }

    public async Task EditBlogAsync(BlogEditViewModel editBlog, Blog blog)
    {
        blog.Title = editBlog.Title;
        blog.ImageUrl = editBlog.ImageUrl;
        blog.Content = editBlog.Content;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBlogAsync(Blog blog)
    {
        blog.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}