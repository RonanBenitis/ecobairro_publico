using ecobairroServer.Source.Core.Models;

namespace ecobairroServer.Source.Core.Services.Interface;

public interface IBairroService
{
    Task<IEnumerable<Bairro>> GetAllBairrosAsync();
    Task<Bairro?> GetBairroByIdAsync(int bairroId);
    Task<bool> AddPointsAsync(int bairroId, int points);
    Task<int> GetPointsAsync(int bairroId);
}
