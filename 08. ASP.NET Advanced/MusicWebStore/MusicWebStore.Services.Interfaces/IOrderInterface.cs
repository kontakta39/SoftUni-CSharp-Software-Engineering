using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IOrderInterface
{
    Task<List<OrderCartViewModel>> Cart(string buyerId);
    Task<bool> AddToCart(Guid albumId, string buyerId);
    Task<(bool, string?, decimal, int)> UpdateQuantity(Guid orderId, Guid albumId, int quantity);
    Task RemoveFromCart(Guid orderId, Guid albumId, string buyerId);
}