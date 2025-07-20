using BookWebStore.Data.Models;
using BookWebStore.ViewModels;

namespace BookWebStore.Services.Interfaces;

public interface IAuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();

    Task<Author?> GetAuthorByIdAsync(Guid id);

    Task<bool> AuthorNameExistsAsync(string name, Guid? id = null);

    Task AddAuthorAsync(AuthorAddViewModel addAuthor);

    Task EditAuthorAsync(AuthorEditViewModel editAuthor, Author author);

    Task DeleteAuthorAsync(AuthorDeleteViewModel deleteAuthor, Author author);
}