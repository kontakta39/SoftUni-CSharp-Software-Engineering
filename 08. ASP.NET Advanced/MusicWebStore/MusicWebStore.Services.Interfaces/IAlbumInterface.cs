using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IAlbumInterface
{
    Task<List<AlbumIndexViewModel>> Index();
    Task<AlbumAddViewModel> Add();
    Task Add(AlbumAddViewModel addAlbum, string tempFolderPath, string finalFolderPath, string tempImageUrl);
    Task<AlbumDetailsViewModel> Details(Guid id);
    Task<AlbumEditViewModel> Edit(Guid id);
    Task Edit(AlbumEditViewModel editAlbum, Guid id, string tempFolderPath, string finalFolderPath, string tempImageUrl, string role);
    Task<AlbumDeleteViewModel> Delete(Guid id);
    Task Delete(AlbumDeleteViewModel deleteAlbum);
}