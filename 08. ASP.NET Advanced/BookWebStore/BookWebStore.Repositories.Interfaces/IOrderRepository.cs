using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<OrderBook>> GetCartItemsAsync(string buyerId);

    Task<Order?> GetUserCurrentOrderAsync(string buyerId, Guid? orderId = null);

    Task<Order?> FindCompletedOrderAsync(string buyerId, Guid orderId);

    Task<OrderBook?> GetOrderBookAsync(Guid orderId, Guid bookId, bool? isReturned = null);

    Task<List<OrderBook>> GetCompletedOrdersByUserAsync(string buyerId);
}