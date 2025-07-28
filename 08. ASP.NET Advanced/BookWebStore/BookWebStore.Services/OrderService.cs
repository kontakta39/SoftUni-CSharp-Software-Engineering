using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;

namespace BookWebStore.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderBook>> GetCartItemsAsync(string buyerId)
    {
        return await _orderRepository.GetCartItemsAsync(buyerId);
    }

    public async Task<Order?> GetUserCurrentOrderAsync(string buyerId, Guid? orderId = null)
    {
        return await _orderRepository.GetUserCurrentOrderAsync(buyerId, orderId);
    }

    public async Task<Order?> FindCompletedOrderAsync(string buyerId, Guid orderId)
    {
        return await _orderRepository.FindCompletedOrderAsync(buyerId, orderId);
    }

    public async Task<Order?> CreateNewOrderAsync(string buyerId, string orderNumber)
    {
        Order order = new Order
        {
            BuyerId = buyerId,
            OrderNumber = orderNumber
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();
        return order;
    }

    public async Task<OrderBook?> GetOrderBookAsync(Guid orderId, Guid bookId, bool? isReturned = null)
    {
        return await _orderRepository.GetOrderBookAsync(orderId, bookId, isReturned);
    }

    public async Task<OrderBook> AddBookToOrderAsync(Order order, Guid bookId)
    {
        OrderBook orderBook = new OrderBook
        {
            OrderId = order.Id,
            BookId = bookId
        };

        order.OrdersBooks.Add(orderBook);
        await _orderRepository.SaveChangesAsync();
        return orderBook;
    }

    public async Task UpdateQuantityAsync(OrderBook orderBook, int? newQuantity = null)
    {
        if (newQuantity == null)
        {
            orderBook.Book.Stock += orderBook.Quantity;
            orderBook.Book.IsDeleted = orderBook.Book.Stock <= 0;
        }
        else
        {
            //Current quantity in the cart
            int oldQuantity = orderBook.Quantity;
            //How much is the increase or decrease
            int difference = newQuantity.Value - oldQuantity;

            //Updating the quantity in the cart
            orderBook.Quantity = newQuantity.Value;
            orderBook.Book.Stock -= difference;

            orderBook.Book.IsDeleted = orderBook.Book.Stock < 1;
        }

        await _orderRepository.SaveChangesAsync();
    }

    public async Task RecalculatePricesAsync(Order order, OrderBook orderBook)
    {
        //Calculating the unit price of a specific book
        orderBook.UnitPrice = orderBook.Quantity * orderBook.Book.Price;

        //Calculating the total price of all books in the order
        order.TotalPrice = order.OrdersBooks
            .Sum(ob => ob.UnitPrice);

        await _orderRepository.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(Order order, OrderBook orderBook)
    {
        order.TotalPrice -= orderBook.UnitPrice;
        order.OrdersBooks.Remove(orderBook);

        if (order.TotalPrice == 0)
        {
            _orderRepository.Remove(order);
        }

        await _orderRepository.SaveChangesAsync();
    }

    public async Task CompleteOrderAsync(Order order)
    {
        order.IsCompleted = true;
        await _orderRepository.SaveChangesAsync();
    }

    public async Task ReturnBookAsync(Order order, OrderBook orderBook)
    {
        orderBook.IsReturned = true;
        await _orderRepository.SaveChangesAsync();
    }

    public async Task<List<OrderBook>> GetCompletedOrdersByUserAsync(string buyerId)
    {
        return await _orderRepository.GetCompletedOrdersByUserAsync(buyerId);
    }
}