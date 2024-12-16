using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public class OrderService : IOrderInterface
{
    private readonly MusicStoreDbContext _context;

    public OrderService(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderCartViewModel>> Cart(string buyerId)
    {
        List<OrderCartViewModel> model = await _context.OrdersAlbums
            .Where(oa => !oa.Order.IsCompleted && oa.Order.BuyerId == buyerId)
            .Select(oa => new OrderCartViewModel
            {
                Id = oa.OrderId,
                AlbumId = oa.AlbumId,
                ImageUrl = oa.Album.ImageUrl!,
                AlbumTitle = oa.Album.Title,
                Quantity = oa.Quantity,
                UnitPrice = oa.Price
            })
            .AsNoTracking()
            .ToListAsync();

        return model;
    }

    public async Task AddToCart(Guid albumId, string buyerId)
    {
        int quantity = 1;

        //Check if the order exists
        Order? order = await _context.Orders
            .Where(o => o.BuyerId == buyerId && !o.IsCompleted)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            //Creating the order number
            Random random = new Random();

            //Create a list of numbers from 1 to 9
            List<int> digits = Enumerable.Range(1, 9).ToList();
            List<int> shuffledDigits = digits.OrderBy(x => random.Next()).ToList();

            // Concatenate the digits into a single number
            string randomNumber = "#" + string.Join("", shuffledDigits);

            order = new Order
            {
                BuyerId = buyerId,
                OrderNumber = randomNumber,
                OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
                TotalQuantity = 0,
                TotalPrice = 0
            };

            _context.Orders.Add(order);
        }
        else
        {
            throw new ArgumentException("You have not completed your previous order.");
        }

        //Check if the album exists and is there enough quantity
        Album? album = await _context.Albums
            .Where(a => a.Id == albumId && !a.IsDeleted)
            .FirstOrDefaultAsync();

        if (album == null || album.Stock < quantity)
        {
            throw new ArgumentException("There is not enough quantity.");
        }

        //Check if the album is not already added to the Cart
        OrderAlbum? existingOrderAlbum = await _context.OrdersAlbums
            .Where(oa => oa.OrderId == order.Id && oa.AlbumId == albumId)
            .FirstOrDefaultAsync();

        if (existingOrderAlbum != null)
        {
            throw new ArgumentException("The album has already been added to the cart.");
        }

        order.OrdersAlbums.Add(new OrderAlbum
        {
            OrderId = order.Id,
            AlbumId = albumId,
            Quantity = quantity,
            Price = quantity * album.Price
        });

        //Calculating the total price and quantity
        order.TotalQuantity += quantity;
        order.TotalPrice += quantity * album.Price;
        album.Stock -= quantity;

        if (album.Stock <= 0)
        {
            album.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<(bool, string?, decimal, int)> UpdateQuantity(Guid orderId, Guid albumId, int quantity)
    {
        //Ensure the requested quantity is valid
        if (quantity < 1)
        {
            return (false, "Quantity must be at least 1.", 0, 0);
        }

        //Retrieve the order and associated albums
        Order? order = await _context.Orders
            .Include(o => o.OrdersAlbums)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return (false, "Order not found.", 0, 0);
        }

        //Find the specific album in the order
        OrderAlbum? orderAlbum = order.OrdersAlbums.FirstOrDefault(oa => oa.AlbumId == albumId);

        if (orderAlbum == null)
        {
            return (false, "Order album not found.", 0, 0);
        }

        //Find the album in the catalog
        Album? album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId && !a.IsDeleted);

        if (album == null)
        {
            return (false, "Album not found.", 0, 0);
        }

        //Check if the requested quantity exceeds available stock
        if (quantity > album.Stock + orderAlbum.Quantity)
        {
            int availableStock = album.Stock + orderAlbum.Quantity;
            return (false, $"Only {availableStock} units are available.", 0, availableStock);
        }

        //Calculate quantity change and update the order
        int quantityChange = quantity - orderAlbum.Quantity;
        orderAlbum.Quantity = quantity;
        orderAlbum.Price = quantity * album.Price;
        order.TotalQuantity += quantityChange;
        order.TotalPrice += quantityChange * album.Price;

        //Update the album stock
        album.Stock -= quantityChange;

        if (album.Stock < 1)
        {
            album.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
        return (true, null, orderAlbum.Price, album.Stock);
    }

    public async Task RemoveFromCart(Guid orderId, Guid albumId, string buyerId)
    {
        OrderAlbum? orderAlbumToBeDeleted = await _context.OrdersAlbums
            .Where(oa => oa.OrderId == orderId && oa.AlbumId == albumId)
            .FirstOrDefaultAsync();

        if (orderAlbumToBeDeleted == null)
        {
            throw new ArgumentNullException();
        }

        Album? album = await _context.Albums
            .Where(a => a.Id == albumId)
            .FirstOrDefaultAsync();

        if (album == null)
        {
            throw new ArgumentNullException();
        }

        Order? currentOrder = await _context.Orders
            .Where(o => o.Id == orderId && o.BuyerId == buyerId && o.IsCompleted == false)
            .FirstOrDefaultAsync();

        if (currentOrder == null)
        {
            throw new ArgumentNullException();
        }

        currentOrder.TotalQuantity -= orderAlbumToBeDeleted.Quantity;
        currentOrder.TotalPrice -= orderAlbumToBeDeleted.Price;
        album.Stock += orderAlbumToBeDeleted.Quantity;

        if (album.Stock >= 1)
        {
            album.IsDeleted = false;
        }

        _context.OrdersAlbums.Remove(orderAlbumToBeDeleted);

        if (currentOrder.TotalQuantity == 0 && currentOrder.TotalPrice == 0)
        {
            _context.Orders.Remove(currentOrder);
        }

        await _context.SaveChangesAsync();
    }
}