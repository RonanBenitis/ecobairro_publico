using ecobairroServer.Data;
using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models.Pessoa;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ecobairroServer.Source.Core.Services;

public class FiscalService : IFiscalService
{
    private readonly DataContext _dataContext;
    public FiscalService(DataContext context)
    {
        _dataContext = context;
    }

    public async Task<Fiscal> AddAsync(FiscalDTO fiscalDto)
    {
        using var transaction = await _dataContext.Database
            .BeginTransactionAsync();

        try
        {
            // Cria novo usuário
            var newUser = new User
            {
                Nome = fiscalDto.Nome,
                Username = fiscalDto.Username,
                Password = fiscalDto.Password,
                Email = fiscalDto.Email,
                Role = "Fiscal"
            };

            _dataContext.Add(newUser);
            // Salva para obter UserId
            await _dataContext.SaveChangesAsync();

            // Criar o municipe com UserId
            var fiscal = new Fiscal
            {
                Rgf = fiscalDto.Rgf,
                UserId = newUser.Id,
                User = newUser
            };

            _dataContext.Add(fiscal);
            await _dataContext.SaveChangesAsync();

            // Commit da transação
            await transaction.CommitAsync();

            return fiscal;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var fiscal = await GetByIdAsync(userId);
        if (fiscal == null) return false;

        // Deleta User atribuido a este fiscal
        _dataContext.Users.Remove(fiscal.User);
        // Redundancia, pois acima já é delete cascading
        _dataContext.Fiscais.Remove(fiscal);

        await _dataContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Fiscal>> GetAllAsync()
    {
        return await _dataContext.Fiscais
            .Include(f => f.User)
            .ToListAsync();
    }

    public async Task<Fiscal?> GetByIdAsync(int userId)
    {
        return await _dataContext.Fiscais
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.UserId == userId);
    }

    public async Task<bool> UpdateAsync(int userId, FiscalDTO fiscalDto)
    {
        var fiscal = await GetByIdAsync(userId);
        if (fiscal == null) return false;

        // Atualiza user
        var user = fiscal.User;
        user.Nome = fiscalDto.Nome;
        user.Username = fiscalDto.Username;
        user.Password = fiscalDto.Password;
        user.Email = fiscalDto.Email;

        // Atualiza municipe
        fiscal.Rgf = fiscalDto.Rgf;

        await _dataContext.SaveChangesAsync();

        return true;
    }
}
