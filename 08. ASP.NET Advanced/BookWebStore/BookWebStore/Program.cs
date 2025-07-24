using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories;
using BookWebStore.Repositories.Interfaces;
using BookWebStore.Services;
using BookWebStore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore;

public class Program
{
    public static async Task Main(string[] args)
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
            options.Password.RequireLowercase = identityOptions.Password.RequireLowercase;
            options.Lockout.DefaultLockoutTimeSpan = identityOptions.Lockout.DefaultLockoutTimeSpan;
            options.Lockout.MaxFailedAccessAttempts = identityOptions.Lockout.MaxFailedAccessAttempts;
            options.Lockout.AllowedForNewUsers = identityOptions.Lockout.AllowedForNewUsers;
        })
            .AddEntityFrameworkStores<BookStoreDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(30);
        });

        // Configure cookie authentication for login and access denied paths
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Home/AccessDenied";
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<IBlogService, BlogService>();

        builder.Services.AddScoped<IGenreRepository, GenreRepository>();

        WebApplication? app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        //StatusCodePages for handling 404 errors
        app.UseStatusCodePagesWithReExecute("/Home/NotFound");

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

        //Initialize roles and admin user
        using (IServiceScope? scope = app.Services.CreateScope())
        {
            IServiceProvider? serviceProvider = scope.ServiceProvider;
            await RoleInitializer.InitializeRolesAsync(serviceProvider);

            string genreJsonPath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory,
                "..", "..", "..", "..",
                "BookWebStore.Data", "Seed", "SeedData", "genres.json"));
            await GenreImporter.ImportGenresFromJsonAsync(genreJsonPath, serviceProvider);
        }

        app.Run();
    }
}