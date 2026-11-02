
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ProgramaBolsa
    {
        [Key]
        public int IdPrograma { get; set; }

        [Required(ErrorMessage = "O nome do programa é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome não pode exceder 200 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O ano letivo é obrigatório.")]
        [StringLength(20, ErrorMessage = "O ano letivo não pode exceder 20 caracteres.")]
        public required string AnoLetivo { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Range(typeof(decimal), "0.01", "999999999",
            ErrorMessage = "O valor máximo deve ser superior a zero.")]
        public decimal ValorMaximoBolsa { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Selecione um tipo de programa.")]
        public int IdTipoPrograma { get; set; }
    }
}
