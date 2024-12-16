using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IBlogInterface
{
    Task<List<BlogIndexViewModel>> Index();
    Task Add(BlogAddViewModel addBlog, string publisherId, string tempFolderPath, string finalFolderPath, string tempImageUrl);
    Task<BlogDetailsViewModel> Details(Guid id);
    Task<BlogEditViewModel> Edit(Guid id, string publisherId);
    Task Edit(BlogEditViewModel editBlog, Guid id, string publisherId, string tempFolderPath, string finalFolderPath, string tempImageUrl);
    Task<BlogDeleteViewModel> Delete(Guid id, string publisherId);
    Task Delete(BlogDeleteViewModel deleteBlog, string publisherId);
}