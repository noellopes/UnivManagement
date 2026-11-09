using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models;

namespace UnivManagement.Models
{
    [Table("ClassGroup")]
    public class Group
    {
        public int GroupId { get; set; }

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

       
    }
}
