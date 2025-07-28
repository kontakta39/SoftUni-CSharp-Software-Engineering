using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static BookWebStore.Constants.ModelConstants;

namespace BookWebStore.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;
    private readonly IOrderService _orderService;

    public AccountController(IConfiguration configuration, IAccountService accountService, IOrderService orderService)
    {
        _configuration = configuration;
        _accountService = accountService;
        _orderService = orderService;
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
        ApplicationUser? userUsernameCheck = await _accountService.GetUserByUsernameAsync(register.Username);
        ApplicationUser? userEmailCheck = await _accountService.GetUserByEmailAsync(register.Email);

        if (userUsernameCheck != null)
        {
            ModelState.AddModelError(nameof(register.Username), "A user with this username has already been registered.");
        }

        if (userEmailCheck != null)
        {
            ModelState.AddModelError(nameof(register.Email), "A user with this email has already been registered.");
        }

        if (!ModelState.IsValid)
        {
            return View(register);
        }

        (IdentityResult result, ApplicationUser user) = await _accountService.RegisterUserAsync(register);

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(register);
        }

        await _accountService.AddUserToRoleAsync(user, "Guest");
        await _accountService.SignInUserAsync(user, isPersistent: false);

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

        bool isEmail = Regex.IsMatch(login.UsernameOrEmail, LoginUsernameOrEmailCheck, RegexOptions.IgnoreCase);
        ApplicationUser? user = null;

        if (isEmail)
        {
            user = await _accountService.GetUserByEmailAsync(login.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(nameof(login.UsernameOrEmail), "No user found with this email");
                return View(login);
            }
        }
        else
        {
            user = await _accountService.GetUserByUsernameAsync(login.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(nameof(login.UsernameOrEmail), "No user found with this username");
                return View(login);
            }
        }

        if (await _accountService.IsUserLockedOutAsync(user))
        {
            ModelState.AddModelError("", "Your account is locked due to multiple failed login attempts. Try again later.");
        }
        else
        {
            var result = await _accountService.PasswordSignInUserAsync(user, login.Password);

            if (result.Succeeded)
            {
                await _accountService.ResetUserAccessFailedCountAsync(user);
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
        await _accountService.LogOutAsync();
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
        ApplicationUser? user = await _accountService.GetUserByEmailAsync(forgotPassword.Email);

        if (user == null)
        {
            ModelState.AddModelError(nameof(forgotPassword.Email), "No user found with the specified email address.");
        }

        if (!ModelState.IsValid)
        {
            return View(forgotPassword);
        }

        string token = await _accountService.GenerateUserResetTokenAsync(user);
        string encodedEmail = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Email!));
        string safeEncodedEmail = HttpUtility.UrlEncode(encodedEmail);
        string resetLink = Url.Action("ResetPassword", "Account", new { token, email = safeEncodedEmail }, Request.Scheme)!;

        string apiKey = _configuration["MailJet:ApiKey"]!;
        string apiSecret = _configuration["MailJet:ApiSecret"]!;
        string fromEmail = _configuration["MailJet:FromEmail"]!;
        string fromName = _configuration["MailJet:FromName"]!;

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
                            new JProperty("Subject", "Reset Password Link"),
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
                return View(forgotPassword);
            }
        }

        TempData["RedirectFromForgotPassword"] = true;
        return RedirectToAction("ForgotPasswordConfirmation", "Account");
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        bool isRedirected = (TempData["RedirectFromForgotPassword"] as bool?) ?? false;

        if (!isRedirected)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token, string email)
    {
        ErrorViewModel errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
        {
            errorViewModel.ErrorMessage = "The reset link is invalid or missing required information.";
            return View("Error", errorViewModel);
        }

        try
        {
            byte[] emailBytes = Convert.FromBase64String(email);
            string decodedEmail = Encoding.UTF8.GetString(emailBytes);

            ApplicationUser? user = await _accountService.GetUserByEmailAsync(decodedEmail);

            if (user == null)
            {
                errorViewModel.ErrorMessage = "No user is associated with this reset link.";
                return View("Error", errorViewModel);
            }

            bool isTokenValid = await _accountService.IsResetTokenValidAsync(user, token);

            if (!isTokenValid)
            {
                errorViewModel.ErrorMessage = "The reset link is invalid or has expired. Please request a new one to reset your password.";
                return View("Error", errorViewModel);
            }

            return View(new ResetPasswordViewModel
            {
                Token = token,
                Email = decodedEmail
            });
        }
        catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
        {
            errorViewModel.ErrorMessage = "The email address in the reset link is invalid.";
            return View("Error", errorViewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
    {
        ErrorViewModel errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        if (string.IsNullOrWhiteSpace(resetPassword.Token) || string.IsNullOrWhiteSpace(resetPassword.Email))
        {
            errorViewModel.ErrorMessage = "The reset link is invalid or missing required information.";
            return View("Error", errorViewModel);
        }

        ApplicationUser? user = null;

        try
        {
            byte[] emailBytes = Convert.FromBase64String(resetPassword.Email);
            string decodedEmail = Encoding.UTF8.GetString(emailBytes);

            user = await _accountService.GetUserByEmailAsync(decodedEmail);

            if (user == null)
            {
                errorViewModel.ErrorMessage = $"Reset password attempt for non-existing user with email: {resetPassword.Email}";
                return View("Error", errorViewModel);
            }

            bool isTokenValid = await _accountService.IsResetTokenValidAsync(user, resetPassword.Token);

            if (!isTokenValid)
            {
                errorViewModel.ErrorMessage = "The reset link is invalid or has expired. Please request a new one to reset your password.";
                return View("Error", errorViewModel);
            }

            resetPassword.Email = decodedEmail;
        }
        catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
        {
            errorViewModel.ErrorMessage = "The email address in the reset link is invalid.";
            return View("Error", errorViewModel);
        }

        if (!ModelState.IsValid)
        {
            return View(resetPassword);
        }

        if (await _accountService.CheckUserPasswordAsync(user, resetPassword.Password))
        {
            ModelState.AddModelError(nameof(resetPassword.Password), "The new password cannot be the same as the old password.");
        }
        else
        {
            IdentityResult result = await _accountService.ResetUserPasswordAsync(user, resetPassword.Token, resetPassword.Password);

            if (result.Succeeded)
            {
                TempData["RedirectFromResetPassword"] = true;
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(resetPassword);
    }

    [HttpGet]
    public async Task<IActionResult> ResetPasswordConfirmation()
    {
        bool isRedirected = (TempData["RedirectFromResetPassword"] as bool?) ?? false;

        if (!isRedirected)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        await _accountService.LogOutAsync();
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Manage(string page = "Profile")
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();  
        }

        bool isMasterAdmin = User.IsInRole("Administrator") &&
                             user.Email == "kontakta39@mail.bg" &&
                             user.UserName == "kontakta39";

        if (!Request.Query.ContainsKey("page") && isMasterAdmin)
        {
            page = "ManageUsers";
        }

        ViewData["ActivePage"] = page;

        switch (page)
        {
            case "ManageUsers":
                if (!isMasterAdmin)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

                List<ApplicationUser> users = await _accountService.GetAllUsersAsync();
                List<(ApplicationUser, string?)> userRoles = new List<(ApplicationUser, string?)>();

                foreach (ApplicationUser u in users)
                {
                    string? role = await _accountService.GetUserRoleAsync(u);
                    userRoles.Add((u, role));
                }
                return View("Manage", userRoles);

            case "Profile":
                ProfileViewModel? profileModel = new ProfileViewModel
                {
                    Username = user.UserName!,
                    CurrentPhoneNumber = user.PhoneNumber
                };
                return View("Manage", profileModel);

            case "Orders":
                List<OrderBook> orderedBooksList = await _orderService.GetCompletedOrdersByUserAsync(user.Id);

                List<CompletedOrderViewModel> userCompletedOrders = orderedBooksList
                .Select(ob => new CompletedOrderViewModel
                {
                    OrderId = ob.Order.Id,
                    BookId = ob.BookId,
                    OrderNumber = ob.Order.OrderNumber,
                    OrderDate = ob.Order.OrderDate,
                    Title = ob.Book.Title,
                    ImageUrl = ob.Book.ImageUrl!,
                    Quantity = ob.Quantity,
                    UnitPrice = ob.UnitPrice,
                    IsReturned = ob.IsReturned,
                    IsDeleted = ob.Book.IsDeleted
                })
                .ToList();
                return View("Manage", userCompletedOrders);

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
        ApplicationUser? user = await _accountService.GetUserByUsernameAsync(profileViewModel.Username);

        if (user == null)
        {
            return NotFound();
        }

        ViewData["ActivePage"] = "Profile";

        if (!ModelState.IsValid)
        {
            return View("Manage", profileViewModel);
        }

        if (!string.IsNullOrWhiteSpace(profileViewModel.NewPhoneNumber))
        {
            bool phoneExists = await _accountService.UserPhoneNumberExistsAsync(profileViewModel.NewPhoneNumber, user.Id);

            if (phoneExists)
            {
                ModelState.AddModelError(nameof(profileViewModel.NewPhoneNumber), "Please enter a different phone number. This one is already associated with an account.");
                return View("Manage", profileViewModel);
            }

            if (profileViewModel.CurrentPhoneNumber == profileViewModel.NewPhoneNumber)
            {
                ModelState.AddModelError(nameof(profileViewModel.NewPhoneNumber), "The new phone number must be different from the old one.");
                return View("Manage", profileViewModel);
            }
        }
        else
        {
            ModelState.AddModelError(nameof(profileViewModel.NewPhoneNumber), "You must enter a new phone number to update.");
            return View("Manage", profileViewModel);
        }
            
        await _accountService.UpdateUserProfile(profileViewModel, user);

        return RedirectToAction("Manage", new { page = "Profile" });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeEmail(EmailViewModel emailViewModel)
    {
        ApplicationUser? user = await _accountService.GetUserByEmailAsync(emailViewModel.CurrentEmail);

        if (user == null)
        {
            return NotFound();
        }

        ViewData["ActivePage"] = "Email";

        if (!ModelState.IsValid)
        {
            return View("Manage", emailViewModel);
        }

        if (!string.IsNullOrWhiteSpace(emailViewModel.NewEmail))
        {
            bool emailExists = await _accountService.UserEmailExistsAsync(emailViewModel.NewEmail, user.Id);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(emailViewModel.NewEmail), "Please enter a different email. This one is already associated with an account.");
                return View("Manage", emailViewModel);
            }

            if (emailViewModel.CurrentEmail == emailViewModel.NewEmail)
            {
                ModelState.AddModelError(nameof(emailViewModel.NewEmail), "The new email must be different from the old one.");
                return View("Manage", emailViewModel);
            }
        }
        else
        {
            ModelState.AddModelError(nameof(emailViewModel.NewEmail), "You must enter a new email to update.");
            return View("Manage", emailViewModel);
        }

        await _accountService.ChangeEmailAsync(emailViewModel, user);

        return RedirectToAction("Manage", new { page = "Email" });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passwordViewModel)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        ViewData["ActivePage"] = "ChangePassword";

        if (!ModelState.IsValid)
        {
            return View("Manage", passwordViewModel);
        }

        //Check if the entered old password is correct
        bool isOldPasswordValid = await _accountService.CheckUserPasswordAsync(user, passwordViewModel.OldPassword!);

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
            IdentityResult result = await _accountService.ChangeUserPasswordAsync(user, passwordViewModel.OldPassword!, passwordViewModel.NewPassword!);

            if (result.Succeeded)
            {
                await _accountService.RefreshUserSignInAsync(user);
                return RedirectToAction("Manage", new { page = "Profile" });
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
        ApplicationUser? currentUser = await _accountService.GetCurrentUserAsync(User); 

        if (currentUser == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(deleteAccount);
        }

        bool isEmailCorrect = string.Equals(currentUser.Email, deleteAccount.Email, StringComparison.OrdinalIgnoreCase);
        bool isPasswordCorrect = await _accountService.CheckUserPasswordAsync(currentUser, deleteAccount.Password);

        //Check if the entered user email address matches with the logged in user email address
        if (!isEmailCorrect)
        {
            ModelState.AddModelError(nameof(deleteAccount.Email), "The entered email does not match your account.");
        }
        else if (!isPasswordCorrect)
        {
            ModelState.AddModelError(nameof(deleteAccount.Password), "You have entered invalid password.");
        }
        else
        {
            IdentityResult result = await _accountService.DeleteUserAsync(currentUser);

            if (result.Succeeded)
            {
                await _accountService.LogOutAsync();
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
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeRole(string userId, string role)
    {
        ApplicationUser? admin = await _accountService.GetCurrentUserAsync(User);
        ApplicationUser? user = await _accountService.GetUserByIdAsync(userId);

        if (admin == null || user == null)
        {
            return NotFound();
        }

        if (!User.IsInRole("Administrator") || admin.Email != "kontakta39@mail.bg" || admin.UserName != "kontakta39")
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        string? currentRole = await _accountService.GetUserRoleAsync(user);

        if (currentRole != null)
        {
            await _accountService.RemoveUserFromRoleAsync(user, currentRole);
            await _accountService.AddUserToRoleAsync(user, role);
        }

        return RedirectToAction("Manage", new { page = "ManageUsers" });
    }
}