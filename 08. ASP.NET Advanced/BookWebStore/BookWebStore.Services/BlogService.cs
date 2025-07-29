using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;

    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _blogRepository.GetAllAsync();
    }

    public async Task<Blog?> GetBlogByIdAsync(Guid id)
    {
        return await _blogRepository.GetByIdAsync(id);
    }

    public async Task AddBlogAsync(BlogAddViewModel addBlog, string publisherId)
    {
        Blog blog = new Blog()
        {
            Title = addBlog.Title,
            ImageUrl = addBlog.ImageUrl,
            PublisherId = publisherId,
            Content = addBlog.Content
        };

        await _blogRepository.AddAsync(blog);
        await _blogRepository.SaveChangesAsync();
    }

    public async Task EditBlogAsync(BlogEditViewModel editBlog, Blog blog)
    {
        blog.Title = editBlog.Title;
        blog.ImageUrl = editBlog.ImageUrl;
        blog.Content = editBlog.Content;

        await _blogRepository.SaveChangesAsync();
    }

    public async Task DeleteBlogAsync(Blog blog)
    {
        blog.IsDeleted = true;
        await _blogRepository.SaveChangesAsync();
    }
}