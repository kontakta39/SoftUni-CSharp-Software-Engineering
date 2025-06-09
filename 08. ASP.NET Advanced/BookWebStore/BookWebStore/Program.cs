using BookWebStore.Data;
using BookWebStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        string? connectionString = builder.Configuration.GetConnectionString("BookStoreConnectionString") ?? throw new InvalidOperationException("Connection string 'BookStoreConnectionString' not found.");
        builder.Services.AddDbContext<BookStoreDbContext>(options =>
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
            .AddEntityFrameworkStores<BookStoreDbContext>();
        builder.Services.AddControllersWithViews();

        WebApplication? app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
