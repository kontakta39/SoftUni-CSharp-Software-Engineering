using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class AuthorService : IAuthorService
{
    private readonly BookStoreDbContext _context;

    public AuthorService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .ToListAsync();
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<bool> AuthorNameExistsAsync(string name, Guid? id = null)
    {
        return await _context.Authors
            .AnyAsync(a => a.Name.ToLower() == name.ToLower() &&
            (id == null || a.Id != id) && !a.IsDeleted);
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

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task EditAuthorAsync(AuthorEditViewModel editAuthor, Author author)
    {
        author.Name = editAuthor.Name;
        author.Biography = editAuthor.Biography;
        author.Nationality = editAuthor.Nationality;
        author.ImageUrl = editAuthor.ImageUrl;
        author.BirthDate = editAuthor.ParsedBirthDate;
        author.Website = editAuthor.Website;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(AuthorDeleteViewModel deleteAuthor, Author author)
    {
        author.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}