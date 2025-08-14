using BookWebStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookWebStore.Data;

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

        //Retrieve admin password from appsettings.json
        string adminUsername = configuration["AdminUsername"] ?? "Admin";
        string adminEmail = configuration["AdminEmail"] ?? "admin@example.com";
        string adminPassword = configuration["AdminPassword"] ?? "Admin123!";

        ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail!);

        if (adminUser == null)
        {
            ApplicationUser? admin = new ApplicationUser
            {
                UserName = adminUsername,
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
                return; //Exit the method if user creation fails
            }
        }
        else
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}