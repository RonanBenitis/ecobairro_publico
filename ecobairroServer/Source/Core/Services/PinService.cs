using ecobairroServer.Data;
using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models;
using ecobairroServer.Source.Core.Models.Marcacao;
using ecobairroServer.Source.Core.Models.Pessoa;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ecobairroServer.Source.Core.Services;

public class PinService : IPinService
{
    private readonly DataContext _dataContext;
    private readonly IMunicipeService _munipeService;
    private readonly IFiscalService _fiscalService;
    private readonly IBairroService _bairroService;

    public PinService(DataContext context,
        IMunicipeService municipeService, IFiscalService fiscalService,
        IBairroService bairroService) 
    {
        _dataContext = context;
        _munipeService = municipeService;
        _fiscalService = fiscalService;
        _bairroService = bairroService;
    }

    /************************
     **  MÉTODOS PUBLICOS  **
     ************************/
    public async Task<PinDTOCreate> AddAsync(PinDTOCreate pinDto)
    {
        var municipe = await _GetMunicipeAsync(pinDto.MunicipeCriadorUserId)
            ?? throw new ArgumentException("Usuário não encontrado ou não é um municipe");

        var newPin = _MapperPinDTOCreateToPin(pinDto, municipe);

        _dataContext.Pins.Add(newPin);
        await _dataContext.SaveChangesAsync();

        pinDto.Id = newPin.Id;

        return pinDto;
    }

    public async Task<IEnumerable<PinDTOShow>> GetAllAsync()
    {
        var pins = await _dataContext.Pins
            .Include(pin => pin.Fiscal) // Inclui o Fiscal relacionado, se existir
                .ThenInclude(fiscal => fiscal!.User) // Inclui o User dentro de Fiscal
            .Include(pin => pin.MunicipeCriador) // Inclui o MunicipeCriador relacionado, se existir
                .ThenInclude(municipe => municipe!.User) // Inclui o User dentro de MunicipeCriador
            .ToListAsync();

        var pinDTOs = pins
            .Select(pin => _MapperPinToPinDTOShow(pin));

        return pinDTOs;
    }

    public async Task<bool> PinAproveAsync(int pinId, int userId)
    {
        var fiscal = await _GetFiscalAsync(userId)
            ?? throw new ArgumentException("Usuário não encontrado ou não é um Fiscal");

        var pin = await _GetPinAsync(pinId)
            ?? throw new ArgumentException("Pin não encontrado");

        pin.FiscalId = fiscal.Id;

        pin.Status = "Aprovado";

        return await _SaveDbChanges();
    }

    public async Task<bool> PinConcludeAsync(int pinId, int userId)
    {
        var pin = await _GetPinAsync(pinId)
            ?? throw new ArgumentException("Pin não encontrado");

        // Validações
        _UserAndPinFiscalComparison(userId, pin.Fiscal);

        if (pin.Pontuacao == null)
        {
            throw new ArgumentException("Pin sem pontuação definida");
        }

        // Acrescentando ponto ao bairro
        await _bairroService.AddPointsAsync(1, pin.Pontuacao ?? 0);

        // Atualiza o status diretamente
        pin.Status = "Concluído";

        return await _SaveDbChanges();
    }

    public async Task<PinDTOShow?> GetByIdAsync(int pinId)
    {
        var pin = await _GetPinAsync(pinId);

        if (pin == null) return null;

        var pinDtoShow = _MapperPinToPinDTOShow(pin);

        return pinDtoShow;
    }

    public async Task<bool> SetPointsAsync(int userId,
        int pinId, int points)
    {
        var pin = await _GetPinAsync(pinId)
            ?? throw new ArgumentException("Pin não encontrado");

        // Valida se Fiscal existe e se é o Fiscal do Pin
        _UserAndPinFiscalComparison(userId, pin.Fiscal);

        // Atualizando
        pin.Pontuacao = points;

        return await _SaveDbChanges();
    }

