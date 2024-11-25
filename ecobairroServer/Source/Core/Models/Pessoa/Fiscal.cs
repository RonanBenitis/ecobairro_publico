using System.ComponentModel.DataAnnotations;
using ecobairroServer.Source.Core.Models.Interface;
using ecobairroServer.Source.Core.Models.Marcacao;

namespace ecobairroServer.Source.Core.Models.Pessoa;

public class Fiscal : IId
{
    public int Id { get; set; }

    // Navigation Property para os Pins administrados pelo Fiscal
    public List<Pin> PinsAdministrados { get; set; } = [];

    [Required]
    [StringLength(10)]
    public string Rgf { get; set; } = string.Empty;

    // FK User
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
}
