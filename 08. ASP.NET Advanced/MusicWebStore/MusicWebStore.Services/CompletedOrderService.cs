using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class CompletedOrderService : ICompletedOrderInterface
{
    private readonly MusicStoreDbContext _context;

    public CompletedOrderService(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompletedOrderViewModel>> OrdersList(string buyerId)
    {
        List<CompletedOrderViewModel> completedOrders = await _context.Orders
            .Where(or => or.BuyerId == buyerId && or.IsCompleted)
            .Select(or => new CompletedOrderViewModel
            {
                Id = or.Id,
                OrderNumber = or.OrderNumber,
                OrderDate = or.OrderDate,
                TotalPrice = or.TotalPrice,
                OrderedAlbums = or.OrdersAlbums
                    .Select(oa => new OrderedAlbumViewModel()
                    {
                        AlbumId = oa.AlbumId,
                        AlbumImageUrl = oa.Album.ImageUrl!,
                        AlbumTitle = oa.Album.Title,
                        AlbumQuantity = oa.Quantity,
                        AlbumPrice = oa.Price,
                        isReturned = oa.isReturned
                    })
                    .ToList()
            })
            .ToListAsync();

        return completedOrders;
    }

    public async Task<Order?> CompleteOrder(Guid orderId, string buyerId)
    {
        //Find the order that matches the provided ID, buyer, and is not completed yet
        Order? orderToComplete = await _context.Orders
            .Where(order => order.Id == orderId && order.BuyerId == buyerId && !order.IsCompleted)
            .FirstOrDefaultAsync();

        if (orderToComplete == null)
        {
            return null;
        }

        //Mark the order as completed
        orderToComplete.IsCompleted = true;

        await _context.SaveChangesAsync();
        return orderToComplete;
    }

    public async Task<OrderAlbum?> ReturnAlbum(Guid orderId, Guid albumId, string buyerId)
    {
        //Find the order that should be returned
        Order? order = await _context.Orders
            .Where(o => o.Id == orderId && o.BuyerId == buyerId && o.IsCompleted == true)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return null; 
        }

        //Find the album in the order that is not yet returned
        OrderAlbum? albumFromOrder = await _context.OrdersAlbums
            .Where(a => a.OrderId == orderId && a.AlbumId == albumId && a.isReturned == false)
            .FirstOrDefaultAsync();

        if (albumFromOrder == null)
        {
            return null;
        }

        //Find the album in the stock
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return null; 
        }

        //Update stock and availability
        album.Stock += albumFromOrder.Quantity;
        album.IsDeleted = false;

        // Mark the album as returned
        albumFromOrder.isReturned = true;

        await _context.SaveChangesAsync();
        return albumFromOrder; 
    }
}