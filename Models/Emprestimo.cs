using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Emprestimo
    {
        [Key]
        public int EmprestimoId { get; set; }

        [Required(ErrorMessage = "A data de empréstimo é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Empréstimo")]
        public DateTime dataEmprestimo { get; set; }

        [Required(ErrorMessage = "A data de devolução é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Devolução")]
        public DateTime dataDevolucao { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(20, ErrorMessage = "O estado não pode ter mais de 20 caracteres.")]
        [RegularExpression("^(Ativo|Devolvido)$", ErrorMessage = "O estado tem de ser 'Ativo' ou 'Devolvido'.")]
        [Display(Name = "Estado")]
        public string estado { get; set; } = "Ativo";

        [Required(ErrorMessage = "Tem de selecionar um exemplar.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um exemplar válido.")]
        [Display(Name = "Exemplar")]
        public int ExemplarId { get; set; }

        [ForeignKey(nameof(ExemplarId))]
        public Exemplar? Exemplar { get; set; }
    }
}