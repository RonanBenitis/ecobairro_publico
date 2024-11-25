using ecobairroServer.Source.Core.Models.Marcacao;
using ecobairroServer.Source.Core.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Models.Pessoa;

/*
 * NOTAS DE APRENDIZADO
 * 
 * SOBRE NULO E INICIANDO VAZIO
 * Melhor inicializar vazio do que nulo por ser mais seguro,
 * evitando possíveis exceções de referência nula.
 * 
 * NO CASO DAS LISTAS
 * Caso optássemos por deixá-las nulas, poderiamos receber a
 * exceção de referência nula. Isso só é interessante se nossa
 * regra de negócio trabalha com a diferença entre listas
 * vazias e listas nulas. No nosso caso, podemos ter valores internos
 * a lista ou não, então, a existência de uma lista vazia
 * não nos prejudica pois nosso código não quer saber se é nulo ou
 * vazio, simplesmente quer acessar os valores caso houver algum.
 * 
 * NO CASO DE TIPOS PRIMITIVOS
 * Tipos primitivos, por padrão, são NOT NULL com ou sem Required.
 * Caso queira que ele aceite nulo, utilize o operador ?.
 * Obs: string não é primitivo, é referencial
 * Obs2: Tipos referenciais, por padrão, entram nulos.
 */

public class User : IId
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Role { get; set; } = string.Empty;
}
