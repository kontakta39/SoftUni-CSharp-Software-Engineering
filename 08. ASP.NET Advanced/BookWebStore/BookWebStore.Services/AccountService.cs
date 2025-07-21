using System.Security.Claims;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookWebStore.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal currentUser)
    {
        return await _userManager.GetUserAsync(currentUser);
    }
}