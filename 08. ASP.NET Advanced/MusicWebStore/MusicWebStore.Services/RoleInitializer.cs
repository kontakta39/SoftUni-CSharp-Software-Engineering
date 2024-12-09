using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicWebStore.Data.Models;

namespace MusicWebStore.Services;

public static class RoleInitializer
{
    public static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
    {
        RoleManager<ApplicationRole>? roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        UserManager<ApplicationUser>? userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        IConfiguration? configuration = serviceProvider.GetRequiredService<IConfiguration>();

        string[] roleNames = { "Administrator", "Moderator", "Guest" };

        foreach (string roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                ApplicationRole? role = new ApplicationRole
                {
                    Name = roleName
                };

                await roleManager.CreateAsync(role);
            }
        }

        string? adminEmail = "kontakta39@mail.bg";
        // Retrieve admin password from secrets.json
        string? adminPassword = configuration["AdminPassword"]
            ?? throw new InvalidOperationException("Admin password not found in configuration.");

        ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            ApplicationUser? admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Ivan",
                LastName = "Angelov",
                EmailConfirmed = false
            };

            IdentityResult? result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Administrator");
            }
            else
            {
                Console.WriteLine($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return; // Exit the method if user creation fails
            }
        }
        else
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}