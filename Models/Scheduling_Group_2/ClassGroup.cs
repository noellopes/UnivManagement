using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.Scheduling_Group_2
{
    public class ClassGroup
    {
        [Key]
        public int IdTurma { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(50, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Nome da Turma")]
        public required string NomeTurma { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Range(1, 500, ErrorMessage = "O {0} deve ser no mínimo {1} e no máximo {2}.")]
        [Display(Name = "Número de Alunos")]
        public int NumeroAlunos { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();
    }
}
