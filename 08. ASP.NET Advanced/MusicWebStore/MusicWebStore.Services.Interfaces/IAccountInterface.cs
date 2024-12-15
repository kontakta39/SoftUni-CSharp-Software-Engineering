using Microsoft.AspNetCore.Identity;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;

namespace MusicWebStore.Services;

public interface IAccountInterface
{
    public Task<List<(ApplicationUser, IList<string>)>> ManageUsers(string userId);
    Task ChangeRole(ApplicationUser user, string role);
    Task<IdentityResult> Register(RegisterViewModel register);
    Task<(bool Success, string ErrorMessage)> LogIn(string email, string password);
}