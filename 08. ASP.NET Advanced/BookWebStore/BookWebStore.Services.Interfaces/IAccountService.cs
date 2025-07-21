using System.Security.Claims;
using BookWebStore.Data.Models;

namespace BookWebStore.Services.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal currentUser);
}