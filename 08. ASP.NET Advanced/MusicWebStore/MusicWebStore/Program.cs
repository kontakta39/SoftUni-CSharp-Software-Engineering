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

        // Configure cookie authentication for login and access denied paths
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Home/AccessDenied"; 
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<IGenreService, GenreService>();

        // Define folder paths for the ImageHandler
        string artistFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Artists Images");
        string albumFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Albums Covers");
        string blogFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Blogs Images");

        // Register ImageHandler as a singleton service
        builder.Services.AddSingleton(new ImageHandler(albumFolderPath));
        builder.Services.AddSingleton(new ImageHandler(artistFolderPath));
        builder.Services.AddSingleton(new ImageHandler(blogFolderPath));

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