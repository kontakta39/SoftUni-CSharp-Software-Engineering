using BookWebStore.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Data;

public class BookStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}