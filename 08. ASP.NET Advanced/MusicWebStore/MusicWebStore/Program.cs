using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicWebStore.Data;
using MusicWebStore.Data.Models;
using MusicWebStore.Services;

namespace MusicWebStore;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();

        string? connectionString = builder.Configuration.GetConnectionString("MusicStoreConnectionString") ?? throw new InvalidOperationException("Connection string 'MusicStoreConnectionString' not found.");
        builder.Services.AddDbContext<MusicStoreDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        IdentityOptions? identityOptions = new IdentityOptions();
        builder.Configuration.GetSection("IdentityOptions").Bind(identityOptions);

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = identityOptions.SignIn.RequireConfirmedAccount;
            options.Password.RequireDigit = identityOptions.Password.RequireDigit;
            options.Password.RequireNonAlphanumeric = identityOptions.Password.RequireNonAlphanumeric;
            options.Password.RequireUppercase = identityOptions.Password.RequireUppercase;
            options.Lockout.DefaultLockoutTimeSpan = identityOptions.Lockout.DefaultLockoutTimeSpan;
            options.Lockout.MaxFailedAccessAttempts = identityOptions.Lockout.MaxFailedAccessAttempts;
            options.Lockout.AllowedForNewUsers = identityOptions.Lockout.AllowedForNewUsers;
        })
        .AddEntityFrameworkStores<MusicStoreDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        // Define folder paths for the ImageHandler
        string artistFinalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images");
        string albumFinalFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");

        // Register ImageHandler as a singleton service
        builder.Services.AddSingleton(new ImageHandler(albumFinalFolderPath));
        builder.Services.AddSingleton(new ImageHandler(artistFinalFolderPath));

        WebApplication? app = builder.Build();

        // Initialize roles and admin user
        using (IServiceScope? scope = app.Services.CreateScope())
        {
            IServiceProvider? serviceProvider = scope.ServiceProvider;
            await RoleInitializer.InitializeRolesAsync(serviceProvider);
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Add middleware to handle 404 errors
        app.UseStatusCodePagesWithReExecute("/Home/NotFound");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();  
        app.UseAuthorization();   

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}