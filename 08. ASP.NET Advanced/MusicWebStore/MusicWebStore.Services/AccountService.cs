using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;
using System.Data;

namespace MusicWebStore.Services;

public class AccountService : IAccountInterface
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<List<(ApplicationUser, IList<string>)>> ManageUsers(string userId)
    {
        List<ApplicationUser>? users = await _userManager.Users
        .Where(u => u.Email != "kontakta39@mail.bg" && u.Id != userId)
        .ToListAsync();

        if (users == null)
        {
            throw new ArgumentNullException();
        }

        List<(ApplicationUser User, IList<string> Roles)>? userRoles = new List<(ApplicationUser User, IList<string> Roles)>();

        foreach (ApplicationUser user in users)
        {
            IList<string>? roles = await _userManager.GetRolesAsync(user);
            userRoles.Add((user, roles));
        }

        return userRoles;
    }

    public async Task ChangeRole(ApplicationUser user, string role)
    {
        IList<string>? currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> Register(RegisterViewModel register)
    {
        ApplicationUser userCheck = await _userManager.FindByEmailAsync(register.Email);

        if (userCheck != null)
        {
            throw new ArgumentException("A user with this email has already been registered.");
        }

        ApplicationUser user = new ApplicationUser
        {
            UserName = register.Email,
            Email = register.Email,
            FirstName = register.FirstName,
            LastName = register.LastName,
            EmailConfirmed = false
        };

        IdentityResult result = await _userManager.CreateAsync(user, register.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Guest");
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        return result;
    }

    public async Task LogIn(string email, string password)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new ArgumentException("No user found with this email address.");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new ArgumentException("Your account is locked due to multiple failed login attempts. Try again later.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
        }
        else if (result.IsLockedOut)
        {
            throw new ArgumentException("Your account is locked. Please try again in 30 minutes.");
        }
        else
        {
            throw new ArgumentException("Invalid login attempt.");
        }
    }
}