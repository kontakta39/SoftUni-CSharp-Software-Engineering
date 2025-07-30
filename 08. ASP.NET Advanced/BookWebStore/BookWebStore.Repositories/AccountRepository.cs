using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BookStoreDbContext _context;

    public AccountRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationUser>> GetAllNonAdminUsersAsync()
    {
        return await _context.Users
            .Where(u => u.Email != "kontakta39@mail.bg" && u.UserName != "kontakta39")
            .ToListAsync();
    }

    public async Task<bool> UserPropertyExistsAsync(string propertyName, string nameKey, string userId)
    {
        return await _context.Users.AnyAsync(u =>
            u.Id != userId &&
            EF.Property<string>(u, propertyName) == nameKey);
    }
}