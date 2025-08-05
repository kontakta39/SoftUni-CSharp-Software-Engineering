using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.ViewModels;
using Moq;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class AuthorServiceTests
{
    private Mock<IBaseRepository> _mockRepository;
    private AuthorService _authorService;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IBaseRepository>();
        _authorService = new AuthorService(_mockRepository.Object);
    }

    [Test]
    public async Task GetAllAuthorsAsync_ReturnsListOfAuthors_WithRequiredPropertiesSet()
    {
        List<Author> authors = new List<Author>
        {
            new Author(),
            new Author()
        };

        _mockRepository
            .Setup(repo => repo.GetAllAsync<Author>())
            .ReturnsAsync(authors);

        List<Author> result = await _authorService.GetAllAuthorsAsync();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllAuthorsAsync_ReturnsEmptyList_WhenNoAuthors()
    {
        List<Author> emptyAuthors = new List<Author>();

        _mockRepository
            .Setup(repo => repo.GetAllAsync<Author>())
            .ReturnsAsync(emptyAuthors);

        List<Author> result = await _authorService.GetAllAuthorsAsync();

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task SearchAuthorsAsync_ReturnsMatchingAuthors_WhenTheyExist()
    {
        string loweredTerm = "dimitar";
        List<Author> expectedAuthors = new List<Author>
        {
            new Author { Name = "Dimitar Dimov" },
            new Author { Name = "Dimitar Talev" }
        };

        _mockRepository
            .Setup(repo => repo.SearchByPropertyAsync<Author>("Name", loweredTerm))
            .ReturnsAsync(expectedAuthors);

        List<Author> result = await _authorService.SearchAuthorsAsync(loweredTerm);

        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo(expectedAuthors[0].Name));
            Assert.That(result[1].Name, Is.EqualTo(expectedAuthors[1].Name));
        });
    }

    [Test]
    public async Task SearchAuthorsAsync_ReturnsEmptyList_WhenNoAuthorsMatch()
    {
        string searchTerm = "Nonexistent Author";
        List<Author> expectedAuthors = new List<Author>();

        _mockRepository
            .Setup(r => r.SearchByPropertyAsync<Author>("Name", searchTerm))
            .ReturnsAsync(expectedAuthors);

        List<Author> result = await _authorService.SearchAuthorsAsync(searchTerm);

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetAuthorByIdAsync_ReturnsCorrectAuthor()
    {
        Guid authorId = Guid.NewGuid();

        Author author = new Author
        {
            Id = authorId
        };

        _mockRepository
            .Setup(repo => repo.GetByIdAsync<Author>(authorId))
            .ReturnsAsync(author);

        Author? result = await _authorService.GetAuthorByIdAsync(authorId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(authorId));
        });
    }

    [Test]
    public async Task GetAuthorByIdAsync_ReturnsNull_WhenAuthorNotFound()
    {
        Guid authorId = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.GetByIdAsync<Author>(authorId))
            .ReturnsAsync((Author?)null);

        Author? result = await _authorService.GetAuthorByIdAsync(authorId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AuthorNameExistsAsync_ReturnsTrue_WhenNameExists()
    {
        string authorName = "Existing Author";
        Guid? id = null;

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Author>("Name", authorName, id))
            .ReturnsAsync(true);

        bool exists = await _authorService.AuthorNameExistsAsync(authorName, id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task AuthorNameExistsAsync_ReturnsTrue_WhenNameExists_WithDifferentId()
    {
        string authorName = "Duplicate Name";
        Guid? id = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Author>("Name", authorName, id))
            .ReturnsAsync(true);

        bool exists = await _authorService.AuthorNameExistsAsync(authorName, id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task AuthorNameExistsAsync_ReturnsFalse_WhenNameDoesNotExist_AndIdIsNull()
    {
        string authorName = "Nonexistent Name";
        Guid? id = null;

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Author>("Name", authorName, id))
            .ReturnsAsync(false);

        bool exists = await _authorService.AuthorNameExistsAsync(authorName, id);

        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task AuthorNameExistsAsync_ReturnsFalse_WhenNameDoesNotExist()
    {
        string authorName = "Nonexistent Author";
        Guid? id = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Author>("Name", authorName, id))
            .ReturnsAsync(false);

        bool exists = await _authorService.AuthorNameExistsAsync(authorName, id);

        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task AddAuthorAsync_CallsAddAndSaveChanges_WithCorrectAuthor()
    {
        AuthorAddViewModel addAuthor = new AuthorAddViewModel
        {
            Name = "New Author",
            Biography = "New Biography",
            Nationality = "New Nationality",
            ImageUrl = "http://image.url",
            ParsedBirthDate = new DateOnly(1990, 1, 1),
            Website = "http://website.url"
        };

        await _authorService.AddAuthorAsync(addAuthor);

        _mockRepository.Verify(
            repo => repo.AddAsync(It.Is<Author>(a =>
                a.Name == addAuthor.Name &&
                a.Biography == addAuthor.Biography &&
                a.Nationality == addAuthor.Nationality &&
                a.ImageUrl == addAuthor.ImageUrl &&
                a.BirthDate == addAuthor.ParsedBirthDate &&
                a.Website == addAuthor.Website
            )),
            Times.Once);

        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task EditAuthorAsync_UpdatesAuthorProperties_AndCallsSaveChanges()
    {
        Author author = new Author
        {
            Name = "Old Name",
            Biography = "Old Biography",
            Nationality = "Old Nationality",
            ImageUrl = "http://old.image.url",
            BirthDate = new DateOnly(1980, 1, 1),
            Website = "http://old.website.url"
        };

        AuthorEditViewModel editAuthor = new AuthorEditViewModel
        {
            Name = "Updated Name",
            Biography = "Updated Biography",
            Nationality = "Updated Nationality",
            ImageUrl = "http://updated.image.url",
            ParsedBirthDate = new DateOnly(1990, 5, 5),
            Website = "http://updated.website.url"
        };

        await _authorService.EditAuthorAsync(editAuthor, author);

        Assert.Multiple(() =>
        {
            Assert.That(author.Name, Is.EqualTo(editAuthor.Name));
            Assert.That(author.Biography, Is.EqualTo(editAuthor.Biography));
            Assert.That(author.Nationality, Is.EqualTo(editAuthor.Nationality));
            Assert.That(author.ImageUrl, Is.EqualTo(editAuthor.ImageUrl));
            Assert.That(author.BirthDate, Is.EqualTo(editAuthor.ParsedBirthDate));
            Assert.That(author.Website, Is.EqualTo(editAuthor.Website));
        });

        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task DeleteAuthorAsync_SetsIsDeletedTrue_AndCallsSaveChanges()
    {
        Author author = new Author
        {
            IsDeleted = false
        };

        await _authorService.DeleteAuthorAsync(author);

        Assert.That(author.IsDeleted, Is.True);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}