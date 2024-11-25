using ecobairroServer.Source.Core.Models.Interface;
using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Models;

public class Bairro : IId
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public int Pontuacao { get; set; }
}
