using ecobairroServer.Source.Core.Models.Interface;
using ecobairroServer.Source.Core.Services.Interface;
using ecobairroServer.Source.Inbound.Controllers.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ecobairroServer.Source.Inbound.Controllers.Abstraction;

public class CrudController<T> : ControllerBase, ICrudController<T> where T : class, IId
{
    protected readonly ICrudService<T> _service;

    public CrudController(ICrudService<T> service)
    {
        _service = service;
    }

    /// <summary>
    /// READ ALL: Obter a lista de objetos desta classe específica
    /// </summary>
    /// <returns>Lista de objetos</returns>
    /// <remarks>Chame este endpoint para obter a lista de objetos desta classe específica que encontra-se armaenada no banco de dados</remarks>
    /// <response code="200">Ação realizada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// READ: Obter um objeto desta classe específica 
    /// </summary>
    /// <param name="id">Identificador (ID) do objeto (tipo int)</param>
    /// <returns>Dados do objeto</returns>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para buscar o objeto desejado da classe especifica deste método</remarks>
    /// <response code="200">Objeto entregue com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<T>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);

        if (entity == null) return NotFound();

        return entity;
    }

    /// <summary>
    /// UPDATE: Atualizar um objeto desta classe específica 
    /// </summary>
    /// <param name="id">Identificador (ID) do objeto (tipo int)</param>
    /// <param name="entity">Objeto do tipo desta classe especifica (Ou seja, se você estiver chamando o endpoint de Municipe, este parametro aguarda o tipo "Municipe")</param>
    /// <returns>Sem conteúdo</returns>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para identificar o objeto que deseja-se atualizar. Como segundo parâmetro, passa-se um objeto (de mesmo id) com os novos dados a serem armazenados.</remarks>
    /// <response code="204">Objeto entregue com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, T entity)
    {
        if (id != entity.Id) return BadRequest();

        if (!await _service.UpdateAsync(entity)) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// CREATE: Armazena um novo objeto da classe específica no banco de dados
    /// </summary>
    /// <param name="entity">Objeto do tipo desta classe especifica (Ou seja, se você estiver chamando o endpoint de Municipe, este parametro aguarda o tipo "Municipe")</param>
    /// <remarks>Passa-se um objeto que será armazenado na tabela respectiva de sua classe. Como exemplo: Caso você esteja criando Municipe, os dados deste objeto irá para Munícipe.</remarks>
    /// <returns>Informações do objeto criado</returns>
    /// <response code="200">Informações do objeto criado</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<T>> Create(T entity)
    {
        var createdEntity = await _service.AddAsync(entity);

        return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
    }

    /// <summary>
    /// DELETE: Exclui um objeto específico no banco de dados
    /// </summary>
    /// <remarks>Utilize um número inteiro (type int) como identificador (ID) para excluir o objeto desejado, armazenado na tabela específica de sua classe</remarks>
    /// <param name="id">Identificador (ID) do objeto (tipo int)</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Objeto excluído com sucesso</response>
    /// <response code="404">Objeto não encontado</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id)) return NotFound();

        return NoContent();
    }

    // Método interno para consulta de existência de um objeto no banco
    private async Task<bool> Exists(int id)
    {
        return await _service.ExistsAsync(id);
    }
}