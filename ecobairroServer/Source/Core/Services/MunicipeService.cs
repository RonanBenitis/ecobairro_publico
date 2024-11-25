using ecobairroServer.Data;
using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models.Pessoa;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ecobairroServer.Source.Core.Services;

public class MunicipeService : IMunicipeService
{
    private readonly DataContext _dataContext;

    public MunicipeService(DataContext context)
    {
        _dataContext = context;
    }
    public async Task<Municipe> AddAsync(MunicipeDTO municipeDto)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync();

        try
        {
            // Criar novo usuário
            var newUser = new User
            {
                Nome = municipeDto.Nome,
                Username = municipeDto.Username,
                Password = municipeDto.Password,
                Email = municipeDto.Email,
                Role = "Municipe"
            };

            _dataContext.Add(newUser);
            // Salva para obter UserId
            await _dataContext.SaveChangesAsync();

            // Criar o munícipe com UserId
            var municipe = new Municipe
            {
                Cpf = municipeDto.Cpf,
                UserId = newUser.Id,
                User = newUser,
            };

            _dataContext.Add(municipe);
            await _dataContext.SaveChangesAsync();

            // Commit da transação
            await transaction.CommitAsync();

            return municipe;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var municipe = await GetMunicipeByUserIdAsync(userId);
        if (municipe == null) return false;

        // Deleta User atribuido a este Municipe
        _dataContext.Users.Remove(municipe.User);
        // Redundancia, pois acima já é delete cascading
        _dataContext.Municipes.Remove(municipe);

        await _dataContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Municipe>> GetAllAsync()
    {
        return await _dataContext.Municipes
            .Include(m => m.User)
            .ToListAsync();
    }

    public async Task<Municipe?> GetMunicipeByUserIdAsync(int userId)
    {
        return await _dataContext.Municipes
            .Include(m => m.User) // Permite Eager Loading (veja explicaçao no md)
            .FirstOrDefaultAsync(m => m.UserId == userId);

        /* Exemplo de consumo de dados com LINQ relacional
         *
         * Console.WriteLine(municipe.User.Nome);
         * Isso acessa a propriedade Nome do User associado à query acima
         */
    }

    public async Task<bool> UpdateAsync(int userId, MunicipeDTO municipeDto)
    {
        var municipe = await GetMunicipeByUserIdAsync(userId);

        if (municipe == null) return false;

        // Atualiza user
        var user = municipe.User;
        user.Nome = municipeDto.Nome;
        user.Username = municipeDto.Username;
        user.Password = municipeDto.Password;
        user.Email = municipeDto.Email;

        //Atualizar municipe
        municipe.Cpf = municipeDto.Cpf;

        await _dataContext.SaveChangesAsync();

        return true;
    }


    /********************
     * MÉTODOS SEM ROTA *
     ********************/
    public async Task<Municipe?> GetMunicipeByIdAsync(int municipeId)
    {
        return await _dataContext.Municipes
            .Include(m => m.User) // Permite Eager Loading (veja explicaçao no md
            .FirstOrDefaultAsync(m => m.Id == municipeId);
    }

}
