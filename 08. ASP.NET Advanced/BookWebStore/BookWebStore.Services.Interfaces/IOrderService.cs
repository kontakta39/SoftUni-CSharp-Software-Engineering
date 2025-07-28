using BookWebStore.Data.Models;

namespace BookWebStore.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderBook>> GetCartItemsAsync(string buyerId);

    Task<Order?> GetUserCurrentOrderAsync(string buyerId, Guid? orderId = null);

    Task<Order?> FindCompletedOrderAsync(string buyerId, Guid orderId);

    Task<Order?> CreateNewOrderAsync(string buyerId, string orderNumber);

    Task<OrderBook?> GetOrderBookAsync(Guid orderId, Guid bookId, bool? isReturned = null);

    Task<OrderBook> AddBookToOrderAsync(Order order, Guid bookId);

    Task UpdateQuantityAsync(OrderBook orderBook, int? quantity = null);

    Task RecalculatePricesAsync(Order order, OrderBook orderBook);

    Task RemoveFromCartAsync(Order order, OrderBook orderBook);

    Task CompleteOrderAsync(Order order);

    Task ReturnBookAsync(Order order, OrderBook orderBook);

    Task<List<OrderBook>> GetCompletedOrdersByUserAsync(string buyerId);
}