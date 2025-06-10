using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace BookWebStore.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
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

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
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
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    throw new ArgumentException("No user found with this email address.");
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    throw new ArgumentException("Your account is locked due to multiple failed login attempts. Try again later.");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);
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
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
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

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        ForgotPasswordViewModel forgotPassword = new ForgotPasswordViewModel();
        return View(forgotPassword);
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(forgotPassword);
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(forgotPassword.Email);
        if (user == null)
        {
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }

        string? token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string? resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

        //Reading SMTP settings from appsettings.json
        string? smtpServer = _configuration["EmailSettings:SmtpServer"];
        int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
        string? smtpUser = _configuration["EmailSettings:SmtpUser"];
        string? smtpPass = _configuration["EmailSettings:SmtpPass"];
        bool enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

        using SmtpClient? client = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = "Reset Password",
            Body = $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>",
            IsBodyHtml = true
        };

        mailMessage.To.Add(forgotPassword.Email);
        await client.SendMailAsync(mailMessage);

        return RedirectToAction("ForgotPasswordConfirmation", "Account");
    }

    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }
}