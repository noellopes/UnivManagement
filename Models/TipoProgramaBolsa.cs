
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoProgramaBolsa
    {
        [Key]
        public int IdTipoPrograma { get; set; }

        [Required]
        [StringLength(200)]
        public required string NomeTipo { get; set; }

        [Required]
        [StringLength(200)]
        public required string Descricao { get; set; }
    }
}
