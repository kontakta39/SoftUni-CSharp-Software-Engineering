using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data.Models;

namespace MusicWebStore.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ManageUsers()
    {
        List<ApplicationUser>? users = await _userManager.Users.ToListAsync();
        List<(ApplicationUser User, IList<string> Roles)>? userRoles = new List<(ApplicationUser User, IList<string> Roles)>();

        foreach (ApplicationUser user in users)
        {
            IList<string>? roles = await _userManager.GetRolesAsync(user);
            userRoles.Add((user, roles));
        }

        return View(userRoles);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeRole(string userId, string role)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        IList<string>? currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);

        return RedirectToAction(nameof(ManageUsers));
    }
}