using System.ComponentModel.DataAnnotations;

namespace ecobairroServer.Source.Core.Dto
{
    public class PinDTOShow
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

        public int? FiscalUserId { get; set; }
        public string NomeFiscal { get; set; } = string.Empty;

        [Required]
        public int MunicipeCriadorUserId { get; set; }
        public string NomeMunicipe { get; set; } = string.Empty;
    }
}
