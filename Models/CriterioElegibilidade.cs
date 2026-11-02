
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class CriterioElegibilidade
    {
        [Key]
        public int IdCriterio { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "999999999",
            ErrorMessage = "The maximum income must be positive.")]
        public decimal RendimentoMaximo { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "The minimum ECTS cannot be negative.")]
        public int EctsMinimos { get; set; }

        public int IdPrograma { get; set; }
    }
}
