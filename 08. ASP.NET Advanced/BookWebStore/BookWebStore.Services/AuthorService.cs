using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _authorRepository.GetAllAsync();
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id)
    {
        return await _authorRepository.GetByIdAsync(id);
    }

    public async Task<bool> AuthorNameExistsAsync(string name, Guid? id = null)
    {
        return await _authorRepository.ExistsByNameAsync(name, id);
    }

    public async Task AddAuthorAsync(AuthorAddViewModel addAuthor)
    {
        Author author = new Author
        {
            Name = addAuthor.Name,
            Biography = addAuthor.Biography,
            Nationality = addAuthor.Nationality,
            ImageUrl = addAuthor.ImageUrl,
            BirthDate = addAuthor.ParsedBirthDate,
            Website = addAuthor.Website
        };

        await _authorRepository.AddAsync(author);
        await _authorRepository.SaveChangesAsync();
    }

    public async Task EditAuthorAsync(AuthorEditViewModel editAuthor, Author author)
    {
        author.Name = editAuthor.Name;
        author.Biography = editAuthor.Biography;
        author.Nationality = editAuthor.Nationality;
        author.ImageUrl = editAuthor.ImageUrl;
        author.BirthDate = editAuthor.ParsedBirthDate;
        author.Website = editAuthor.Website;

        await _authorRepository.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(AuthorDeleteViewModel deleteAuthor, Author author)
    {
        author.IsDeleted = true;
        await _authorRepository.SaveChangesAsync();
    }
}