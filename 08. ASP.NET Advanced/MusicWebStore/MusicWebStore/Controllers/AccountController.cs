﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicWebStore.Data.Models;
using MusicWebStore.Services;
using MusicWebStore.ViewModels;
using System.Security.Claims;

namespace MusicWebStore.Controllers;

public class AccountController : Controller
{
    private readonly IAccountInterface _accountService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(IAccountInterface accountService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _accountService = accountService;
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

        List<(ApplicationUser User, IList<string> Roles)>? userRoles = await _accountService.ManageUsers(userId);

        if (userRoles == null)
        {
            return NotFound();
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

        await _accountService.ChangeRole(user, role);
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

        try
        {
            IdentityResult result = await _accountService.Register(register);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
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
            var (success, errorMessage) = await _accountService.LogIn(model.Email, model.Password);

            if (success)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = errorMessage;
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