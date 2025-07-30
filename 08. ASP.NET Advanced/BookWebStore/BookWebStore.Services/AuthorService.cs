using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;

namespace BookWebStore.Services;

public class AuthorService : IAuthorService
{
    private readonly IBaseRepository _baseRepository;

    public AuthorService(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _baseRepository.GetAllAsync<Author>();
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id)
    {
        return await _baseRepository.GetByIdAsync<Author>(id);
    }

    public async Task<bool> AuthorNameExistsAsync(string name, Guid? id = null)
    {
        return await _baseRepository.ExistsByPropertyAsync<Author>("Name", name, id);
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

        await _baseRepository.AddAsync(author);
        await _baseRepository.SaveChangesAsync();
    }

    public async Task EditAuthorAsync(AuthorEditViewModel editAuthor, Author author)
    {
        author.Name = editAuthor.Name;
        author.Biography = editAuthor.Biography;
        author.Nationality = editAuthor.Nationality;
        author.ImageUrl = editAuthor.ImageUrl;
        author.BirthDate = editAuthor.ParsedBirthDate;
        author.Website = editAuthor.Website;

        await _baseRepository.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(Author author)
    {
        author.IsDeleted = true;
        await _baseRepository.SaveChangesAsync();
    }
}