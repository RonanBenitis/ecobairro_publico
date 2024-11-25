using ecobairroServer.Source.Core.Dto;
using ecobairroServer.Source.Core.Models.Marcacao;
using ecobairroServer.Source.Core.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ecobairroServer.Source.Inbound.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PinController : ControllerBase
{
    private readonly IPinService _service;

    public PinController(IPinService service)
    {
        _service = service;
    }

    /// <summary>
    /// READ ALL: Obter a lista de Pins
    /// </summary>
    /// <returns>Lista de pins</returns>
    /// <remarks>Chame este endpoint para obter a lista de objetos desta classe específica que encontra-se armaenada no banco de dados</remarks>
    /// <response code="200">Ação realizada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PinDTOShow>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// READ: Obter um Pin utilizando seu ID
    /// </summary>
    /// <param name="id">Identificador (ID) do pin do tipo int</param>
    /// <returns>Dados do objeto</returns>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para buscar o objeto desejado da classe especifica deste método</remarks>
    /// <response code="200">Objeto entregue com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PinDTOShow>> GetById(int id)
    {
        var pinDtoShow = await _service.GetByIdAsync(id);

        if (pinDtoShow == null) return NotFound();

        return pinDtoShow;
    }

    /// <summary>
    /// CREATE (Qualquer Municipe): Cria um novo Pin
    /// </summary>
    /// <param name="pinDto">JSON dos campos necessários para criação de um novo Pin</param>
    /// <remarks>Método de criação de Pin através das informações básicas necessários. O Pin só pode ser criado por um MUNICIPE, caso contrário, o código não permitirá sua criação</remarks>
    /// <returns>Informações do objeto criado</returns>
    /// <response code="201">Pin criado com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Pin>> Create(PinDTOCreate pinDto)
    {
        var createdPin = await _service.AddAsync(pinDto);

        return CreatedAtAction(nameof(GetById), new { id = createdPin.Id }, createdPin);
    }

    /// <summary>
    /// DELETE: Exclui um Pin
    /// </summary>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para excluir um Pin existentee</remarks>
    /// <param name="id">Identificador (ID) do Pin (tipo int)</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Objeto excluído com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</response>
    /// <response code="404">Pin não encontado</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteByIdAsync(id)) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// APROVE (Qualquer Fiscal): Aprovação de Pin pelo seu ID
    /// </summary>
    /// <remarks>Rota para aprovação do Pin, tranformando seu Status como Aprovado. Somente um usuário FISCAL pode aprovar Pin, caso contrário, o código não concluirá a ação</remarks>
    /// <param name="pinId">ID do Pin (do tipo int)</param>
    /// <param name="userId">ID do Usuário (tipo int)</param>
    /// <returns>Nada</returns>
    /// <response code="204">Pin aprovado com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</response>
    /// <response code="404">Pin não encontado</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response>
    [HttpPut("aprovapin/{pinId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> AprovaPinById(int pinId, int userId)
    {
        if (!await _service.PinAproveAsync(pinId, userId)) return NotFound();

        return NoContent();
    }
    /// <summary>
    /// CONCLUDE (Só Fiscal do Pin): Aprovação de Pin pelo seu ID
    /// </summary>
    /// <remarks>Rota para concluir Pin, tranformando seu Status como Concluido. Somente o FISCAL DESTE PIN pode concluir Pin, caso contrário, o código não concluirá a ação</remarks>
    /// <param name="pinId">ID do Pin (do tipo int)</param>
    /// <param name="userId">[Int] ID do Usuário (Precisa ser o FISCAL DO PIN)</param>
    /// <returns>Nada</returns>
    /// <response code="204">Pin concluido com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</response
    /// <response code="404">Pin não encontado</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response>
    [HttpPut("concluipin/{pinId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> ConcluiPinById(int pinId, int userId)
    {
        if (!await _service.PinConcludeAsync(pinId, userId)) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// UPDATE WHOLE: Acesso de atualização de maior privilegio ao Pin
    /// </summary>
    /// <param name="pinId">Identificador (ID) do Pin (tipo int)</param>
    /// <param name="pinDto">Objeto de transferencia de Pin</param>
    /// <returns>Sem conteúdo</returns>
    /// <remarks>Método utilizado para editar a maior parte do objeto</remarks>
    /// <response code="204">Pin editado com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</
    /// <response code="404">Pin não encontado</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response
    [HttpPut("updatewhole/{pinId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWhole(int pinId, PinDTO pinDto)
    {
        if (pinId != pinDto.Id) return BadRequest();

        if (!await _service.UpdateWholeAsync(pinDto)) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// SET POINTS (Só Fiscal do Pin): Define pontuação ao Pin pelo seu ID
    /// </summary>
    /// <remarks>Rota para adicionar pontos ao Pin. Somente o FISCAL DESTE PIN pode realizer essa ação, caso contrário, a ação não será concluida</remarks>
    /// <param name="pinId">[Int] ID do Pin</param>
    /// <param name="userId">[Int] ID do Usuário (Precisa ser o FISCAL DO PIN)</param>
    /// <param name="points">[Int] Quantidade de pontos a ser inserido ao Pin</param>
    /// <returns>Nada</returns>
    /// <response code="204">Pin concluido com sucesso</response>
    /// <response code="400">Solicitação não atende aos requisitos do sistema. Veja mensagem no Postman ou em seu navegador</
    /// <response code="404">Pin não encontado</response>
    /// <response code="500">Erro interno do Servidor. Veja mensagem no Postman ou em seu navegador</response
    [HttpPut("setpoints/{pinId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> SetPoints(int userId, int pinId, int points)
    {
        if (!await _service.SetPointsAsync(userId, pinId, points)) return NotFound();

        return NoContent();
    }
}
