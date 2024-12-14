using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data.Models;
using MusicWebStore.ViewModels;
using System.Security.Claims;

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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ManageUsers()
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        if (userId == null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        List<ApplicationUser>? users = await _userManager.Users
            .Where(u => u.Email != "kontakta39@mail.bg" && u.Id != userId)
            .ToListAsync();

        List<(ApplicationUser User, IList<string> Roles)>? userRoles = new List<(ApplicationUser User, IList<string> Roles)>();

        foreach (ApplicationUser user in users)
        {
            IList<string>? roles = await _userManager.GetRolesAsync(user);
            userRoles.Add((user, roles));
        }

        return View(userRoles);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeRole(string userId, string role)
    {
        if (!User.IsInRole("Administrator"))
        {
            return RedirectToAction("AccessDenied", "Home");
        }

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

    [HttpGet]
    public IActionResult Register()
    {
        RegisterViewModel register = new RegisterViewModel();
        return View(register);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        ApplicationUser? userCheck = await _userManager.FindByEmailAsync(register.Email);

        if (userCheck != null)
        {
            //Clear the email field and ModelState
            ModelState.Remove(nameof(register.Email));
            register.Email = string.Empty;

            ViewData["ErrorMessage"] = "There is already registered user with this email.";
            return View(register);
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
            //Adding a Guest role to the newly registered user
            await _userManager.AddToRoleAsync(user, "Guest");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(register);
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        LoginViewModel login = new LoginViewModel();
        return View(login);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "No user found with this email address.";
                return View(model);
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                ViewData["ErrorMessage"] = "Your account is locked due to multiple failed login attempts. Try again later.";
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user); // Reset failed attempts on successful login
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsLockedOut)
            {
                ViewData["ErrorMessage"] = "Your account is locked. Please try again in 30 minutes.";
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid login attempt.";
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}