namespace BookWebStore.Services.Interfaces;

public interface IBookService
{
    Task<bool> HasBooksInStockByGenreIdAsync(Guid genreId);
}