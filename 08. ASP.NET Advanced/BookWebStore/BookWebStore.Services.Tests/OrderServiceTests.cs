using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Moq;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockOrderRepository;
    private Mock<IBaseRepository> _mockBaseRepository;
    private OrderService _orderService;

    [SetUp]
    public void Setup()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockBaseRepository = new Mock<IBaseRepository>();
        _orderService = new OrderService(_mockOrderRepository.Object, _mockBaseRepository.Object);
    }

    [Test]
    public async Task GetCartItemsAsync_ReturnsListOfOrderBooks()
    {
        string buyerId = "user123";

        List<OrderBook> cartItems = new List<OrderBook>
            {
                new OrderBook(),
                new OrderBook()
            };

        _mockOrderRepository
            .Setup(repo => repo.GetCartItemsAsync(buyerId))
            .ReturnsAsync(cartItems);

        List<OrderBook> result = await _orderService.GetCartItemsAsync(buyerId);

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetCartItemsAsync_ReturnsEmptyList_WhenNoItems()
    {
        string buyerId = "user123";
        List<OrderBook> emptyCart = new List<OrderBook>();

        _mockOrderRepository
            .Setup(repo => repo.GetCartItemsAsync(buyerId))
            .ReturnsAsync(emptyCart);

        List<OrderBook> result = await _orderService.GetCartItemsAsync(buyerId);

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetUserCurrentOrderAsync_ReturnsOrder_WhenOrderExists()
    {
        string buyerId = "user123";
        Guid orderId = Guid.NewGuid();

        Order expectedOrder = new Order
        {
            Id = orderId,
            BuyerId = buyerId,
            IsCompleted = false
        };

        _mockOrderRepository
            .Setup(repo => repo.GetUserCurrentOrderAsync(buyerId, orderId))
            .ReturnsAsync(expectedOrder);

        Order? result = await _orderService.GetUserCurrentOrderAsync(buyerId, orderId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(orderId));
            Assert.That(result.BuyerId, Is.EqualTo(buyerId));
            Assert.That(result.IsCompleted, Is.False);
        });
    }

    [Test]
    public async Task GetUserCurrentOrderAsync_ReturnsNull_WhenOrderNotFound()
    {
        string buyerId = "user123";
        Guid orderId = Guid.NewGuid();

        _mockOrderRepository
            .Setup(repo => repo.GetUserCurrentOrderAsync(buyerId, orderId))
            .ReturnsAsync((Order?)null);

        Order? result = await _orderService.GetUserCurrentOrderAsync(buyerId, orderId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUserCurrentOrderAsync_WithNullOrderId_ReturnsOrder()
    {
        string buyerId = "user123";

        Order expectedOrder = new Order
        {
            BuyerId = buyerId,
            IsCompleted = false
        };

        _mockOrderRepository
            .Setup(repo => repo.GetUserCurrentOrderAsync(buyerId, null))
            .ReturnsAsync(expectedOrder);

        Order? result = await _orderService.GetUserCurrentOrderAsync(buyerId, null);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.BuyerId, Is.EqualTo(buyerId));
        Assert.That(result.IsCompleted, Is.False);
    }

    [Test]
    public async Task FindCompletedOrderAsync_ReturnsOrder_WhenCompletedOrderExists()
    {
        string buyerId = "user123";
        Guid orderId = Guid.NewGuid();

        Order expectedOrder = new Order
        {
            Id = orderId,
            BuyerId = buyerId,
            IsCompleted = true
        };

        _mockOrderRepository
            .Setup(repo => repo.FindCompletedOrderAsync(buyerId, orderId))
            .ReturnsAsync(expectedOrder);

        Order? result = await _orderService.FindCompletedOrderAsync(buyerId, orderId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(orderId));
            Assert.That(result.BuyerId, Is.EqualTo(buyerId));
            Assert.That(result.IsCompleted, Is.True);
        });
    }

    [Test]
    public async Task FindCompletedOrderAsync_ReturnsNull_WhenNoCompletedOrderExists()
    {
        string buyerId = "user123";
        Guid orderId = Guid.NewGuid();

        _mockOrderRepository
            .Setup(repo => repo.FindCompletedOrderAsync(buyerId, orderId))
            .ReturnsAsync((Order?)null);

        Order? result = await _orderService.FindCompletedOrderAsync(buyerId, orderId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateNewOrderAsync_CreatesAndReturnsOrder_WithCorrectValues()
    {
        string buyerId = "user123";
        string orderNumber = "#319472658";

        await _orderService.CreateNewOrderAsync(buyerId, orderNumber);

        _mockBaseRepository.Verify(
            repo => repo.AddAsync(It.Is<Order>(o =>
                o.BuyerId == buyerId &&
                o.OrderNumber == orderNumber
            )),
            Times.Once);

        _mockBaseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetOrderBookAsync_ReturnsOrderBook_WhenFound()
    {
        Guid orderId = Guid.NewGuid();
        Guid bookId = Guid.NewGuid();
        bool? isReturned = null;

        OrderBook expectedOrderBook = new OrderBook
        {
            OrderId = orderId,
            BookId = bookId,
            IsReturned = false
        };

        _mockOrderRepository
            .Setup(repo => repo.GetOrderBookAsync(orderId, bookId, isReturned))
            .ReturnsAsync(expectedOrderBook);

        OrderBook? result = await _orderService.GetOrderBookAsync(orderId, bookId, isReturned);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.OrderId, Is.EqualTo(orderId));
            Assert.That(result.BookId, Is.EqualTo(bookId));
            Assert.That(result.IsReturned, Is.False);
        });
    }

    [Test]
    public async Task GetOrderBookAsync_ReturnsOrderBook_WhenIsReturnedFalse()
    {
        Guid orderId = Guid.NewGuid();
        Guid bookId = Guid.NewGuid();
        bool isReturned = false;

        OrderBook expected = new OrderBook
        {
            OrderId = orderId,
            BookId = bookId,
            IsReturned = false
        };

        _mockOrderRepository
            .Setup(r => r.GetOrderBookAsync(orderId, bookId, isReturned))
            .ReturnsAsync(expected);

        OrderBook? result = await _orderService.GetOrderBookAsync(orderId, bookId, isReturned);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsReturned, Is.False);
    }

    [Test]
    public async Task GetOrderBookAsync_ReturnsNull_WhenNotFound()
    {
        //Arrange: Simulate no returned books are matched
        Guid orderId = Guid.NewGuid();
        Guid bookId = Guid.NewGuid();
        bool isReturned = true;

        _mockOrderRepository
            .Setup(repo => repo.GetOrderBookAsync(orderId, bookId, isReturned))
            .ReturnsAsync((OrderBook?)null);

        OrderBook? result = await _orderService.GetOrderBookAsync(orderId, bookId, isReturned);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddBookToOrderAsync_AddsOrderBookToOrder_AndSavesChanges()
    {
        Order order = new Order
        {
            Id = Guid.NewGuid(),
            OrdersBooks = new List<OrderBook>()
        };

        Guid bookId = Guid.NewGuid();

        OrderBook result = await _orderService.AddBookToOrderAsync(order, bookId);

        Assert.Multiple(() =>
        {
            Assert.That(result.OrderId, Is.EqualTo(order.Id));
            Assert.That(result.BookId, Is.EqualTo(bookId));
            Assert.That(order.OrdersBooks, Does.Contain(result));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuantityAsync_WhenNewQuantityIsNull_AddsQuantityBackToStockAndSetsIsDeletedFalse()
    {
        Book book = new Book
        {
            Stock = 5,
            IsDeleted = false
        };

        OrderBook orderBook = new OrderBook
        {
            Book = book,
            Quantity = 3
        };

        await _orderService.UpdateQuantityAsync(orderBook, null);

        Assert.That(book.Stock, Is.EqualTo(8));  //5 + 3
        Assert.That(book.IsDeleted, Is.False);   //Stock > 0
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuantityAsync_WhenNewQuantityIsNull_AddsQuantityBackAndKeepsIsDeletedFalse_IfStockWasZero()
    {
        Book book = new Book
        {
            Stock = 0,
            IsDeleted = true
        };

        OrderBook orderBook = new OrderBook
        {
            Book = book,
            Quantity = 2
        };

        await _orderService.UpdateQuantityAsync(orderBook, null);

        Assert.That(book.Stock, Is.EqualTo(2));  //0 + 2
        Assert.That(book.IsDeleted, Is.False);   //Stock > 0 after addition
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuantityAsync_WhenNewQuantityIsSet_UpdatesQuantityAndStockProperly_WithoutDeletingBook()
    {
        Book book = new Book
        {
            Stock = 10,
            IsDeleted = false
        };

        OrderBook orderBook = new OrderBook
        {
            Book = book,
            Quantity = 5
        };

        int newQuantity = 8;  //Increase quantity by 3

        await _orderService.UpdateQuantityAsync(orderBook, newQuantity);

        int difference = newQuantity - 5; //3
        int expectedStock = 10 - difference; //7

        Assert.Multiple(() =>
        {
            Assert.That(orderBook.Quantity, Is.EqualTo(newQuantity));
            Assert.That(book.Stock, Is.EqualTo(expectedStock));
            Assert.That(book.IsDeleted, Is.False); //Stock > 0
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuantityAsync_WhenNewQuantityIsSet_AndStockFallsToZero_DeletesBook()
    {
        Book book = new Book
        {
            Stock = 2,
            IsDeleted = false
        };

        OrderBook orderBook = new OrderBook
        {
            Book = book,
            Quantity = 3
        };

        int newQuantity = 5; //Increase by 2, stock decreases by 2

        await _orderService.UpdateQuantityAsync(orderBook, newQuantity);

        Assert.Multiple(() =>
        {
            Assert.That(book.Stock, Is.EqualTo(0)); //Stock cannot goes to 0
            Assert.That(book.IsDeleted, Is.True);   //Stock == 0 means deleted
            Assert.That(orderBook.Quantity, Is.EqualTo(newQuantity));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuantityAsync_WhenNewQuantityIsSet_AndStockWouldBecomeNegative_SetsStockToZeroAndDeletesBook()
    {
        Book book = new Book
        {
            Stock = 1,
            IsDeleted = false
        };

        OrderBook orderBook = new OrderBook
        {
            Book = book,
            Quantity = 3
        };

        int newQuantity = 6; //Increase by 3, stock would go negative without Math.Max

        await _orderService.UpdateQuantityAsync(orderBook, newQuantity);

        Assert.Multiple(() =>
        {
            Assert.That(book.Stock, Is.EqualTo(0)); //Stock set to 0 by Math.Max
            Assert.That(book.IsDeleted, Is.True);   //Stock == 0 means deleted
            Assert.That(orderBook.Quantity, Is.EqualTo(newQuantity));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateQuantityAsync_WhenQuantityUnchanged_DoesNotChangeStockOrIsDeleted()
    {
        Book book = new Book
        {
            Stock = 10,
            IsDeleted = false
        };

        OrderBook orderBook = new OrderBook
        {
            Book = book,
            Quantity = 5
        };

        await _orderService.UpdateQuantityAsync(orderBook, 5);

        Assert.Multiple(() =>
        {
            Assert.That(book.Stock, Is.EqualTo(10)); //No change
            Assert.That(book.IsDeleted, Is.False);
            Assert.That(orderBook.Quantity, Is.EqualTo(5));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task RecalculatePricesAsync_UpdatesUnitPriceAndTotalPriceCorrectly()
    {
        Book book1 = new Book 
        { 
            Price = 10.00m 
        };

        Book book2 = new Book 
        { 
            Price = 5.00m 
        };

        OrderBook orderBook1 = new OrderBook 
        { 
            Book = book1, 
            Quantity = 2 
        }; //20.00

        OrderBook orderBook2 = new OrderBook 
        { 
            Book = book2, 
            Quantity = 3 
        }; //15.00

        Order order = new Order
        {
            OrdersBooks = new List<OrderBook> 
            { 
                orderBook1, 
                orderBook2 
            }
        };

        await _orderService.RecalculatePricesAsync(order, orderBook1);
        await _orderService.RecalculatePricesAsync(order, orderBook2);

        Assert.Multiple(() =>
        {
            Assert.That(orderBook1.UnitPrice, Is.EqualTo(20.00m));
            Assert.That(orderBook2.UnitPrice, Is.EqualTo(15.00m));
            Assert.That(order.TotalPrice, Is.EqualTo(35.00m));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task RecalculatePricesAsync_WithZeroQuantity_SetsUnitPriceToZero()
    {
        Book book = new Book 
        { 
            Price = 10.00m 
        };

        OrderBook orderBook = new OrderBook 
        { 
            Book = book, 
            Quantity = 0 
        };

        Order order = new Order
        {
            OrdersBooks = new List<OrderBook> { orderBook }
        };

        await _orderService.RecalculatePricesAsync(order, orderBook);

        Assert.That(orderBook.UnitPrice, Is.EqualTo(0.00m));
        Assert.That(order.TotalPrice, Is.EqualTo(0.00m));
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task RemoveFromCartAsync_RemovesOrderBookAndUpdatesTotalPrice()
    {
        OrderBook orderBook = new OrderBook
        {
            UnitPrice = 10.00m
        };

        Order order = new Order
        {
            TotalPrice = 30.00m,
            OrdersBooks = new List<OrderBook> 
            { 
                orderBook 
            }
        };

        await _orderService.RemoveFromCartAsync(order, orderBook);

        Assert.That(order.TotalPrice, Is.EqualTo(20.00m));
        Assert.That(order.OrdersBooks.Contains(orderBook), Is.False);
        _mockBaseRepository.Verify(r => r.Remove(It.IsAny<Order>()), Times.Never);
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task RemoveFromCartAsync_RemovesOrderWhenTotalPriceBecomesZero()
    {
        OrderBook orderBook = new OrderBook
        {
            UnitPrice = 15.00m
        };

        Order order = new Order
        {
            TotalPrice = 15.00m,
            OrdersBooks = new List<OrderBook> 
            { 
                orderBook 
            }
        };

        await _orderService.RemoveFromCartAsync(order, orderBook);

        Assert.That(order.TotalPrice, Is.EqualTo(0.00m));
        Assert.That(order.OrdersBooks.Contains(orderBook), Is.False);
        _mockBaseRepository.Verify(r => r.Remove(order), Times.Once);
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task CompleteOrderAsync_SetsIsCompletedTrue_AndSavesChanges()
    {
        Order order = new Order 
        { 
            IsCompleted = false 
        };

        await _orderService.CompleteOrderAsync(order);

        Assert.That(order.IsCompleted, Is.True);
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task ReturnBookAsync_SetsIsReturnedToTrue_AndSavesChanges()
    {
        OrderBook orderBook = new OrderBook 
        { 
            IsReturned = false 
        };

        await _orderService.ReturnBookAsync(orderBook);

        Assert.That(orderBook.IsReturned, Is.True);
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetCompletedOrdersByUserAsync_ReturnsListOfOrderBooks()
    {
        string buyerId = "user123";

        List<OrderBook> completedOrders = new List<OrderBook>
        {
            new OrderBook(),
            new OrderBook() 
        };

        _mockOrderRepository
            .Setup(repo => repo.GetCompletedOrdersByUserAsync(buyerId))
            .ReturnsAsync(completedOrders);

        List<OrderBook> result = await _orderService.GetCompletedOrdersByUserAsync(buyerId);

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetCompletedOrdersByUserAsync_ReturnsEmptyList_WhenNoCompletedOrders()
    {
        string buyerId = "user123";
        List<OrderBook> emptyList = new List<OrderBook>();

        _mockOrderRepository
            .Setup(repo => repo.GetCompletedOrdersByUserAsync(buyerId))
            .ReturnsAsync(emptyList);

        List<OrderBook> result = await _orderService.GetCompletedOrdersByUserAsync(buyerId);

        Assert.That(result.Count, Is.EqualTo(0));
    }
}