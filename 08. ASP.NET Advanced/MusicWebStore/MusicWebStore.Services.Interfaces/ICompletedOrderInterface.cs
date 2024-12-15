using MusicWebStore.Data;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface ICompletedOrderInterface
{
    Task<List<CompletedOrderViewModel>> OrdersList(string buyerId);
    Task<Order?> CompleteOrder(Guid orderId, string buyerId);
    Task<OrderAlbum?> ReturnAlbum(Guid orderId, Guid albumId, string buyerId);
}