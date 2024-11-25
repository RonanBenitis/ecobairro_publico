using Microsoft.EntityFrameworkCore;
using ecobairroServer.Source.Core.Models.Interface;
using ecobairroServer.Source.Core.Services.Interface;

namespace ecobairroServer.Source.Core.Services.Abstraction;

public class CrudService<T> : ICrudService<T> where T : class, IId
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _table;

    public CrudService(DbContext context)
    {
        _context = context;
        _table = _context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        _table.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _table.FindAsync(id);

        if (entity == null) return false;

        _table.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _table
            .AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _table.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ExistsAsync(entity.Id))
            {
                return false;
            }
            throw;
        }
    }
}
