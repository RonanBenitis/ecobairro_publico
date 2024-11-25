using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Dto;

public class PinDTOCreate
{
    public int Id { get; set; }

    [Required]
    public int MunicipeCriadorUserId { get; set; }

    [Required]
    [StringLength(200)]
    public string Latitude { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Longitude { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Endereco { get; set; } = string.Empty;

    public string? Descricao { get; set; } = string.Empty;
}
