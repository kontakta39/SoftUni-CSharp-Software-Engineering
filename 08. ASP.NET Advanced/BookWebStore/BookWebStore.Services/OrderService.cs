using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class OrderService : IOrderService
{
    private readonly BookStoreDbContext _context;

    public OrderService(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderBook>> GetCartItemsAsync(ApplicationUser user)
    {
        List<OrderBook> cart = await _context.OrdersBooks
           .Include(ob => ob.Order)
           .Include(ob => ob.Book)
           .Where(ob => ob.Order.BuyerId == user.Id && !ob.Order.IsCompleted)
           .ToListAsync();

        return cart;
    }

    public async Task<Order?> GetUserCurrentOrderAsync(string buyerId, Guid? orderId = null)
    {
        return await _context.Orders
            .Include(o => o.OrdersBooks)
            .FirstOrDefaultAsync(o => o.BuyerId == buyerId &&
                (orderId == null || o.Id == orderId) && !o.IsCompleted);
    }

    public async Task<Order?> FindCompletedOrderAsync(string buyerId, Guid orderId)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => o.BuyerId == buyerId && o.Id == orderId && o.IsCompleted);
    }

    public async Task<Order?> CreateNewOrderAsync(string userId, string orderNumber)
    {
        Order order = new Order
        {
            BuyerId = userId,
            OrderNumber = orderNumber
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<OrderBook?> GetOrderBookAsync(Guid orderId, Guid bookId, bool? isReturned = null)
    {
        return await _context.OrdersBooks
                        .Include(ob => ob.Order)
                        .Include(ob => ob.Book)
                        .FirstOrDefaultAsync(ob => ob.OrderId == orderId && ob.BookId == bookId &&
                        (isReturned == null || ob.IsReturned == isReturned));
    }

    public async Task<OrderBook> AddBookToOrderAsync(Order order, Book book)
    {
        OrderBook orderBook = new OrderBook
        {
            OrderId = order.Id,
            BookId = book.Id
        };

        order.OrdersBooks.Add(orderBook);
        await _context.SaveChangesAsync();
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

        await _context.SaveChangesAsync();
    }

    public async Task RecalculatePricesAsync(Order order, OrderBook orderBook)
    {
        //Calculating the unit price of a specific book
        orderBook.UnitPrice = orderBook.Quantity * orderBook.Book.Price;

        //Calculating the total price of all books in the order
        order.TotalPrice = order.OrdersBooks
            .Sum(ob => ob.UnitPrice);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(Order order, OrderBook orderBook)
    {
        order.TotalPrice -= orderBook.UnitPrice;
        order.OrdersBooks.Remove(orderBook);

        if (order.TotalPrice == 0)
        {
            _context.Orders.Remove(order);
        }

        await _context.SaveChangesAsync();
    }

    public async Task CompleteOrderAsync(Order order)
    {
        order.IsCompleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task ReturnBookAsync(Order order, OrderBook orderBook)
    {
        orderBook.IsReturned = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderBook>> GetCompletedOrdersByUserAsync(ApplicationUser user)
    {
        return await _context.OrdersBooks
            .Include(ob => ob.Book)
            .Include(ob => ob.Order)
            .Where(ob => ob.Order.BuyerId == user.Id && ob.Order.IsCompleted)
            .OrderBy(ob => ob.Order.OrderDate)
            .ToListAsync();
    }
}