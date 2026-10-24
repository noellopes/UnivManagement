using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class AnoLetivo
    {

        public int IdAnoLetivo { get; set; }

        [Required(ErrorMessage = "O {0} de início é obrigatório. | The start {0} is required.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "O {0} de início deve ter exatamente {1} caracteres. | The start {0} must be exactly {1} characters.")]
        public required string StartYear { get; set; }

        [Required(ErrorMessage = "O {0} de fim é obrigatório. | The end {0} is required.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "O {0} de fim deve ter exatamente {1} caracteres. | The end {0} must be exactly {1} characters.")]
        public required string EndYear { get; set; }

        [StringLength(60, ErrorMessage = "A {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
        public string? Designation { get; set; }

    }
}
