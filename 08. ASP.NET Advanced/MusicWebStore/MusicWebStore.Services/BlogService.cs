using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class BlogService : IBlogInterface
{
    private readonly MusicStoreDbContext _context;
    private readonly ImageHandler _imageHandler;

    public BlogService(MusicStoreDbContext context, ImageHandler imageHandler)
    {
        _context = context;
        _imageHandler = imageHandler;
    }

    public async Task<List<BlogIndexViewModel>> Index()
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

        return allBlogs;
    }

    public async Task Add(BlogAddViewModel addBlog, string publisherId, string tempFolderPath, string finalFolderPath, string tempImageUrl)
    {
        Blog blog = new Blog()
        {
            Title = addBlog.Title,
            PublisherId = publisherId,
            Content = addBlog.Content
        };

        //Handle image upload
        if (addBlog.ImageFile != null)
        {
            string fileName = await _imageHandler.SaveFinalImageAsync(addBlog.ImageFile);
            blog.ImageUrl = fileName;
        }
        else if (tempImageUrl != null)
        {
            string tempFileName = tempImageUrl;
            string finalFileName = ImageHandler.MoveImageToFinalFolder(tempFileName, tempFolderPath, finalFolderPath);

            if (finalFileName != null)
            {
                blog.ImageUrl = finalFileName;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
    }

    public async Task<BlogDetailsViewModel> Details(Guid id)
    {
        Blog? findBlog = await _context.Blogs
           .Where(b => b.Id == id && b.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (findBlog == null)
        {
            throw new ArgumentNullException();
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

        return blog;
    }

    public async Task<BlogEditViewModel> Edit(Guid id, string publisherId)
    {
        Blog? findBlog = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (findBlog == null)
        {
            throw new ArgumentNullException();
        }

        BlogEditViewModel editBlog = new BlogEditViewModel()
        {
            Title = findBlog.Title,
            ImageUrl = findBlog.ImageUrl,
            Content = findBlog.Content
        };

        return editBlog;
    }

    public async Task Edit(BlogEditViewModel editBlog,Guid id, string publisherId, string tempFolderPath, string finalFolderPath, string tempImageUrl)
    {
        Blog? blog = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (blog == null)
        {
            throw new ArgumentNullException();
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
        else if (tempImageUrl != null && blog.ImageUrl != null)
        {
            string fileName = tempImageUrl;
            string currentTempFilePath = Path.Combine(Directory.GetCurrentDirectory(), tempFolderPath, fileName);

            if (File.Exists(currentTempFilePath))
            {
                // Use ImageHandler to move the image from temp to final folder
                ImageHandler.MoveImageToFinalFolder(fileName, tempFolderPath, finalFolderPath);
                blog.ImageUrl = fileName; // Save the file name in the database
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<BlogDeleteViewModel> Delete(Guid id, string publisherId)
    {
        BlogDeleteViewModel? deleteBlog = await _context.Blogs
            .Where(b => b.Id == id && b.PublisherId == publisherId && b.IsDeleted == false)
            .Select(b => new BlogDeleteViewModel()
            {
                Id = b.Id,
                Title = b.Title
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (deleteBlog == null)
        {
            throw new ArgumentNullException();
        }

        return deleteBlog;
    }

    public async Task Delete(BlogDeleteViewModel deleteBlog, string publisherId)
    {
        Blog? blog = await _context.Blogs
          .Where(b => b.Id == deleteBlog.Id && b.PublisherId == publisherId && b.IsDeleted == false)
          .FirstOrDefaultAsync();

        if (blog != null)
        {
            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
}