using ecobairroServer.Source.Core.Models.Interface;
using ecobairroServer.Source.Core.Models.Pessoa;
using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Models.Marcacao;

public class ChamadasPin : IId
{
    public int Id { get; set; }

    // Relacionamento Municipe
    [Required]
    public int MunicipeId { get; set; }
    public required Municipe Municipe { get; set; }

    // Relacionamento Pin
    [Required]
    public int PinId { get; set; }
    public required Pin Pin { get; set; }
}
