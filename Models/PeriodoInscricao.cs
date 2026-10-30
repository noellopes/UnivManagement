using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class PeriodoInscricao
    {
        public int PeriodoInscricaoId { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [StringLength(150, ErrorMessage = "A {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Designação do Período")]
        public required string Designacao { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Data de Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Display(Name = "Período Ativo")]
        public bool Ativo { get; set; } = true;
    }
}