
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoCandidatura
    {
        [Key]
        public int IdTipoCandidatura { get; set; }

        [Required]
        [StringLength(200)]
        public required string Designacao { get; set; }

        [Required]
        [StringLength(200)]
        public required string Descricao { get; set; }
    }
}
