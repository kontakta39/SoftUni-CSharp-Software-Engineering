using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BookStoreDbContext _context;

    public OrderRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderBook>> GetCartItemsAsync(string buyerId)
    {
        return await _context.OrdersBooks
            .Include(ob => ob.Order)
            .Include(ob => ob.Book)
            .Where(ob => ob.Order.BuyerId == buyerId && !ob.Order.IsCompleted)
            .ToListAsync();
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

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<OrderBook?> GetOrderBookAsync(Guid orderId, Guid bookId, bool? isReturned = null)
    {
        return await _context.OrdersBooks
                .Include(ob => ob.Order)
                .Include(ob => ob.Book)
                .FirstOrDefaultAsync(ob => ob.OrderId == orderId && ob.BookId == bookId &&
                (isReturned == null || ob.IsReturned == isReturned));
    }

    public void Remove(Order order)
    {
        _context.Orders.Remove(order);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderBook>> GetCompletedOrdersByUserAsync(string buyerId)
    {
        return await _context.OrdersBooks
            .Include(ob => ob.Book)
            .Include(ob => ob.Order)
            .Where(ob => ob.Order.BuyerId == buyerId && ob.Order.IsCompleted)
            .OrderBy(ob => ob.Order.OrderDate)
            .ToListAsync();
    }
}