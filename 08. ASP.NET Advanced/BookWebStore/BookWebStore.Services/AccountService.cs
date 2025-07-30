using System.Security.Claims;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Services;

public class AccountService : IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBaseRepository _baseRepository;

    public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IBaseRepository baseRepository)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _baseRepository = baseRepository;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal currentUser)
    {
        return await _userManager.GetUserAsync(currentUser);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task AddUserToRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<string?> GetUserRoleAsync(ApplicationUser user)
    {
        return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
    }


    public async Task<(IdentityResult, ApplicationUser)> RegisterUserAsync(RegisterViewModel register)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = register.Username,
            Email = register.Email,
            FirstName = register.FirstName,
            LastName = register.LastName,
            EmailConfirmed = false
        };

        IdentityResult result = await _userManager.CreateAsync(user, register.Password);
        return (result, user);
    }

    public async Task SignInUserAsync(ApplicationUser user, bool isPersistent)
    {
        await _signInManager.SignInAsync(user, isPersistent);
    }

    public async Task<bool> IsUserLockedOutAsync(ApplicationUser user)
    {
        return await _userManager.IsLockedOutAsync(user);
    }

    public async Task<SignInResult> PasswordSignInUserAsync(ApplicationUser user, string password, bool isPersistent = false, bool lockoutOnFailure = true)
    {
        return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }

    public async Task ResetUserAccessFailedCountAsync(ApplicationUser user)
    {
        await _userManager.ResetAccessFailedCountAsync(user);
    }

    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<string> GenerateUserResetTokenAsync(ApplicationUser user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<bool> IsResetTokenValidAsync(ApplicationUser user, string token)
    {
        return await _userManager.VerifyUserTokenAsync(
            user,
            _userManager.Options.Tokens.PasswordResetTokenProvider,
            "ResetPassword",
            token);
    }

    public async Task<IdentityResult> ResetUserPasswordAsync(ApplicationUser user, string token, string newPassword)
    {
        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userManager.Users
            .Where(u => u.Email != "kontakta39@mail.bg" && u.UserName != "kontakta39")
            .ToListAsync();
    }

    public async Task<bool> UserPhoneNumberExistsAsync(string phoneNumber, string userId)
    {
        return await _userManager.Users.AnyAsync(u => u.Id != userId && u.PhoneNumber == phoneNumber);
    }

    public async Task UpdateUserProfile(string newPhoneNumber, ApplicationUser user)
    {
        user.PhoneNumber = newPhoneNumber;
        await _baseRepository.SaveChangesAsync();
    }

    public async Task<bool> UserEmailExistsAsync(string email, string userId)
    {
        return await _userManager.Users.AnyAsync(u => u.Id != userId && u.Email == email);
    }

    public async Task ChangeEmailAsync(EmailViewModel emailViewModel, ApplicationUser user)
    {
        user.Email = emailViewModel.NewEmail;
        user.NormalizedEmail = emailViewModel.NewEmail!.ToUpper();
        await _baseRepository.SaveChangesAsync();
    }

    public async Task<IdentityResult> ChangeUserPasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }

    public async Task RefreshUserSignInAsync(ApplicationUser user)
    {
        await _signInManager.RefreshSignInAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task RemoveUserFromRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.RemoveFromRoleAsync(user, role);
    }
}