using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UnivManagement.Models
{
    public class Emprestimo : IValidatableObject
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
        [StringLength(50)]
        [Display(Name = "Estado")]
        public string estado { get; set; } = "Ativo";

        [Required(ErrorMessage = "Deve selecionar um exemplar.")]
        [Display(Name = "Exemplar")]
        public int ExemplarId { get; set; }

        public virtual Exemplar? Exemplar { get; set; }

        // Validação personalizada de regras de negócio
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (dataDevolucao < dataEmprestimo)
            {
                yield return new ValidationResult(
                    "A data de devolução não pode ser anterior à data de empréstimo.",
                    new[] { nameof(dataDevolucao) }
                );
            }
        }
    }
}