using BookWebStore.Data.Models;

namespace BookWebStore.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<List<ApplicationUser>> GetAllNonAdminUsersAsync();

    Task<bool> UserPropertyExistsAsync(string propertyName, string nameKey, string userId);
}