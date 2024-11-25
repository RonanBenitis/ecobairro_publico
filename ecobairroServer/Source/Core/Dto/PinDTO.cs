using ecobairroServer.Source.Core.Models.Interface;
using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Dto;

public class PinDTO : IId
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
}
