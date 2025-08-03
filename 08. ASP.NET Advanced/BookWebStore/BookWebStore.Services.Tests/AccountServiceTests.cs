using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace BookWebStore.Services.Tests;

[TestFixture]
public class AccountServiceTests
{
    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    private Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private Mock<IAccountRepository> _mockAccountRepository;
    private Mock<IBaseRepository> _mockBaseRepository;
    private AccountService _accountService;

    [SetUp]
    public void SetUp()
    {
        //Mock UserStore - needed for UserManager
        Mock<IUserStore<ApplicationUser>>? mockUserStore = new Mock<IUserStore<ApplicationUser>>();

        //Mock Options for Identity
        Mock<IOptions<IdentityOptions>>? options = new Mock<IOptions<IdentityOptions>>();
        options.Setup(o => o.Value).Returns(new IdentityOptions
        {
            Tokens = new TokenOptions
            {
                PasswordResetTokenProvider = "Default"
            }
        });

        //Mock PasswordHasher
        Mock<IPasswordHasher<ApplicationUser>>? passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();

        //Mock Validations
        List<IUserValidator<ApplicationUser>>? userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
        List<IPasswordValidator<ApplicationUser>>? passwordValidators = new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() };

        //Mock KeyNormalizer
        Mock<ILookupNormalizer>? keyNormalizer = new Mock<ILookupNormalizer>();

        //IdentityErrorDescriber - real
        IdentityErrorDescriber? errorDescriber = new IdentityErrorDescriber();

        //Mock ServiceProvider (can be empty)
        Mock<IServiceProvider>? serviceProvider = new Mock<IServiceProvider>();

        //Mock Logger for UserManager
        Mock<ILogger<UserManager<ApplicationUser>>>? loggerUserManager = new Mock<ILogger<UserManager<ApplicationUser>>>();

