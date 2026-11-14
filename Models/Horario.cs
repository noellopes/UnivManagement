using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Horario
    {
        public int HorarioId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Ano Letivo")]
        [StringLength(9, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public required string AnoLetivo { get; set; }
    }
}