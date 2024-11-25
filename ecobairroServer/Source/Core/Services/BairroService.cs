using ecobairroServer.Data;
using ecobairroServer.Source.Core.Models;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ecobairroServer.Source.Core.Services;

public class BairroService : IBairroService
{
    private readonly DataContext _dataContext;

    public BairroService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /************************
     **  MÉTODOS PUBLICOS  **
     ************************/
    public async Task<bool> AddPointsAsync(int bairroId, int points)
    {
        var bairro = await GetBairroByIdAsync(bairroId);

        if (bairro == null) return false;

        bairro.Pontuacao = bairro.Pontuacao + points;

        try
        {
            await _dataContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    public async Task<int> GetPointsAsync(int bairroId)
    {
        var bairro = await GetBairroByIdAsync(bairroId)
            ?? throw new ArgumentException("bairro inexistente");

        return bairro.Pontuacao;
    }

    public async Task<IEnumerable<Bairro>> GetAllBairrosAsync()
    {
        return await _dataContext.Bairros
            .ToListAsync();
    }

    public async Task<Bairro?> GetBairroByIdAsync(int bairroId)
    {
        var bairro = await _dataContext.Bairros
            .FirstOrDefaultAsync(b => b.Id == bairroId);

        if (bairro == null) return null;

        return bairro;
    }

}
