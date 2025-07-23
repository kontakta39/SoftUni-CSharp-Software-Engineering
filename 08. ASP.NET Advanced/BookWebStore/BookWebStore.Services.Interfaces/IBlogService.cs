using BookWebStore.Data.Models;
using BookWebStore.ViewModels;

namespace BookWebStore.Services.Interfaces;

public interface IBlogService
{
    Task<List<Blog>> GetAllBlogsAsync();

    Task<Blog?> GetBlogByIdAsync(Guid id);

    Task AddBlogAsync(BlogAddViewModel addBlog, ApplicationUser publisher);

    Task EditBlogAsync(BlogEditViewModel editBlog, Blog blog);

    Task DeleteBlogAsync(Blog blog);
}