using ecobairroServer.Source.Core.Models.Pessoa;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ecobairroServer.Source.Inbound.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// READ ALL: Obter a lista de Usuários
    /// </summary>
    /// <returns>Lista de objetos</returns>
    /// <remarks>Chame este endpoint para obter a lista de objetos desta classe específica que encontra-se armaenada no banco de dados</remarks>
    /// <response code="200">Ação realizada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// READ: Obter Usuário por E-mail
    /// </summary>
    /// <param name="email">Email do usuário, tipo string</param>
    /// <returns>Dados do objeto</returns>
    /// <remarks>Busque um usuário através do e-mail inserido, obtendo sua correlação ou um valor de "Não encontrado" caso correlação inexistente</remarks>
    /// <response code="200">Objeto entregue com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpGet("email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetByEmail(string email)
    {
        var user = await _service.GetByEmail(email);

        if (user == null) return NotFound();

        return user;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetById(int userId)
    {
        var user = await _service.GetById(userId);

        if (user == null) return NotFound();

        return user;
    }
}
