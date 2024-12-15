using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IGenreService
{
    Task<List<GenreIndexViewModel>> Index();
    Task Add(GenreAddViewModel addGenre);
    Task<GenreEditViewModel> Edit(Guid id);
    Task Edit(GenreEditViewModel editModel, Guid id);  
    Task<GenreDeleteViewModel> Delete(Guid id);  
    Task Delete(GenreDeleteViewModel model);  
}