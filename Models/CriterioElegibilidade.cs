using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class CriterioElegibilidade
    {
        
        public int CriterioElegibilidadeId { get; set; }

        
        [Required(ErrorMessage = "Please enter the description.")]
        [StringLength(200)]
        public required string Descricao { get; set; }

        // Maximum income allowed to be eligible for the scholarship
        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Please enter a positive amount.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RendimentoMaximo { get; set; }

        // Minimum number of ECTS credits required
        [Required]
        [Range(1, int.MaxValue,
            ErrorMessage = "ECTS must be a positive number.")]
        public int EctsMinimos { get; set; }
    }
}