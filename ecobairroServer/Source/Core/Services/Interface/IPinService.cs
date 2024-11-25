using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models.Marcacao;

namespace ecobairroServer.Source.Core.Services.Interface;

public interface IPinService
{
    Task<IEnumerable<PinDTOShow>> GetAllAsync();
    Task<PinDTOShow?> GetByIdAsync(int pinId);
    Task<PinDTOCreate> AddAsync(PinDTOCreate pinDto);
    Task<bool> SetPointsAsync(int userId, int pinId, int points);
    Task<bool> PinAproveAsync(int pinId, int userId);
    Task<bool> PinConcludeAsync(int pinId, int userId);
    Task<bool> UpdateWholeAsync(PinDTO pin);
    Task<bool> DeleteByIdAsync(int pinId);
}
