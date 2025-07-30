using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.ViewModels;
using Moq;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class GenreServiceTests
{
    private Mock<IBaseRepository> _mockRepository;
    private GenreService _genreService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IBaseRepository>();
        _genreService = new GenreService(_mockRepository.Object);
    }

    [Test]
    public async Task GetAllGenresAsync_ReturnsListOfGenres()
    {
        //Arrange
        List<Genre> genres = new List<Genre>
        {
            new Genre(),
            new Genre()
        };

        _mockRepository
            .Setup(repo => repo.GetAllAsync<Genre>())
            .ReturnsAsync(genres);

        //Act
        List<Genre> result = await _genreService.GetAllGenresAsync();

        //Assert
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllGenresAsync_ReturnsEmptyList_WhenNoGenres()
    {
        List<Genre> emptyGenres = new List<Genre>();

        _mockRepository
            .Setup(repo => repo.GetAllAsync<Genre>())
            .ReturnsAsync(emptyGenres);

        List<Genre> result = await _genreService.GetAllGenresAsync();

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetGenreByIdAsync_ReturnsCorrectGenre()
    {
        Guid genreId = Guid.NewGuid();

        Genre genre = new Genre
        {
            Id = genreId
        };

        _mockRepository
            .Setup((IBaseRepository repo) => repo.GetByIdAsync<Genre>(genreId))
            .ReturnsAsync(genre);

        Genre? result = await _genreService.GetGenreByIdAsync(genreId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(genreId));
        });
    }

    [Test]
    public async Task GetGenreByIdAsync_ReturnsNull_WhenGenreNotFound()
    {
        Guid genreId = Guid.NewGuid();

        _mockRepository
            .Setup((IBaseRepository repo) => repo.GetByIdAsync<Genre>(genreId))
            .ReturnsAsync((Genre?)null);

        Genre? result = await _genreService.GetGenreByIdAsync(genreId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GenreNameExistsAsync_ReturnsTrue_WhenNameExists()
    {
        string genreName = "Fiction";
        Guid? id = null;

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Genre>("Name", genreName, id))
            .ReturnsAsync(true);

        bool exists = await _genreService.GenreNameExistsAsync(genreName, id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task GenreNameExistsAsync_ReturnsTrue_WhenNameExists_WithDifferentId()
    {
        string genreName = "Duplicate Genre";
        Guid? id = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Genre>("Name", genreName, id))
            .ReturnsAsync(true);

        bool exists = await _genreService.GenreNameExistsAsync(genreName, id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task GenreNameExistsAsync_ReturnsFalse_WhenNameDoesNotExist_AndIdIsNull()
    {
        string genreName = "Nonexistent Genre";
        Guid? id = null;

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Genre>("Name", genreName, id))
            .ReturnsAsync(false);

        bool exists = await _genreService.GenreNameExistsAsync(genreName, id);

        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task GenreNameExistsAsync_ReturnsFalse_WhenNameDoesNotExist()
    {
        string genreName = "Nonexistent Genre";
        Guid id = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Genre>("Name", genreName, id))
            .ReturnsAsync(false);

        bool exists = await _genreService.GenreNameExistsAsync(genreName, id);

        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task AddGenreAsync_CallsAddAndSaveChanges()
    {
        GenreAddViewModel addGenre = new GenreAddViewModel
        {
            Name = "New Genre"
        };

        await _genreService.AddGenreAsync(addGenre);

        _mockRepository.Verify(
           repo => repo.AddAsync(It.Is<Genre>(g =>
                g.Name == "New Genre"
           )),
           Times.Once);

        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task EditGenreAsync_UpdatesGenreNameAndSavesChanges()
    {
        Genre genre = new Genre
        {
            Name = "Old Name"
        };

        GenreEditViewModel editGenre = new GenreEditViewModel
        {
            Name = "Updated Name"
        };

        await _genreService.EditGenreAsync(editGenre, genre);

        Assert.That(genre.Name, Is.EqualTo("Updated Name"));
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task DeleteGenreAsync_SetsIsDeletedTrueAndSavesChanges()
    {
        Genre genre = new Genre
        {
            IsDeleted = false
        };

        await _genreService.DeleteGenreAsync(genre);

        Assert.That(genre.IsDeleted, Is.True);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}