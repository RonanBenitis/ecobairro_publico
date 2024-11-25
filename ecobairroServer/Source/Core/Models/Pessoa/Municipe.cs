using ecobairroServer.Source.Core.Models.Interface;
using ecobairroServer.Source.Core.Models.Marcacao;
using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Models.Pessoa;

public class Municipe : IId
{
    public int Id { get; set; }

    // Navigation Property para os pins criados pelo Municipe (N:1)
    public List<Pin> PinsCriados { get; set; } = new();

    // Navigation Property para os pins chamados pelo Municipe (N:1 tabela relacional 1:N)
    public List<ChamadasPin> PinsChamados { get; set; } = new();

    [Required]
    [StringLength(11)]
    public string Cpf { get; set; } = string.Empty;

    // FK User
    [Required]
    public int UserId { get; set; }
    public required User User { get; set; }

}
