using System.Text;
using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BookWebStore.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly BookStoreDbContext _context;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, BookStoreDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
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
            ApplicationUser? user = await _userManager.FindByNameAsync(login.UsernameOrEmail)
                                 ?? await _userManager.FindByEmailAsync(login.UsernameOrEmail);

            if (user == null)
            {
                ModelState.Remove(nameof(login.UsernameOrEmail));
                login.UsernameOrEmail = string.Empty;
                throw new ArgumentException("No user found with this username or email.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new ArgumentException("Your account is locked due to multiple failed login attempts Try again later.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, isPersistent: false, lockoutOnFailure: true);

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
            return View(login);
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

        try
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                throw new ArgumentException($"Password reset attempt for non-existing user with email: {forgotPassword.Email}");
            }

            //Generate password token for password reset
            string? token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string? resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            //Receiving the SMTP settings from appsettings.json
            string apiKey = _configuration["MailJet:ApiKey"];
            string apiSecret = _configuration["MailJet:ApiSecret"];
            string fromEmail = _configuration["MailJet:FromEmail"];
            string fromName = _configuration["MailJet:FromName"];

            //Creating the HTTP client for MailJet
            using (var client = new HttpClient())
            {
                var requestContent = new StringContent(
                    new JObject(
                        new JProperty("Messages", new JArray(
                            new JObject(
                                new JProperty("From", new JObject(
                                    new JProperty("Email", fromEmail),
                                    new JProperty("Name", fromName)
                                )),
                                new JProperty("To", new JArray(
                                    new JObject(
                                        new JProperty("Email", forgotPassword.Email),
                                        new JProperty("Name", user.UserName)
                                    )
                                )),
                                new JProperty("Subject", "Reset your password"),
                                new JProperty("HTMLPart", $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>"),
                                new JProperty("TextPart", "To reset your password, click the link in the email.")
                            )
                        ))
                    ).ToString(), Encoding.UTF8, "application/json"
                );

                //Adding the API key for authorization
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}")));

                //Sending the email
                var response = await client.PostAsync("https://api.mailjet.com/v3.1/send", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to send email. Status: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                }
            }
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ModelState.Remove(nameof(forgotPassword.Email));
            forgotPassword.Email = "";
            return View(forgotPassword);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Failed to send password reset email: {ex.Message}");
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
            return View(model);
        }

        try
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new ArgumentException($"Reset password attempt for non-existing user with email: {model.Email}");
            }

            // Check if the new password is the same as the old one
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new ArgumentException("The new password cannot be the same as the old password.");
            }

            //Reset the password using the token
            IdentityResult? result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> ResetPasswordConfirmation()
    {
        await _signInManager.SignOutAsync();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Manage(string page = "Profile")
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return View("NotFound");
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
                    CurrentEmail = user.Email!
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

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel profileViewModel)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(profileViewModel.Username);

        if (profileViewModel == null)
        {
            return View("NotFound");
        }

        if (!ModelState.IsValid)
        {
            ViewData["ActivePage"] = "Profile";
            return View("Manage", profileViewModel);
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(profileViewModel.PhoneNumber))
            {
                bool phoneExists = await _context.Users
                    .AnyAsync(u => u.PhoneNumber == profileViewModel.PhoneNumber);

                if (phoneExists)
                {
                    throw new ArgumentException("The phone number is already in use.");
                }
            }

            user.PhoneNumber = profileViewModel.PhoneNumber;
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ModelState.Remove(nameof(profileViewModel.PhoneNumber));
            profileViewModel.PhoneNumber = user.PhoneNumber;
            ViewData["ActivePage"] = "Profile";
            return View("Manage", profileViewModel);
        }

        return RedirectToAction("Manage", new { page = "Profile" });
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmail(EmailViewModel emailViewModel)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(emailViewModel.CurrentEmail);

        if (user == null)
        {
            return View("NotFound");
        }

        if (!ModelState.IsValid)
        {
            ViewData["ActivePage"] = "Email";
            return View("Manage", emailViewModel);
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(emailViewModel.NewEmail))
            {
                bool emailExists = await _context.Users
                    .AnyAsync(u => u.Email == emailViewModel.NewEmail);

                if (emailExists)
                {
                    throw new ArgumentException("The email is already in use.");
                }
            }

            user.Email = emailViewModel.NewEmail;
            user.NormalizedEmail = emailViewModel.NewEmail.ToUpper();
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ModelState.Remove(nameof(emailViewModel.NewEmail));
            emailViewModel.NewEmail = "";
            ViewData["ActivePage"] = "Email";
            return View("Manage", emailViewModel);
        }

        return RedirectToAction("Manage", new { page = "Email" });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passwordViewModel)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return View("NotFound");
        }

        if (!ModelState.IsValid)
        {
            ViewData["ActivePage"] = "ChangePassword";
            return View("Manage", passwordViewModel);
        }

        try
        {
            //Check if the entered old password is correct
            bool isOldPasswordValid = await _userManager.CheckPasswordAsync(user, passwordViewModel.OldPassword!);

            if (!isOldPasswordValid)
            {
                throw new ArgumentException("The current password is incorrect.", nameof(passwordViewModel.OldPassword));
            }

            if (passwordViewModel.OldPassword == passwordViewModel.NewPassword)
            {
                throw new ArgumentException("The new password must be different from the old one.", nameof(passwordViewModel.NewPassword));
            }

            IdentityResult result;

            result = await _userManager.ChangePasswordAsync(user, passwordViewModel.OldPassword!, passwordViewModel.NewPassword!);
        }
        catch (ArgumentException ex)
        {
            string[] messageSplit = ex.Message.Split(" (Parameter").ToArray();
            string key = string.IsNullOrEmpty(ex.ParamName) ? "" : ex.ParamName;
            ModelState.AddModelError(key, messageSplit[0]);
            ViewData["ActivePage"] = "ChangePassword";
            return View("Manage", passwordViewModel);
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction("Manage", new { page = "ChangePassword" });
    }

    [HttpGet]
    public IActionResult DeletePersonalData()
    {
        DeletePersonalDataViewModel deleteAccount = new DeletePersonalDataViewModel();
        return View(deleteAccount);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePersonalData(DeletePersonalDataViewModel deleteAccount)
    {
        ApplicationUser? currentUser = await _userManager.GetUserAsync(User); 

        if (currentUser == null)
        {
            return View("NotFound");
        }

        if (!ModelState.IsValid)
        {
            return View(deleteAccount);
        }

        try
        {
            //Check if the entered user email address matches with the logged in user email address
            if (!string.Equals(currentUser.Email, deleteAccount.Email, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.Remove(nameof(currentUser.Email));
                deleteAccount.Email = "";
                throw new ArgumentException("The entered email does not match your account.");
            }

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(currentUser, deleteAccount.Password);

            if (!isPasswordCorrect)
            {
                throw new ArgumentException("You have entered invalid password.");
            }

            IdentityResult? result = await _userManager.DeleteAsync(currentUser);
            await _signInManager.SignOutAsync();

        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(deleteAccount);
        }

        return RedirectToAction("Index", "Home");
    }
}