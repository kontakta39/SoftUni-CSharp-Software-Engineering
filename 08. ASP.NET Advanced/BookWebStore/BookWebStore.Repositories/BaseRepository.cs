using BookWebStore.Data;
using BookWebStore.Data.Entities;
using BookWebStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore.Repositories;

public class BaseRepository : IBaseRepository
{
    private readonly BookStoreDbContext _context;

    public BaseRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<TType>> GetAllAsync<TType>() where TType : class, IEntity
    {
        return await _context.Set<TType>()
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<TType>> SearchByPropertyAsync<TType>(string propertyName, string loweredTerm)
    where TType : class, IEntity
    {
        return await _context.Set<TType>()
         .Where(e => EF.Property<string>(e, propertyName).ToLower().Contains(loweredTerm) && !e.IsDeleted)
         .ToListAsync();
    }

    public async Task<TType?> GetByIdAsync<TType>(Guid id) where TType : class, IEntity
    {
        return await _context.Set<TType>()
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<bool> ExistsByPropertyAsync<TType>(string propertyName, string nameKey, Guid? id = null)
    where TType : class, IEntity
    {
        return await _context.Set<TType>()
            .AnyAsync(e => EF.Property<string>(e, propertyName) == nameKey && 
            !e.IsDeleted && (id == null || e.Id != id));
    }

    public async Task AddAsync<TType>(TType entity) where TType : class
    {
        await _context.Set<TType>().AddAsync(entity);
    }

    public void Remove<TType>(TType entity) where TType : class
    {
        _context.Set<TType>().Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}