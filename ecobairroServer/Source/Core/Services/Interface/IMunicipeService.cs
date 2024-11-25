using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models.Pessoa;

namespace ecobairroServer.Source.Core.Services.Interface;

public interface IMunicipeService
{
    Task<IEnumerable<Municipe>> GetAllAsync();
    Task<Municipe?> GetMunicipeByUserIdAsync(int userId);
    Task<Municipe?> GetMunicipeByIdAsync(int municipeId);
    Task<Municipe> AddAsync(MunicipeDTO municipe);
    Task<bool> UpdateAsync(int userId, MunicipeDTO municipeDto);
    Task<bool> DeleteAsync(int userId);
}
