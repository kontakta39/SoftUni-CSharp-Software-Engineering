using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IArtistInterface
{
    Task<List<ArtistIndexViewModel>> Index();
    Task<ArtistAddViewModel> Add();
    Task Add(ArtistAddViewModel addArtist, string tempImageUrl);
    Task<ArtistDetailsViewModel> Details(Guid id);
    Task<ArtistEditViewModel> Edit(Guid id);
    Task Edit(ArtistEditViewModel editArtist, Guid id, string newImageUrl);
    Task<ArtistDeleteViewModel> Delete(Guid id);
    Task Delete(ArtistDeleteViewModel deleteArtist);
}