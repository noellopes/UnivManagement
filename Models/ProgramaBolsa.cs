
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ProgramaBolsa
    {
        [Key]
        public int IdPrograma { get; set; }

        [Required]
        [StringLength(200)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(500)]
        public required string Descricao { get; set; }

        [Required]
        [StringLength(20)]
        public required string AnoLetivo { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Range(typeof(decimal), "0", "999999999")]
        public decimal ValorMaximoBolsa { get; set; }

        public int IdTipoPrograma { get; set; }
    }
}
