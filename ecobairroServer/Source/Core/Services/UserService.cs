using ecobairroServer.Data;
using ecobairroServer.Source.Core.Models.Pessoa;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ecobairroServer.Source.Core.Services;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;

    public UserService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dataContext.Users
            .ToListAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dataContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(int id)
    {
        return await _dataContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
