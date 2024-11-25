using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ecobairroServer.Source.Inbound.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BairroController : ControllerBase
{
    private readonly IBairroService _service;

    public BairroController(IBairroService service)
    {
        _service = service;
    }

    /// <summary>
    /// READ ALL: Obter lista de todos os bairros cadastrados
    /// </summary>
    /// <returns>Lista de bairros</returns>
    /// <remarks>Chame este endpoint para obter a lista de objetos desta classe específica que encontra-se armaenada no banco de dados</remarks>
    /// <response code="200">Ação realizada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Bairro>>> GetAll()
    {
        return Ok(await _service.GetAllBairrosAsync());
    }

    /// <summary>
    /// GET POINTS: Busca os pontos de um determinado bairro
    /// </summary>
    /// <remarks>Rota para buscar a quantidade de pontos de um bairro especifico</remarks>
    /// <returns>Nada</returns>
    /// <response code="200">Pontos retornados com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response>
    [HttpGet("getpoint/{bairroId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetPoints(int bairroId)
    {
        int pontos = await _service.GetPointsAsync(bairroId);

        return pontos;
    }
}
