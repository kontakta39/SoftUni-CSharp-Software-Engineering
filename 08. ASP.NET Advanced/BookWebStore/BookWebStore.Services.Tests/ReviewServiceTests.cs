using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.ViewModels;
using Moq;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class ReviewServiceTests
{
    private Mock<IReviewRepository> _mockReviewRepository;
    private Mock<IBaseRepository> _mockBaseRepository;
    private ReviewService _reviewService;

    [SetUp]
    public void SetUp()
    {
        _mockReviewRepository = new Mock<IReviewRepository>();
        _mockBaseRepository = new Mock<IBaseRepository>();
        _reviewService = new ReviewService(_mockReviewRepository.Object, _mockBaseRepository.Object);
    }

    [Test]
    public async Task GetBookReviewsAsync_ShouldReturnReviews_WhenBookHasReviews()
    {
        Guid bookId = Guid.NewGuid();
        List<Review> expectedReviews = new List<Review>
        {
            new Review(),
            new Review()
        };

        _mockReviewRepository
            .Setup(r => r.GetBookReviewsAsync(bookId))
            .ReturnsAsync(expectedReviews);

        List<Review> result = await _reviewService.GetBookReviewsAsync(bookId);

        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetBookReviewsAsync_ShouldReturnEmptyList_WhenNoReviewsExist()
    {
        Guid bookId = Guid.NewGuid();
        List<Review> noReviews = new List<Review>();

        _mockReviewRepository
            .Setup(r => r.GetBookReviewsAsync(bookId))
            .ReturnsAsync(noReviews);

        List<Review> result = await _reviewService.GetBookReviewsAsync(bookId);

        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task ReviewExistsAsync_ShouldReturnReview_WhenReviewExists()
    {
        Guid bookId = Guid.NewGuid();
        string userId = "user123";

        Review expectedReview = new Review
        { 
             BookId = bookId,
             UserId = userId,
        };

        _mockReviewRepository
            .Setup(r => r.ReviewExistsAsync(bookId, userId))
            .ReturnsAsync(expectedReview);

        Review? result = await _reviewService.ReviewExistsAsync(bookId, userId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.BookId, Is.EqualTo(expectedReview.BookId));
            Assert.That(result!.UserId, Is.EqualTo(expectedReview.UserId));
        });
    }

    [Test]
    public async Task ReviewExistsAsync_ShouldReturnNull_WhenReviewDoesNotExist()
    {
        Guid bookId = Guid.NewGuid();
        string userId = "user123";

        _mockReviewRepository
            .Setup(r => r.ReviewExistsAsync(bookId, userId))
            .ReturnsAsync((Review?)null);

        Review? result = await _reviewService.ReviewExistsAsync(bookId, userId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddReviewAsync_ShouldCreateAndAddReview_WithCorrectProperties()
    {
        ReviewAddViewModel addReview = new ReviewAddViewModel
        {
            BookId = Guid.NewGuid(),
            Rating = 4,
            ReviewText = "Interesting book"
        };

        string userId = "user123";

        await _reviewService.AddReviewAsync(addReview, userId);

        _mockBaseRepository.Verify(
            repo => repo.AddAsync(It.Is<Review>(r =>
                r.BookId == addReview.BookId &&
                r.UserId == userId &&
                r.Rating == addReview.Rating &&
                r.ReviewText == addReview.ReviewText
            )),
            Times.Once);

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task EditReviewAsync_ShouldUpdateReviewProperties_AndSaveChanges()
    {
        ReviewEditViewModel editReview = new ReviewEditViewModel
        {
            Rating = 5,
            ReviewText = "Updated review text"
        };

        Review review = new Review
        {
            ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)),
            Rating = 3,
            ReviewText = "Old review text",
            IsEdited = false
        };

        DateOnly beforeEditDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await _reviewService.EditReviewAsync(editReview, review);

        DateOnly afterEditDate = DateOnly.FromDateTime(DateTime.UtcNow);

        Assert.Multiple(() =>
        {
            Assert.That(review.Rating, Is.EqualTo(editReview.Rating));
            Assert.That(review.ReviewText, Is.EqualTo(editReview.ReviewText));
            Assert.That(review.IsEdited, Is.True);

            //Check if ReviewDate is set to the current day (between before and after)
            Assert.That(review.ReviewDate, Is.InRange(beforeEditDate, afterEditDate));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}