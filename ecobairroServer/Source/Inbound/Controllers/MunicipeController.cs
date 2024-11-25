using Microsoft.AspNetCore.Mvc;
using ecobairroServer.Source.Core.Models.Pessoa;
using ecobairroServer.Source.Core.Services.Interface;
using ecobairroServer.Source.Core.Dto;

namespace ecobairroServer.Source.Inbound.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MunicipeController : ControllerBase
{
    private readonly IMunicipeService _service;
    public MunicipeController(IMunicipeService service)
    {
        _service = service;
    }

    /// <summary>
    /// READ ALL: Obter a lista de Municipes
    /// </summary>
    /// <returns>Lista de objetos</returns>
    /// <remarks>Chame este endpoint para obter a lista de objetos desta classe específica que encontra-se armaenada no banco de dados</remarks>
    /// <response code="200">Ação realizada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Municipe>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// READ: Obter um Municipe
    /// </summary>
    /// <param name="userId">Identificador (ID) do usuário, tipo inteiro. Note que é USUÁRIO, e não ID do Municipe ou Fiscal</param>
    /// <returns>Dados do objeto</returns>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para buscar o objeto desejado da classe especifica deste método</remarks>
    /// <response code="200">Objeto entregue com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Municipe>> GetById(int userId)
    {
        var municipe = await _service.GetMunicipeByUserIdAsync(userId);

        if (municipe == null) return NotFound();

        return municipe;
    }

    /// <summary>
    /// CREATE: Armazena um novo objeto Muncipe
    /// </summary>
    /// <param name="municipeDto">DTO de Municipe, só utilizar o template do Swagger</param>
    /// <remarks>Utiliza-se dos dados do DTO Municipe para criar o USUÁRIO e o MUNICIPE, relacionando-os</remarks>
    /// <returns>Informações do objeto criado</returns>
    /// <response code="201">Informações do objeto criado</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Municipe>> Create(MunicipeDTO municipeDto)
    {
        var createdEntity = await _service.AddAsync(municipeDto);

        return CreatedAtAction(nameof(GetById), new { userId = createdEntity.UserId }, createdEntity);
    }

    /// <summary>
    /// UPDATE: Atualizar um Municipe
    /// </summary>
    /// <param name="userId">Identificador (ID) do objeto (tipo int)</param>
    /// <param name="municipeDto">Objeto do tipo desta classe especifica (Ou seja, se você estiver chamando o endpoint de Municipe, este parametro aguarda o tipo "Municipe")</param>
    /// <returns>Sem conteúdo</returns>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para identificar o objeto que deseja-se atualizar. Como segundo parâmetro, passa-se um objeto (de mesmo id) com os novos dados a serem armazenados.</remarks>
    /// <response code="204">Objeto entregue com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int userId, MunicipeDTO municipeDto)
    {
        if (!await _service.UpdateAsync(userId, municipeDto)) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// DELETE: Exclui um Municipe
    /// </summary>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para excluir um Municipe. Deletá-lo excluírá, também, seu usuário</remarks>
    /// <param name="userId">Identificador (ID) do objeto (tipo int)</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Objeto excluído com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int userId)
    {
        if (!await _service.DeleteAsync(userId)) return NotFound();

        return NoContent();
    }
}