        //Initializing UserManager with mocks
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            mockUserStore.Object,
            options.Object,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            keyNormalizer.Object,
            errorDescriber,
            serviceProvider.Object,
            loggerUserManager.Object)
        { CallBase = true };

        //Mock IHttpContextAccessor for SignInManager
        Mock<IHttpContextAccessor>? mockContextAccessor = new Mock<IHttpContextAccessor>();
        Mock<HttpContext>? mockHttpContext = new Mock<HttpContext>();
        mockContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        //Mock IAuthenticationSchemeProvider
        Mock<IAuthenticationSchemeProvider>? mockSchemes = new Mock<IAuthenticationSchemeProvider>();

        //Mock IUserClaimsPrincipalFactory<ApplicationUser>
        Mock<IUserClaimsPrincipalFactory<ApplicationUser>>? mockUserPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

        //Mock Logger for SignInManager
        Mock<ILogger<SignInManager<ApplicationUser>>>? loggerSignInManager = new Mock<ILogger<SignInManager<ApplicationUser>>>();

        //Initializing SignInManager
        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            mockContextAccessor.Object,
            mockUserPrincipalFactory.Object,
            options.Object,
            loggerSignInManager.Object,
            mockSchemes.Object)
        { CallBase = true };

        _mockAccountRepository = new Mock<IAccountRepository>();
        _mockBaseRepository = new Mock<IBaseRepository>();

        _accountService = new AccountService(
            _mockSignInManager.Object,
            _mockUserManager.Object,
            _mockAccountRepository.Object,
            _mockBaseRepository.Object);
    }

    [Test]
    public async Task GetCurrentUserAsync_ValidClaimsPrincipal_ReturnsUser()
    {
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
        ApplicationUser expectedUser = new ApplicationUser();

        _mockUserManager
            .Setup(um => um.GetUserAsync(claimsPrincipal))
            .ReturnsAsync(expectedUser);

        ApplicationUser? actualUser = await _accountService.GetCurrentUserAsync(claimsPrincipal);

        Assert.Multiple(() =>
        {
            Assert.That(actualUser, Is.Not.Null);
            Assert.That(actualUser, Is.EqualTo(expectedUser));
        });
    }

    [Test]
    public async Task GetCurrentUserAsync_InvalidClaimsPrincipal_ReturnsNull()
    {
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

        _mockUserManager
            .Setup(um => um.GetUserAsync(claimsPrincipal))
            .ReturnsAsync((ApplicationUser?)null);

        ApplicationUser? actualUser = await _accountService.GetCurrentUserAsync(claimsPrincipal);

        Assert.That(actualUser, Is.Null);
    }

    [Test]
    public async Task GetUserByIdAsync_ValidId_ReturnsUser()
    {
        string userId = "user-123";

        ApplicationUser expectedUser = new ApplicationUser 
        { 
            Id = userId
        };

        _mockUserManager
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        ApplicationUser? actualUser = await _accountService.GetUserByIdAsync(userId);

        Assert.Multiple(() =>
        {
            Assert.That(actualUser, Is.Not.Null);
            Assert.That(actualUser!.Id, Is.EqualTo(expectedUser.Id));
        });
    }

    [Test]
    public async Task GetUserByIdAsync_InvalidId_ReturnsNull()
    {
        string userId = "invalid-id";

        _mockUserManager
            .Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser?)null);

        ApplicationUser? actualUser = await _accountService.GetUserByIdAsync(userId);

        Assert.That(actualUser, Is.Null);
    }

    [Test]
    public async Task GetUserByUsernameAsync_ValidUsername_ReturnsUser()
    {
        string username = "testuser";

        ApplicationUser expectedUser = new ApplicationUser 
        {
            UserName = username
        };

        _mockUserManager
            .Setup(um => um.FindByNameAsync(username))
            .ReturnsAsync(expectedUser);

        ApplicationUser? actualUser = await _accountService.GetUserByUsernameAsync(username);

        Assert.Multiple(() =>
        {
            Assert.That(actualUser, Is.Not.Null);
            Assert.That(actualUser!.UserName, Is.EqualTo(expectedUser.UserName));
        });
    }

    [Test]
    public async Task GetUserByUsernameAsync_InvalidUsername_ReturnsNull()
    {
        string username = "nonexistentuser";

        _mockUserManager
            .Setup(um => um.FindByNameAsync(username))
            .ReturnsAsync((ApplicationUser?)null);

        ApplicationUser? actualUser = await _accountService.GetUserByUsernameAsync(username);

        Assert.That(actualUser, Is.Null);
    }

    [Test]
    public async Task GetUserByEmailAsync_ValidEmail_ReturnsUser()
    {
        string email = "test@example.com";

        ApplicationUser expectedUser = new ApplicationUser 
        {
            Email = email 
        };

        _mockUserManager
            .Setup(um => um.FindByEmailAsync(email))
            .ReturnsAsync(expectedUser);

        ApplicationUser? actualUser = await _accountService.GetUserByEmailAsync(email);

        Assert.Multiple(() =>
        {
            Assert.That(actualUser, Is.Not.Null);
            Assert.That(actualUser!.Email, Is.EqualTo(expectedUser.Email));
        });
    }

    [Test]
    public async Task GetUserByEmailAsync_InvalidEmail_ReturnsNull()
    {
        string email = "nonexistent@example.com";

        _mockUserManager
            .Setup(um => um.FindByEmailAsync(email))
            .ReturnsAsync((ApplicationUser?)null);

        ApplicationUser? actualUser = await _accountService.GetUserByEmailAsync(email);

        Assert.That(actualUser, Is.Null);
    }

    [Test]
    public async Task AddUserToRoleAsync_ValidUserAndRole_CallsAddToRoleAsync()
    {
        ApplicationUser user = new ApplicationUser();
        string role = "Administrator";

        _mockUserManager
            .Setup(um => um.AddToRoleAsync(user, role))
            .ReturnsAsync(IdentityResult.Success);

        await _accountService.AddUserToRoleAsync(user, role);

        _mockUserManager.Verify(um => um.AddToRoleAsync(user, role), Times.Once);
    }

    [Test]
    public async Task CheckUserPasswordAsync_ValidPassword_ReturnsTrue()
    {
        ApplicationUser user = new ApplicationUser();
        string password = "correct_password";

        _mockUserManager
            .Setup(um => um.CheckPasswordAsync(user, password))
            .ReturnsAsync(true);

        bool result = await _accountService.CheckUserPasswordAsync(user, password);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task CheckUserPasswordAsync_InvalidPassword_ReturnsFalse()
    {
        ApplicationUser user = new ApplicationUser();
        string password = "wrong_password";

        _mockUserManager
            .Setup(um => um.CheckPasswordAsync(user, password))
            .ReturnsAsync(false);

        bool result = await _accountService.CheckUserPasswordAsync(user, password);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetUserRoleAsync_UserHasRoles_ReturnsFirstRole()
    {
        ApplicationUser user = new ApplicationUser();
        List<string> roles = new List<string> 
        { 
            "Administrator"
        };

        _mockUserManager
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(roles);

        string? result = await _accountService.GetUserRoleAsync(user);

        Assert.That(result, Is.EqualTo(roles[0]));
    }

    [Test]
    public async Task GetUserRoleAsync_UserHasNoRoles_ReturnsNull()
    {
        ApplicationUser user = new ApplicationUser();
        List<string> roles = new List<string>();

        _mockUserManager
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(roles);

        string? result = await _accountService.GetUserRoleAsync(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task RegisterUserAsync_ValidData_ReturnsSuccessAndUser()
    {
        RegisterViewModel register = new RegisterViewModel
        {
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = "Password123!"
        };

        _mockUserManager
            .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), register.Password))
            .ReturnsAsync(IdentityResult.Success);

        (IdentityResult result, ApplicationUser? user) = await _accountService.RegisterUserAsync(register);

        Assert.That(result.Succeeded, Is.True);

        _mockUserManager.Verify(um => um.CreateAsync(
            It.Is<ApplicationUser>(u =>
                u.UserName == register.Username &&
                u.Email == register.Email &&
                u.FirstName == register.FirstName &&
                u.LastName == register.LastName &&
                u.EmailConfirmed == false
            ),
            register.Password), Times.Once);
    }

    [Test]
    public async Task RegisterUserAsync_InvalidData_ReturnsFailure()
    {
        RegisterViewModel register = new RegisterViewModel
        {
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = "Password123!"
        };

        IdentityResult failedResult = IdentityResult.Failed(new IdentityError 
        { 
            Code = "DuplicateUser", 
            Description = "User already exists." 
        });

        _mockUserManager
            .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), register.Password))
            .ReturnsAsync(failedResult);

        (IdentityResult result, ApplicationUser? user) = await _accountService.RegisterUserAsync(register);

        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Has.Exactly(1).Items);
            Assert.That(result.Errors.Any(e => e.Code == "DuplicateUser"), Is.True);
            Assert.That(result.Errors.Any(e => e.Description == "User already exists."), Is.True);
        });
    }

    [Test]
    public async Task SignInUserAsync_ValidUser_CallsSignInAsync()
    {
        ApplicationUser user = new ApplicationUser();
        bool isPersistent = true;

        _mockSignInManager
            .Setup(sm => sm.SignInAsync(user, isPersistent, null))
            .Returns(Task.CompletedTask);

        await _accountService.SignInUserAsync(user, isPersistent);

        _mockSignInManager.Verify(sm => sm.SignInAsync(user, isPersistent, null), Times.Once);
    }

    [Test]
    public async Task IsUserLockedOutAsync_UserIsLockedOut_ReturnsTrue()
    {
        ApplicationUser user = new ApplicationUser();

        _mockUserManager
            .Setup(um => um.IsLockedOutAsync(user))
            .ReturnsAsync(true);

        bool result = await _accountService.IsUserLockedOutAsync(user);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IsUserLockedOutAsync_UserIsNotLockedOut_ReturnsFalse()
    {
        ApplicationUser user = new ApplicationUser();

        _mockUserManager
            .Setup(um => um.IsLockedOutAsync(user))
            .ReturnsAsync(false);

        bool result = await _accountService.IsUserLockedOutAsync(user);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task PasswordSignInUserAsync_ValidCredentials_ReturnsSuccess()
    {
        ApplicationUser user = new ApplicationUser();
        string password = "correct_password";

        _mockSignInManager
            .Setup(sm => sm.PasswordSignInAsync(user, password, false, true))
            .ReturnsAsync(SignInResult.Success);

        SignInResult result = await _accountService.PasswordSignInUserAsync(user, password);

        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task PasswordSignInUserAsync_InvalidCredentials_ReturnsFailed()
    {
        ApplicationUser user = new ApplicationUser();
        string password = "wrong_password";

        _mockSignInManager
            .Setup(sm => sm.PasswordSignInAsync(user, password, false, true))
            .ReturnsAsync(SignInResult.Failed);

        SignInResult result = await _accountService.PasswordSignInUserAsync(user, password);

        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public async Task ResetUserAccessFailedCountAsync_ValidUser_CallsResetAccessFailedCountAsync()
    {
        ApplicationUser user = new ApplicationUser();

        _mockUserManager
            .Setup(um => um.ResetAccessFailedCountAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _accountService.ResetUserAccessFailedCountAsync(user);

        _mockUserManager.Verify(um => um.ResetAccessFailedCountAsync(user), Times.Once);
    }

    [Test]
    public async Task LogOutAsync_CallsSignOutAsync()
    {
        _mockSignInManager
            .Setup(sm => sm.SignOutAsync())
            .Returns(Task.CompletedTask);

        await _accountService.LogOutAsync();

        _mockSignInManager.Verify(sm => sm.SignOutAsync(), Times.Once);
    }

    [Test]
    public async Task GenerateUserResetTokenAsync_ValidUser_ReturnsToken()
    {
        ApplicationUser user = new ApplicationUser();
        string expectedToken = "reset-token";

        _mockUserManager
            .Setup(um => um.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(expectedToken);

        string actualToken = await _accountService.GenerateUserResetTokenAsync(user);

        Assert.That(actualToken, Is.EqualTo(expectedToken));
    }

    [Test]
    public async Task IsResetTokenValidAsync_ValidToken_ReturnsTrue()
    {
        ApplicationUser user = new ApplicationUser();
        string token = "valid-token";
        string tokenProvider = _mockUserManager.Object.Options.Tokens.PasswordResetTokenProvider;

        _mockUserManager
            .Setup(um => um.VerifyUserTokenAsync(user, tokenProvider, "ResetPassword", token))
            .ReturnsAsync(true);

        bool result = await _accountService.IsResetTokenValidAsync(user, token);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IsResetTokenValidAsync_InvalidToken_ReturnsFalse()
    {
        ApplicationUser user = new ApplicationUser();
        string token = "invalid-token";
        string tokenProvider = _mockUserManager.Object.Options.Tokens.PasswordResetTokenProvider;

        _mockUserManager
            .Setup(um => um.VerifyUserTokenAsync(user, tokenProvider, "ResetPassword", token))
            .ReturnsAsync(false);

        bool result = await _accountService.IsResetTokenValidAsync(user, token);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task ResetUserPasswordAsync_ValidTokenAndPassword_ReturnsSuccess()
    {
        ApplicationUser user = new ApplicationUser();
        string token = "valid-token";
        string newPassword = "NewPassword123!";

        _mockUserManager
            .Setup(um => um.ResetPasswordAsync(user, token, newPassword))
            .ReturnsAsync(IdentityResult.Success);

        IdentityResult result = await _accountService.ResetUserPasswordAsync(user, token, newPassword);

        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task ResetUserPasswordAsync_InvalidTokenOrPassword_ReturnsFailure()
    {
        ApplicationUser user = new ApplicationUser();
        string token = "invalid-token";
        string newPassword = "weak";

        IdentityResult failedResult = IdentityResult.Failed(
            new IdentityError 
            { 
                Code = "InvalidToken", 
                Description = "The token is invalid." 
            }
        );

        _mockUserManager
            .Setup(um => um.ResetPasswordAsync(user, token, newPassword))
            .ReturnsAsync(failedResult);

        IdentityResult result = await _accountService.ResetUserPasswordAsync(user, token, newPassword);

        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors.Any(e => e.Code == "InvalidToken"), Is.True);
            Assert.That(result.Errors.Any(e => e.Description == "The token is invalid."), Is.True);

        });
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsAllNonAdminUsers()
    {
        List<ApplicationUser> users = new List<ApplicationUser>
        {
            new ApplicationUser(),
            new ApplicationUser()
        };

        _mockAccountRepository
            .Setup(r => r.GetAllNonAdminUsersAsync())
            .ReturnsAsync(users);

        List<ApplicationUser> result = await _accountService.GetAllUsersAsync();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllUsersAsync_NoUsers_ReturnsEmptyList()
    {
        List<ApplicationUser> users = new List<ApplicationUser>();

        _mockAccountRepository
            .Setup(r => r.GetAllNonAdminUsersAsync())
            .ReturnsAsync(users);

        List<ApplicationUser> result = await _accountService.GetAllUsersAsync();

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task UserPhoneNumberExistsAsync_ShouldReturnTrue_WhenPhoneNumberExists()
    {
        string phoneNumber = "+359888123456";
        string userId = "user-123";

        _mockAccountRepository
            .Setup(repo => repo.UserPropertyExistsAsync("PhoneNumber", phoneNumber, userId))
            .ReturnsAsync(true);

        bool result = await _accountService.UserPhoneNumberExistsAsync(phoneNumber, userId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UserPhoneNumberExistsAsync_ShouldReturnFalse_WhenPhoneNumberDoesNotExist()
    {
        string phoneNumber = "+359888123456";
        string userId = "user-123";

        _mockAccountRepository
            .Setup(repo => repo.UserPropertyExistsAsync("PhoneNumber", phoneNumber, userId))
            .ReturnsAsync(false);

        bool result = await _accountService.UserPhoneNumberExistsAsync(phoneNumber, userId);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateUserProfile_ShouldUpdatePhoneNumberAndCallSaveChanges()
    {
        ApplicationUser user = new ApplicationUser
        {
            PhoneNumber = "+359888123456"
        };

        string newPhoneNumber = "+359899112233";

        await _accountService.UpdateUserProfile(newPhoneNumber, user);

        Assert.That(user.PhoneNumber, Is.EqualTo(newPhoneNumber));
        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UserEmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
    {
        string email = "test@example.com";
        string userId = "user-123";

        _mockAccountRepository
            .Setup(repo => repo.UserPropertyExistsAsync("Email", email, userId))
            .ReturnsAsync(true);

        bool result = await _accountService.UserEmailExistsAsync(email, userId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UserEmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
    {
        string email = "test@example.com";
        string userId = "user-123";

        _mockAccountRepository
            .Setup(repo => repo.UserPropertyExistsAsync("Email", email, userId))
            .ReturnsAsync(false);

        bool result = await _accountService.UserEmailExistsAsync(email, userId);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task ChangeEmailAsync_ShouldUpdateEmailAndCallSaveChanges()
    {
        ApplicationUser user = new ApplicationUser
        {
            Email = "old@example.com",
            NormalizedEmail = "OLD@EXAMPLE.COM"
        };

        EmailViewModel emailViewModel = new EmailViewModel
        {
            NewEmail = "new@example.com"
        };

        await _accountService.ChangeEmailAsync(emailViewModel, user);

        Assert.Multiple(() =>
        {
            Assert.That(user.Email, Is.EqualTo(emailViewModel.NewEmail));
            Assert.That(user.NormalizedEmail, Is.EqualTo(emailViewModel.NewEmail.ToUpper()));
        });

        _mockBaseRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task ChangeUserPasswordAsync_ShouldReturnSuccess_WhenPasswordChanged()
    {
        ApplicationUser user = new ApplicationUser();
        string oldPassword = "oldPass123!";
        string newPassword = "newPass456!";

        IdentityResult successResult = IdentityResult.Success;

        _mockUserManager
            .Setup(um => um.ChangePasswordAsync(user, oldPassword, newPassword))
            .ReturnsAsync(successResult);

        IdentityResult result = await _accountService.ChangeUserPasswordAsync(user, oldPassword, newPassword);

        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task ChangeUserPasswordAsync_ShouldReturnFailed_WhenPasswordChangeFails()
    {
        ApplicationUser user = new ApplicationUser();
        string oldPassword = "oldPass123!";
        string newPassword = "newPass456!";

        IdentityResult failedResult = IdentityResult.Failed(new IdentityError 
        { 
            Description = "Incorrect Password."
        });

        _mockUserManager
            .Setup(um => um.ChangePasswordAsync(user, oldPassword, newPassword))
            .ReturnsAsync(failedResult);

        IdentityResult result = await _accountService.ChangeUserPasswordAsync(user, oldPassword, newPassword);

        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
            Assert.That(result.Errors.Any(e => e.Description == "Incorrect Password."), Is.True);
        });
    }

    [Test]
    public async Task RefreshUserSignInAsync_ShouldCallRefreshSignInAsyncOnce()
    {
        ApplicationUser user = new ApplicationUser();

        _mockSignInManager
            .Setup(sm => sm.RefreshSignInAsync(user))
            .Returns(Task.CompletedTask);

        await _accountService.RefreshUserSignInAsync(user);

        _mockSignInManager.Verify(sm => sm.RefreshSignInAsync(user), Times.Once);
    }

    [Test]
    public async Task DeleteUserAsync_ShouldReturnSuccess_WhenUserDeleted()
    {
        ApplicationUser user = new ApplicationUser();
        IdentityResult successResult = IdentityResult.Success;

        _mockUserManager
            .Setup(um => um.DeleteAsync(user))
            .ReturnsAsync(successResult);

        IdentityResult result = await _accountService.DeleteUserAsync(user);

        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task DeleteUserAsync_ShouldReturnFailed_WhenDeletionFails()
    {
        ApplicationUser user = new ApplicationUser();

        IdentityResult failedResult = IdentityResult.Failed(new IdentityError 
        { 
            Description = "Deletion failed." 
        });

        _mockUserManager
            .Setup(um => um.DeleteAsync(user))
            .ReturnsAsync(failedResult);

        IdentityResult result = await _accountService.DeleteUserAsync(user);

        Assert.Multiple(() =>
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
            Assert.That(result.Errors.Any(e => e.Description == "Deletion failed."), Is.True);
        });
    }

    [Test]
    public async Task RemoveUserFromRoleAsync_ValidUserAndRole_CallsRemoveFromRoleAsync()
    {
        ApplicationUser user = new ApplicationUser();
        string role = "Administrator";

        _mockUserManager
            .Setup(um => um.RemoveFromRoleAsync(user, role))
            .ReturnsAsync(IdentityResult.Success);

        await _accountService.RemoveUserFromRoleAsync(user, role);

        _mockUserManager.Verify(um => um.RemoveFromRoleAsync(user, role), Times.Once);
    }
}