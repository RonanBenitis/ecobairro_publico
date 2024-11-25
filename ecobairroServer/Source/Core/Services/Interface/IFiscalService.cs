using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models.Pessoa;

namespace ecobairroServer.Source.Core.Services.Interface;

public interface IFiscalService
{
    Task<IEnumerable<Fiscal>> GetAllAsync();
    Task<Fiscal?> GetByIdAsync(int userId);
    Task<Fiscal> AddAsync(FiscalDTO fiscalDto);
    Task<bool> UpdateAsync(int userId, FiscalDTO fiscalDto);
    Task<bool> DeleteAsync(int userId);
}