    public async Task<bool> UpdateWholeAsync(PinDTO pinDto)
    {
        var pin = await _GetPinAsync(pinDto.Id)
            ?? throw new ArgumentException("Pin não encontrado");

        // Serealizando
        pin.Pontuacao = pinDto.Pontuacao;
        pin.Descricao = pinDto.Descricao;
        pin.Latitude = pinDto.Latitude;
        pin.Longitude = pinDto.Longitude;
        pin.Categoria = pinDto.Categoria;
        pin.Status = pinDto.Status;
        pin.Endereco = pinDto.Endereco;

        _dataContext.Entry(pin).State = EntityState.Modified;

        return await _SaveDbChanges();
    }

    public async Task<bool> DeleteByIdAsync(int pinId)
    {
        var pin = await _GetPinAsync(pinId)
            ?? throw new ArgumentException("Pin não encontrado");

        _dataContext.Pins.Remove(pin);
        await _dataContext.SaveChangesAsync();

        return true;
    }

    /************************
     **  MÉTODOS PRIVADOS  **
     ************************/
    private async Task<Fiscal?> _GetFiscalAsync(int userId)
    {
        return await _fiscalService.GetByIdAsync(userId);
    }

    private async Task<Municipe?> _GetMunicipeAsync(int userId)
    {
        return await _munipeService.GetMunicipeByUserIdAsync(userId);
    }

    private async Task<Bairro?> _GetBairroAsync(int bairroId)
    {
        return await _bairroService.GetBairroByIdAsync(bairroId);
    }

    private async Task<Pin?> _GetPinAsync(int pinId)
    {
        /*
         * Detalhe: Não usamos o GetByIdAsync pois ele
         * retornaria um pinDTOShow, um intermediário, não
         * o objeto do banco mesmo. Por isso consultamos
         * direto o contexto do banco.
         */
        return await _dataContext.Pins
             .Include(pin => pin.Fiscal) // Inclui o Fiscal relacionado, se existir
                .ThenInclude(fiscal => fiscal!.User) // Inclui o User dentro de Fiscal
            .Include(pin => pin.MunicipeCriador) // Inclui o MunicipeCriador relacionado, se existir
                .ThenInclude(municipe => municipe!.User) // Inclui o User dentro de MunicipeCriador
            .FirstOrDefaultAsync(p => p.Id == pinId);
    }

    private async Task<bool> _SaveDbChanges()
    {
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

    private void _UserAndPinFiscalComparison(int userId, Fiscal? fiscal)
    {
        if (fiscal == null)
        {
            throw new ArgumentException("Pin não possui Fiscal");
        }
        else if (userId != fiscal.UserId)
        {
            Console.WriteLine($"Consultando autorização do usuário de id {fiscal.UserId}...");
            throw new ArgumentException("Este usuário não é o Fiscal deste pin");
        }
        Console.WriteLine($"Usuário de id {fiscal.UserId} possui autorização para a operação");
    }

    /*********************************
     **  MÉTODOS PRIVADOS: MAPPERS  **
     *********************************/
    private PinDTOShow _MapperPinToPinDTOShow(Pin pin)
    {
        return new PinDTOShow
        {
            Id = pin.Id,
            Pontuacao = pin.Pontuacao,
            Descricao = pin.Descricao,
            Latitude = pin.Latitude,
            Longitude = pin.Longitude,
            Categoria = pin.Categoria,
            Status = pin.Status,
            Endereco = pin.Endereco,
            FiscalUserId = pin.Fiscal?.UserId, // LINQ É MÁGICO
            NomeFiscal = pin.Fiscal?.User?.Nome ?? string.Empty,
            MunicipeCriadorUserId = pin.MunicipeCriador!.UserId, // LINQ É MÁGICO
            NomeMunicipe = pin.MunicipeCriador.User.Nome
        };
    }

    private Pin _MapperPinDTOCreateToPin(PinDTOCreate pinDtoCreate,
        Municipe municipe)
    {
        return new Pin
        {
            Latitude = pinDtoCreate.Latitude,
            Longitude = pinDtoCreate.Longitude,
            Endereco = pinDtoCreate.Endereco,
            Descricao = pinDtoCreate.Descricao,
            Categoria = "Limpeza Urbana",
            Status = "Aguardando análise",
            MunicipeCriadorId = municipe.Id,
            BairroId = 1
        };
    }
}
