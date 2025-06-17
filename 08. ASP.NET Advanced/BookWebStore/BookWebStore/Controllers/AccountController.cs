using System.Diagnostics;
using System.Text;
using System.Web;
using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        ApplicationUser? userUsernameCheck = await _userManager.FindByNameAsync(register.Username);
        ApplicationUser? userEmailCheck = await _userManager.FindByEmailAsync(register.Email);

        if (userUsernameCheck != null || userEmailCheck != null)
        {
            string errorMessage = "A user with this username has already been registered.";
            ModelState.Remove(nameof(register.Username));
            register.Username = string.Empty;
            ModelState.AddModelError(nameof(register.Username), errorMessage);
        }

        if (!ModelState.IsValid)
        {
            return View(register);
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

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(register);
        }

        await _userManager.AddToRoleAsync(user, "Guest");
        await _signInManager.SignInAsync(user, isPersistent: false);

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

        ApplicationUser? user = await _userManager.FindByNameAsync(login.UsernameOrEmail)
                             ?? await _userManager.FindByEmailAsync(login.UsernameOrEmail);

        if (user == null)
        {
            string errorMessage = "No user found with this username or email.";
            ModelState.Remove(nameof(login.UsernameOrEmail));
            login.UsernameOrEmail = string.Empty;
            ModelState.AddModelError(nameof(login.UsernameOrEmail), errorMessage);
        }
        else if (await _userManager.IsLockedOutAsync(user))
        {
            ModelState.AddModelError("", "Your account is locked due to multiple failed login attempts. Try again later.");
        }
        else
        {
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked. Please try again in 30 minutes.");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
            }
        }

        return View(login);
    }

    [HttpPost]
    [Authorize]
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
        ApplicationUser? user = await _userManager.FindByEmailAsync(forgotPassword.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "No user found with the specified email address.");
            ModelState.Remove(nameof(forgotPassword.Email));
            forgotPassword.Email = string.Empty;
        }

        if (!ModelState.IsValid)
        {
            return View(forgotPassword);
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string encodedEmail = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Email));
        string safeEncodedEmail = HttpUtility.UrlEncode(encodedEmail);
        string resetLink = Url.Action("ResetPassword", "Account", new { token, email = safeEncodedEmail }, Request.Scheme);

        string apiKey = _configuration["MailJet:ApiKey"];
        string apiSecret = _configuration["MailJet:ApiSecret"];
        string fromEmail = _configuration["MailJet:FromEmail"];
        string fromName = _configuration["MailJet:FromName"];

        using (HttpClient? client = new HttpClient())
        {
            StringContent? requestContent = new StringContent(
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
                            new JProperty("Subject", "Password Reset"),
                            new JProperty("HTMLPart", $"<p>Click <a href='{resetLink}'>here</a> to reset your password. The link is valid for 30 minutes.</p>"),
                            new JProperty("TextPart", $"To reset your password, open the following link: {resetLink}")
                        )
                    ))
                ).ToString(), Encoding.UTF8, "application/json"
            );

            client.DefaultRequestHeaders.Add("Authorization", "Basic " +
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}")));

            HttpResponseMessage? response = await client.PostAsync("https://api.mailjet.com/v3.1/send", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Failed to send password reset email. Please try again later.");
                ModelState.Remove(nameof(forgotPassword.Email));
                forgotPassword.Email = string.Empty;
                return View(forgotPassword);
            }
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
        ErrorViewModel errorModel = new ErrorViewModel();

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
        {
            errorModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            errorModel.ErrorMessage = "The password recovery link is invalid or missing required information.";

            return View("Error", errorModel);
        }

        try
        {
            //Decode Base64 email from URL
            byte[] emailBytes = Convert.FromBase64String(email);
            string decodedEmail = Encoding.UTF8.GetString(emailBytes);

            ResetPasswordViewModel model = new ResetPasswordViewModel
            {
                Token = token,
                Email = decodedEmail
            };

            return View(model);
        }
        catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
        {
            errorModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            errorModel.ErrorMessage = "The email address in the reset link is invalid.";

            return View("Error", errorModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", $"Reset password attempt for non-existing user with email: {model.Email}");
        }
        else if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            ModelState.AddModelError(nameof(model.Password), "The new password cannot be the same as the old password.");
        }
        else
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ResetPasswordConfirmation()
    {
        await _signInManager.SignOutAsync();
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Manage(string page = "Profile")
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return View("NotFound");
        }

        bool isMasterAdmin = User.IsInRole("Administrator") &&
                             user.Email == "kontakta39@mail.bg" &&
                             user.UserName == "kontakta39";

        if (!Request.Query.ContainsKey("page") && isMasterAdmin)
        {
            page = "ManageUsers";
        }

        ViewData["UserEmail"] = user.Email;
        ViewData["Username"] = user.UserName;
        ViewData["ActivePage"] = page;

        switch (page)
        {
            case "ManageUsers":
                if (!isMasterAdmin)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

                List<ApplicationUser> users = await _userManager.Users
                    .Where(u => u.Email != "kontakta39@mail.bg" && u.UserName != "kontakta39")
                    .ToListAsync();

                List<(ApplicationUser, IList<string>)>? userRoles = new List<(ApplicationUser, IList<string>)>();

                foreach (ApplicationUser u in users)
                {
                    IList<string>? roles = await _userManager.GetRolesAsync(u);
                    userRoles.Add((u, roles));
                }
                return View("Manage", userRoles);

            case "Profile":
                ProfileViewModel? profileModel = new ProfileViewModel
                {
                    Username = user.UserName!,
                    PhoneNumber = user.PhoneNumber
                };
                return View("Manage", profileModel);

            case "Email":
                EmailViewModel? emailModel = new EmailViewModel
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
    [Authorize]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel profileViewModel)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(profileViewModel.Username);

        if (user == null)
        {
            return View("NotFound");
        }

        ViewData["ActivePage"] = "Profile";

        if (!ModelState.IsValid)
        {
            return View("Manage", profileViewModel);
        }

        if (!string.IsNullOrWhiteSpace(profileViewModel.PhoneNumber))
        {
            bool phoneExists = await _context.Users
                .AnyAsync(u => u.PhoneNumber == profileViewModel.PhoneNumber);

            if (phoneExists)
            {
                string errorMessage = "Please enter a different phone number. This one is already associated with an account.";
                ModelState.Remove(nameof(profileViewModel.PhoneNumber));
                profileViewModel.PhoneNumber = null;
                ModelState.AddModelError(nameof(profileViewModel.PhoneNumber), errorMessage);
                return View("Manage", profileViewModel);
            }
        }
        else
        {
            ModelState.Remove(nameof(profileViewModel.PhoneNumber));
        }

        user.PhoneNumber = profileViewModel.PhoneNumber;
        await _context.SaveChangesAsync();

        return RedirectToAction("Manage", new { page = "Profile" });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeEmail(EmailViewModel emailViewModel)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(emailViewModel.CurrentEmail);

        if (user == null)
        {
            return View("NotFound");
        }

        ViewData["ActivePage"] = "Email";

        if (!ModelState.IsValid)
        {
            return View("Manage", emailViewModel);
        }

        if (!string.IsNullOrWhiteSpace(emailViewModel.NewEmail))
        {
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == emailViewModel.NewEmail);

            if (emailExists)
            {
                string errorMessage = "Please enter a different email. This one is already associated with an account.";
                ModelState.Remove(nameof(emailViewModel.NewEmail));
                emailViewModel.NewEmail = null;
                ModelState.AddModelError(nameof(emailViewModel.NewEmail), errorMessage);
                return View("Manage", emailViewModel);
            }
        }

        user.Email = emailViewModel.NewEmail;
        user.NormalizedEmail = emailViewModel.NewEmail.ToUpper();
        await _context.SaveChangesAsync();

        return RedirectToAction("Manage", new { page = "Email" });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passwordViewModel)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return View("NotFound");
        }

        ViewData["ActivePage"] = "ChangePassword";

        if (!ModelState.IsValid)
        {
            return View("Manage", passwordViewModel);
        }

        //Check if the entered old password is correct
        bool isOldPasswordValid = await _userManager.CheckPasswordAsync(user, passwordViewModel.OldPassword!);

        if (!isOldPasswordValid)
        {
            ModelState.AddModelError(nameof(passwordViewModel.OldPassword), "The current password is incorrect.");
        }
        else if (passwordViewModel.OldPassword == passwordViewModel.NewPassword)
        {
            ModelState.AddModelError("", "The new password must be different from the old one.");
        }
        else
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(user, passwordViewModel.OldPassword!, passwordViewModel.NewPassword!);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Manage", new { page = "ChangePassword" });
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View("Manage", passwordViewModel);
    }

    [HttpGet]
    [Authorize]
    public IActionResult DeletePersonalData()
    {
        DeletePersonalDataViewModel deleteAccount = new DeletePersonalDataViewModel();
        return View(deleteAccount);
    }

    [HttpPost]
    [Authorize]
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

        bool isEmailCorrect = string.Equals(currentUser.Email, deleteAccount.Email, StringComparison.OrdinalIgnoreCase);
        bool isPasswordCorrect = await _userManager.CheckPasswordAsync(currentUser, deleteAccount.Password);

        //Check if the entered user email address matches with the logged in user email address
        if (!isEmailCorrect)
        {
            string errorMessage = "The entered email does not match your account.";
            ModelState.Remove(nameof(deleteAccount.Email));
            deleteAccount.Email = string.Empty;
            ModelState.AddModelError(nameof(deleteAccount.Email), errorMessage);
        }
        else if (!isPasswordCorrect)
        {
            ModelState.AddModelError(nameof(deleteAccount.Password), "You have entered invalid password.");
        }
        else
        {
            IdentityResult result = await _userManager.DeleteAsync(currentUser);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(deleteAccount);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeRole(string userId, string role)
    {
        ApplicationUser? admin = await _userManager.GetUserAsync(User);
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        if (admin == null || user == null)
        {
            return View("NotFound");
        }

        if (!User.IsInRole("Administrator") || admin.Email != "kontakta39@mail.bg" || admin.UserName != "kontakta39")
        {
            return View("AccessDenied");
        }

        IList<string>? currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);

        return RedirectToAction("Manage", new { page = "ManageUsers" });
    }
}