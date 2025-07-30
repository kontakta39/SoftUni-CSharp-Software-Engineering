using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBaseRepository _baseRepository;

    public BlogService(IBlogRepository blogRepository, IBaseRepository baseRepository)
    {
        _blogRepository = blogRepository;
        _baseRepository = baseRepository;
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _blogRepository.GetAllBlogsAsync();
    }

    public async Task<Blog?> GetBlogByIdAsync(Guid id)
    {
        return await _blogRepository.GetBlogByIdAsync(id);
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

        await _baseRepository.AddAsync(blog);
        await _baseRepository.SaveChangesAsync();
    }

    public async Task EditBlogAsync(BlogEditViewModel editBlog, Blog blog)
    {
        blog.Title = editBlog.Title;
        blog.ImageUrl = editBlog.ImageUrl;
        blog.Content = editBlog.Content;

        await _baseRepository.SaveChangesAsync();
    }

    public async Task DeleteBlogAsync(Blog blog)
    {
        blog.IsDeleted = true;
        await _baseRepository.SaveChangesAsync();
    }
}