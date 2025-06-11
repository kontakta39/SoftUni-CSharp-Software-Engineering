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
    private readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
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
            ApplicationUser? userUsernameCheck = await _userManager.FindByNameAsync(register.Username);
            ApplicationUser? userEmailCheck = await _userManager.FindByEmailAsync(register.Email);

            if (userUsernameCheck != null)
            {
                ModelState.Remove("Username");
                register.Username = "";
                throw new ArgumentException("A user with this username has already been registered.");
            }

            if (userEmailCheck != null)
            {
                ModelState.Remove("Email");
                register.Email = "";
                throw new ArgumentException("A user with this email has already been registered.");
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = register.Username,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                EmailConfirmed = false
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(user, "Guest");
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(register);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        LoginViewModel login = new LoginViewModel();
        return View(login);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(LoginViewModel login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        try
        {
            ApplicationUser? userUsernameCheck = await _userManager.FindByNameAsync(login.Username);

            if (userUsernameCheck == null)
            {
                throw new ArgumentException("No user found with this username.");
            }

            if (await _userManager.IsLockedOutAsync(userUsernameCheck))
            {
                throw new ArgumentException("Your account is locked due to multiple failed login attempts Try again later.");
            }

            var result = await _signInManager.PasswordSignInAsync(userUsernameCheck, login.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(userUsernameCheck);
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

        return RedirectToAction("Index", "Home");
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
            _logger.LogInformation($"Password reset attempt for non-existing user with email: {forgotPassword.Email}");
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }

        string? token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string? resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

        try
        {
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

            MailMessage? mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = "Reset Password",
                Body = $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(forgotPassword.Email);
            await client.SendMailAsync(mailMessage);

            _logger.LogInformation($"Password reset email sent successfully to {forgotPassword.Email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email.");
            ModelState.AddModelError(string.Empty, "There was an error sending the password reset email.");
            return View(forgotPassword);
        }

        return RedirectToAction("ForgotPasswordConfirmation", "Account");
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Invalid password reset token.");
        }

        ResetPasswordViewModel? model = new ResetPasswordViewModel
        {
            Token = token,
            Email = email
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            //Log that the model validation failed
            _logger.LogWarning("Reset password model validation failed for user: {Email}", model.Email);
            return View(model);
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("Reset password attempt for non-existing user with email: {Email}", model.Email);
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }

        //Reset the password using the token
        IdentityResult? result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Password reset successfully for user: {Email}", model.Email);
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }

        //If there are errors during password reset, log them
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
            _logger.LogError("Error resetting password for user {Email}: {ErrorDescription}", model.Email, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Manage(string page = "Profile")
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound("User cannot be found.");
        }

        ViewData["ActivePage"] = page;

        switch (page)
        {
            case "Profile":
                ProfileViewModel profileModel = new ProfileViewModel
                {
                    Username = user.UserName!,
                    PhoneNumber = user.PhoneNumber
                };

                return View("Manage", profileModel);
            case "Email":
                EmailViewModel emailModel = new EmailViewModel
                {
                    CurrentEmail = user.Email
                };

                return View("Manage", emailModel);
            case "ChangePassword":
                return View("Manage", new ChangePasswordViewModel());
            case "DeleteAccount":
                return View("Manage");
            default:
                return View("Manage");
        }
    }

    [HttpGet]
    public IActionResult DeletePersonalData()
    {
        return View();
    }
}