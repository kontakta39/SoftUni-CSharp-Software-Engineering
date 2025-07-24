using System.Security.Claims;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BookWebStore.Services.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal currentUser);

    Task<ApplicationUser?> GetUserByIdAsync(string id);

    Task<ApplicationUser?> GetUserByUsernameAsync(string username);

    Task<ApplicationUser?> GetUserByEmailAsync(string email);

    Task AddUserToRoleAsync(ApplicationUser user, string role);

    Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password);

    Task<string?> GetUserRoleAsync(ApplicationUser user);



    Task<(IdentityResult, ApplicationUser)> RegisterUserAsync(RegisterViewModel register);

    Task SignInUserAsync(ApplicationUser user, bool isPersistent);

    Task<bool> IsUserLockedOutAsync(ApplicationUser user);

    Task<SignInResult> PasswordSignInUserAsync(ApplicationUser user, string password, bool isPersistent = false, bool lockoutOnFailure = true);

    Task ResetUserAccessFailedCountAsync(ApplicationUser user);

    Task LogOutAsync();

    Task<string> GenerateUserResetTokenAsync(ApplicationUser user);

    Task<bool> IsResetTokenValidAsync(ApplicationUser user, string token);

    Task<IdentityResult> ResetUserPasswordAsync(ApplicationUser user, string token, string newPassword);

    Task<List<ApplicationUser>> GetAllUsersAsync();

    Task<bool> UserPhoneNumberExistsAsync(string phoneNumber, string userId);

    Task UpdateUserProfile(ProfileViewModel profileViewModel, ApplicationUser user);

    Task<bool> UserEmailExistsAsync(string email, string userId);

    Task ChangeEmailAsync(EmailViewModel emailViewModel, ApplicationUser user);

    Task<IdentityResult> ChangeUserPasswordAsync(ApplicationUser user, string oldPassword, string newPassword);

    Task RefreshUserSignInAsync(ApplicationUser user);

    Task<IdentityResult> DeleteUserAsync(ApplicationUser user);

    Task RemoveUserFromRoleAsync(ApplicationUser user, string role);
}