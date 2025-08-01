using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.ViewModels;
using Moq;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class BookServiceTests
{
    private Mock<IBookRepository> _mockBookRepository;
    private Mock<IBaseRepository> _mockBaseRepository;
    private BookService _bookService;

    [SetUp]
    public void SetUp()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockBaseRepository = new Mock<IBaseRepository>();
        _bookService = new BookService(_mockBookRepository.Object, _mockBaseRepository.Object);
    }

    [Test]
    public async Task GetAllBooksAsync_ReturnsListOfBooks_WithRequiredPropertiesSet()
    {
        List<Book> books = new List<Book>
        {
            new Book(),
            new Book()
        };

        _mockBookRepository
            .Setup(repo => repo.GetAllBooksAsync())
            .ReturnsAsync(books);

        List<Book> result = await _bookService.GetAllBooksAsync();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllBooksAsync_ReturnsEmptyList_WhenNoBooks()
    {
        List<Book> emptyBooks = new List<Book>();

        _mockBookRepository
            .Setup(repo => repo.GetAllBooksAsync())
            .ReturnsAsync(emptyBooks);

        List<Book> result = await _bookService.GetAllBooksAsync();

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetBookByIdAsync_ReturnsCorrectBook()
    {
        Guid bookId = Guid.NewGuid();

        Book expectedBook = new Book
        {
            Id = bookId
        };

        _mockBookRepository
            .Setup(repo => repo.GetBookByIdAsync(bookId))
            .ReturnsAsync(expectedBook);

        Book? result = await _bookService.GetBookByIdAsync(bookId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(bookId));
        });
    }

    [Test]
    public async Task GetBookByIdAsync_ReturnsNull_WhenBookNotFound()
    {
        Guid missingId = Guid.NewGuid();

        _mockBookRepository
            .Setup(repo => repo.GetBookByIdAsync(missingId))
            .ReturnsAsync((Book?)null);

        Book? result = await _bookService.GetBookByIdAsync(missingId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task BookTitleExistsAsync_ReturnsTrue_WhenTitleExists()
    {
        string bookTitle = "Existing Book";
        Guid? id = null;

        _mockBaseRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Book>("Title", bookTitle, id))
            .ReturnsAsync(true);

        bool exists = await _bookService.BookNameExistsAsync(bookTitle, id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task BookTitleExistsAsync_ReturnsTrue_WhenTitleExists_WithDifferentId()
    {
        string title = "Duplicate Title";
        Guid? id = Guid.NewGuid();

        _mockBaseRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Book>("Title", title, id))
            .ReturnsAsync(true);

        bool exists = await _bookService.BookNameExistsAsync(title, id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task BookTitleExistsAsync_ReturnsFalse_WhenTitleDoesNotExist_AndIdIsNull()
    {
        string title = "Nonexistent Book";
        Guid? id = null;

        _mockBaseRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Book>("Title", title, id))
            .ReturnsAsync(false);

        bool exists = await _bookService.BookNameExistsAsync(title, id);

        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task BookTitleExistsAsync_ReturnsFalse_WhenTitleDoesNotExist()
    {
        string title = "Non‑Existing Book";
        Guid? id = Guid.NewGuid();

        _mockBaseRepository
            .Setup(repo => repo.ExistsByPropertyAsync<Book>("Title", title, id))
            .ReturnsAsync(false);

        bool exists = await _bookService.BookNameExistsAsync(title, id);

        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task AddBookAsync_CallsAddAndSaveChanges_WithCorrectBook()
    {
        BookAddViewModel addBook = new BookAddViewModel
        {
            Title = "The Pragmatic Programmer",
            Publisher = "Addison-Wesley",
            ReleaseYear = 1999,
            PagesNumber = 352,
            ImageUrl = "http://image.url",
            Price = 42.5m,
            Stock = 10,
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        await _bookService.AddBookAsync(addBook);

        _mockBaseRepository.Verify(repo => repo.AddAsync(It.Is<Book>(book =>
            book.Title == addBook.Title &&
            book.Publisher == addBook.Publisher &&
            book.ReleaseYear == addBook.ReleaseYear &&
            book.PagesNumber == addBook.PagesNumber &&
            book.ImageUrl == addBook.ImageUrl &&
            book.Price == addBook.Price &&
            book.Stock == addBook.Stock &&
            book.AuthorId == addBook.AuthorId &&
            book.GenreId == addBook.GenreId
        )), Times.Once);

        _mockBaseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task EditBookAsync_UpdatesBookProperties_AndCallsSaveChanges()
    {
        Guid authorId = Guid.NewGuid();
        Guid genreId = Guid.NewGuid();

        Book book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Publisher = "Old Publisher",
            ReleaseYear = 2000,
            PagesNumber = 300,
            ImageUrl = "http://old.image.url",
            Price = 25.0m,
            Stock = 5,
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        BookEditViewModel editBook = new BookEditViewModel
        {
            Title = "New Title",
            Publisher = "New Publisher",
            ReleaseYear = 2024,
            PagesNumber = 500,
            ImageUrl = "http://new.image.url",
            Price = 50.0m,
            Stock = 15,
            AuthorId = authorId,
            GenreId = genreId
        };

        await _bookService.EditBookAsync(editBook, book);

        Assert.Multiple(() =>
        {
            Assert.That(book.Title, Is.EqualTo(editBook.Title));
            Assert.That(book.Publisher, Is.EqualTo(editBook.Publisher));
            Assert.That(book.ReleaseYear, Is.EqualTo(editBook.ReleaseYear));
            Assert.That(book.PagesNumber, Is.EqualTo(editBook.PagesNumber));
            Assert.That(book.ImageUrl, Is.EqualTo(editBook.ImageUrl));
            Assert.That(book.Price, Is.EqualTo(editBook.Price));
            Assert.That(book.Stock, Is.EqualTo(editBook.Stock));
            Assert.That(book.AuthorId, Is.EqualTo(editBook.AuthorId));
            Assert.That(book.GenreId, Is.EqualTo(editBook.GenreId));
        });

        _mockBaseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task HasBooksInStockByGenreIdAsync_ReturnsTrue_WhenBooksInStockExist()
    {
        Guid genreId = Guid.NewGuid();

        _mockBookRepository
            .Setup(repo => repo.HasBooksInStockByPropertyIdAsync("GenreId", genreId))
            .ReturnsAsync(true);

        bool result = await _bookService.HasBooksInStockByGenreIdAsync(genreId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task HasBooksInStockByGenreIdAsync_ReturnsFalse_WhenNoBooksInStock()
    {
        Guid genreId = Guid.NewGuid();

        _mockBookRepository
            .Setup(repo => repo.HasBooksInStockByPropertyIdAsync("GenreId", genreId))
            .ReturnsAsync(false);

        bool result = await _bookService.HasBooksInStockByGenreIdAsync(genreId);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task HasBooksInStockByAuthorIdAsync_ReturnsTrue_WhenBooksInStockExist()
    {
        Guid authorId = Guid.NewGuid();

        _mockBookRepository
            .Setup(repo => repo.HasBooksInStockByPropertyIdAsync("AuthorId", authorId))
            .ReturnsAsync(true);

        bool result = await _bookService.HasBooksInStockByAuthorIdAsync(authorId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task HasBooksInStockByAuthorIdAsync_ReturnsFalse_WhenNoBooksInStock()
    {
        Guid authorId = Guid.NewGuid();

        _mockBookRepository
            .Setup(repo => repo.HasBooksInStockByPropertyIdAsync("AuthorId", authorId))
            .ReturnsAsync(false);

        bool result = await _bookService.HasBooksInStockByAuthorIdAsync(authorId);

        Assert.That(result, Is.False);
    }
}