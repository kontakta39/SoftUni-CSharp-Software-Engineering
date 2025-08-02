using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.ViewModels;
using Moq;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class BlogServiceTests
{
    private Mock<IBlogRepository> _mockBlogRepository;
    private Mock<IBaseRepository> _mockBaseRepository;
    private BlogService _blogService;

    [SetUp]
    public void SetUp()
    {
        _mockBlogRepository = new Mock<IBlogRepository>();
        _mockBaseRepository = new Mock<IBaseRepository>();
        _blogService = new BlogService(_mockBlogRepository.Object, _mockBaseRepository.Object);
    }

    [Test]
    public async Task GetAllBlogsAsync_ShouldReturnAllBlogs()
    {
        List<Blog> expectedBlogs = new List<Blog>
        {
            new Blog(),
            new Blog()
        };

        _mockBlogRepository
            .Setup(r => r.GetAllBlogsAsync())
            .ReturnsAsync(expectedBlogs);

        List<Blog> result = await _blogService.GetAllBlogsAsync();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllBlogsAsync_ShouldReturnEmptyList_WhenNoBlogsExist()
    {
        List<Blog> noBlogs = new List<Blog>();

        _mockBlogRepository
            .Setup(r => r.GetAllBlogsAsync())
            .ReturnsAsync(noBlogs);

        List<Blog> result = await _blogService.GetAllBlogsAsync();

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetBlogByIdAsync_ShouldReturnBlog_WhenBlogExists()
    {
        Guid id = Guid.NewGuid();

        Blog expectedBlog = new Blog
        {
            Id = id
        };

        _mockBlogRepository
            .Setup(r => r.GetBlogByIdAsync(id))
            .ReturnsAsync(expectedBlog);

        Blog? result = await _blogService.GetBlogByIdAsync(id);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(expectedBlog.Id));
        });
    }

    [Test]
    public async Task GetBlogByIdAsync_ShouldReturnNull_WhenBlogDoesNotExist()
    {
        Guid id = Guid.NewGuid();

        _mockBlogRepository
            .Setup(r => r.GetBlogByIdAsync(id))
            .ReturnsAsync((Blog?)null);

        Blog? result = await _blogService.GetBlogByIdAsync(id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddBlogAsync_ShouldCreateAndAddBlog_WithCorrectProperties()
    {
        BlogAddViewModel addBlog = new BlogAddViewModel
        {
            Title = "Test Blog",
            ImageUrl = "http://example.com/image.jpg",
            Content = "Test content"
        };

        string publisherId = "publisher123";

        await _blogService.AddBlogAsync(addBlog, publisherId);

        _mockBaseRepository.Verify(
            repo => repo.AddAsync(It.Is<Blog>(b =>
                b.Title == addBlog.Title &&
                b.ImageUrl == addBlog.ImageUrl &&
                b.Content == addBlog.Content &&
                b.PublisherId == publisherId
            )),
            Times.Once);

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task EditBlogAsync_ShouldUpdateBlogProperties_AndSaveChanges()
    {
        BlogEditViewModel editBlog = new BlogEditViewModel
        {
            Title = "Updated Title",
            ImageUrl = "http://example.com/updated-image.jpg",
            Content = "Updated content"
        };

        Blog blog = new Blog
        {
            Title = "Old Title",
            ImageUrl = "http://example.com/old-image.jpg",
            Content = "Old content"
        };

        await _blogService.EditBlogAsync(editBlog, blog);

        Assert.Multiple(() =>
        {
            Assert.That(blog.Title, Is.EqualTo(editBlog.Title));
            Assert.That(blog.ImageUrl, Is.EqualTo(editBlog.ImageUrl));
            Assert.That(blog.Content, Is.EqualTo(editBlog.Content));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task DeleteBlogAsync_ShouldMarkBlogAsDeleted_AndSaveChanges()
    {
        Blog blog = new Blog
        {
            IsDeleted = false
        };

        await _blogService.DeleteBlogAsync(blog);

        Assert.That(blog.IsDeleted, Is.True);
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}