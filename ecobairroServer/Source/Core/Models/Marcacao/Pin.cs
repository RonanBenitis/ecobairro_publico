using ecobairroServer.Source.Core.Models.Interface;
using ecobairroServer.Source.Core.Models.Pessoa;
using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Models.Marcacao;

public class Pin : IId
{
    public int Id { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A pontuação não pode ser negativa.")]
    public int? Pontuacao { get; set; }

    public string? Descricao { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Latitude { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Longitude { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Endereco { get; set; } = string.Empty;

    // Relacionamento Fiscal (pode NULL)
    // FK
    public int? FiscalId { get; set; }
    // Navigation Property ao fiscal que administra este pin
    public Fiscal? Fiscal { get; set; }

    // Relacionamento Municipe
    // FK
    [Required]
    public int MunicipeCriadorId { get; set; }
    public Municipe? MunicipeCriador { get; set; }

    // Relacionamento ChamadasPin
    // Navigation Property para as chamadas feitas a este Pin
    public List<ChamadasPin> ChamadasPins { get; set; } = [];

    [Required]
    public int BairroId { get; set; }
    public Bairro? Bairro { get; set; }
}
